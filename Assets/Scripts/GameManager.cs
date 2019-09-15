using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public const int GAME_START_COUNT_DOWN_TIME = 3;
    public const float GAME_TIME = 120;
    public const int GAME_OVER_TIME = 3;


    [SerializeField]
    private GridColorManager m_GridColorManager;

    public GridColorManager GridColorManager { get { return m_GridColorManager; } }
    public InputManager InputManager { get; private set; }
    public LogicManager LogicManager { get; private set; }

    public GameManagerState CurrentState { get; private set; }
    private GameManagerState m_RequestedState;

    public bool IsSinglePlay { get; private set; }

    private Action<int> m_GameStartCountDownCallBack;
    public void AddGameStartCountDownCallBack(Action<int> action)
    {
        m_GameStartCountDownCallBack += action;
    }

    private int m_GameStartCountDown;
    private float m_GameStartCountDownTime;

    private float m_GameOverTime;

    public float RemainTime { get; private set; }

    public static GameManager Instance {
        get;
        private set;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyはGameManagerのスコープによって処理するかどうか決める
            // 暫定ではスコープは、InGameのみとする
            //DontDestroyOnLoad(gameObject);
            OnAwake();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Destroy(gameObject);
        }
    }

    private void OnAwake()
    {
        IsSinglePlay = PlayerPrefs.GetInt(ConstKeys.IS_SINGLE_PLAY_MODE_KEY, 0) == 0;

        InputManager = new InputManager();
        LogicManager = new LogicManager();
    }

    private void Start()
    {
        CurrentState = GameManagerState.None;
        RequestState(GameManagerState.GameStart);

        InputManager.OnStart();
        LogicManager.OnStart();
        m_GridColorManager.OnStart();
    }

    private void Update()
    {
        // ステートの変更
        if (m_RequestedState != GameManagerState.None)
        {
            if (CurrentState != GameManagerState.None)
            {
                OnEndState();
            }

            CurrentState = m_RequestedState;
            m_RequestedState = GameManagerState.None;

            if (CurrentState != GameManagerState.None)
            {
                OnStartState();
            }
        }

        OnUpdateState();
    }

    public void RequestState(GameManagerState state)
    {
        m_RequestedState = state;
    }

    #region StateMachine

    private void OnStartState()
    {
        switch (CurrentState)
        {
            case GameManagerState.None:
                break;
            case GameManagerState.GameStart:
                m_GameStartCountDown = GAME_START_COUNT_DOWN_TIME;
                m_GameStartCountDownTime = 0;
                break;
            case GameManagerState.Game:
                Debug.Log("Game Start!");
                RemainTime = GAME_TIME;
                break;
            case GameManagerState.GameEnd:
                Debug.Log("Game End!");
                m_GameOverTime = GAME_OVER_TIME;
                break;
        }
        LogicManager.OnStateState();
        GridColorManager.OnStartState();
    }

    private void OnUpdateState()
    {
        switch (CurrentState)
        {
            case GameManagerState.None:
                break;
            case GameManagerState.GameStart:
                OnUpdateGameStart();
                break;
            case GameManagerState.Game:
                OnUpdateGame();
                break;
            case GameManagerState.GameEnd:
                OnUpdateGameEnd();
                break;
        }
    }

    private void OnEndState()
    {
        switch (CurrentState)
        {
            case GameManagerState.None:
                break;
            case GameManagerState.GameStart:
                break;
            case GameManagerState.Game:
                break;
            case GameManagerState.GameEnd:
                break;
        }
        LogicManager.OnEndState();
    }

    #region UpdateState

    private void OnUpdateGameStart()
    {
        m_GameStartCountDownTime -= Time.deltaTime;
        if (m_GameStartCountDownTime <= 0)
        {
            // カウントダウンが0になった1秒後にゲームへと遷移する
            if (m_GameStartCountDown < 0)
            {
                RequestState(GameManagerState.Game);
            }

            m_GameStartCountDownCallBack?.Invoke(m_GameStartCountDown);
            m_GameStartCountDown--;
            m_GameStartCountDownTime = 1;
        }
        m_GridColorManager.OnUpdate();
    }

    private void OnUpdateGame()
    {
        RemainTime -= Time.deltaTime;
        if (RemainTime < 0)
        {
            RemainTime = 0;
        }

        InputManager.OnUpdate();
        LogicManager.OnUpdate();
        m_GridColorManager.OnUpdate();

        var playerState = LogicManager.FieldDataPlayer1.CurrentState;
        if (playerState == UnitFieldState.GameOver || playerState == UnitFieldState.TimeUp)
        {
            RequestState(GameManagerState.GameEnd);
        }

        if (!IsSinglePlay)
        {
            // 対戦時は2人目の状態も見る
            playerState = LogicManager.FieldDataPlayer2.CurrentState;
            if (playerState == UnitFieldState.GameOver || playerState == UnitFieldState.TimeUp)
            {
                RequestState(GameManagerState.GameEnd);
            }
        }
    }

    private void OnUpdateGameEnd()
    {
        m_GameOverTime -= Time.deltaTime;
        if (m_GameOverTime <= 0)
        {
            if (!IsSinglePlay)
            {
                JudgeWinner();
            }
            SceneManager.LoadScene("Title");
        }
    }

    #endregion

    #endregion

    private void JudgeWinner()
    {
        PlayerPrefs.SetInt(ConstKeys.WINNER, -1);

        int winner = -1;
        var p1Field = LogicManager.FieldDataPlayer1;
        var p2Field = LogicManager.FieldDataPlayer2;
        if (p1Field.CurrentState == UnitFieldState.GameOver)
        {
            winner = 2;
        }
        else if (p2Field.CurrentState == UnitFieldState.GameOver)
        {
            winner = 1;
        }
        else
        {
            if (p1Field.Score > p2Field.Score)
            {
                winner = 1;
            }
            else if (p1Field.Score < p2Field.Score)
            {
                winner = 2;
            }
            else
            {
                // 引き分け
                winner = 0;
            }
        }
        PlayerPrefs.SetInt(ConstKeys.WINNER, winner);
    }

    /// <summary>
    /// 時間切れかどうか
    /// </summary>
    public bool IsTimeUp()
    {
        return RemainTime <= 0;
    }
}
