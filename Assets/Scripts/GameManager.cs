using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GridColorManager m_GridColorManager;

    public GridColorManager GridColorManager { get { return m_GridColorManager; } }
    public InputManager InputManager { get; private set; }
    public LogicManager LogicManager { get; private set; }

    public GameManagerState CurrentState { get; private set; }
    private GameManagerState m_RequestedState;

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
        InputManager = new InputManager();
        LogicManager = new LogicManager();
    }

    private void Start()
    {
        CurrentState = GameManagerState.None;
        RequestState(GameManagerState.Input);

        InputManager.OnStart();
        LogicManager.OnStart(true);
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

    private void OnStartState()
    {
        switch (CurrentState)
        {

        }
    }

    private void OnUpdateState()
    {
        switch (CurrentState)
        {

        }
        InputManager.OnUpdate();
        LogicManager.OnUpdate();
        m_GridColorManager.OnUpdate();
    }

    private void OnEndState()
    {
        switch (CurrentState)
        {

        }
    }
}
