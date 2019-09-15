using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// フィールドとプレイヤーが動かすミノのデータなどを管理する。
/// </summary>
public class UnitFieldData
{
    /// <summary>
    /// フィールド全体の横幅
    /// </summary>
    public const int FILED_WIDTH = 8;

    /// <summary>
    /// フィールド全体の縦幅
    /// </summary>
    public const int FIELD_HEIGHT = 19;

    /// <summary>
    /// 上の隠れる分のオフセット
    /// </summary>
    public const int FIELD_TOP_OFFSET = 3;

    public const float INPUT_TIME_PERIOD = 0.5f;
    public const float AUTO_DROP_TIME_PERIOD = 0.1f;


    public UnitData[,] Units { get; private set; }
    public MinoData CurrentMino { get; private set; }
    public MinoData NextMino { get; private set; }
    public MinoData NextNextMino { get; private set; }

    public int Score { get; private set; }

    public List<ComboData> ComboDataList { get; private set; }

    public UnitFieldState CurrentState { get; private set; }
    private UnitFieldState m_RequestedState;

    /// <summary>
    /// これはプレイヤー1のデータである
    /// </summary>
    private bool m_IsPlayer1;

    private MinoGenerator m_MinoGenerator;

    private float m_AutoDropTimeCount;

    private int m_ComboCount;

    private Action m_StartStateAction;
    public void AddStartStateAction(Action action)
    {
        m_StartStateAction += action;
    }

    public UnitFieldData(bool isPlayer1)
    {
        m_IsPlayer1 = isPlayer1;
    }

    public void OnStart()
    {
        Units = new UnitData[FIELD_HEIGHT, FILED_WIDTH];
        for (var i = 0; i < FIELD_HEIGHT; i++)
        {
            for (var j = 0; j < FILED_WIDTH; j++)
            {
                Units[i, j] = new UnitData();
            }
        }

        m_MinoGenerator = new MinoGenerator(GameManager.Instance.MinoGenerateRoundData);

        CurrentMino = m_MinoGenerator.GenerateMino();
        NextMino = m_MinoGenerator.GenerateMino();
        NextNextMino = m_MinoGenerator.GenerateMino();

        m_AutoDropTimeCount = 0;
        ComboDataList = null;
        CurrentMino.Pos = new Vector2Int((FILED_WIDTH - MinoData.MINO_HEIGHT) / 2, 0);
        Score = 0;
    }

    public void OnStartState()
    {
        var gameState = GameManager.Instance.CurrentState;

        switch (gameState)
        {
            case GameManagerState.GameStart:
                break;
            case GameManagerState.Game:
                RequestState(UnitFieldState.Input);
                break;
            case GameManagerState.GameEnd:
                break;
        }
    }

    public void OnUpdate()
    {
        var gameState = GameManager.Instance.CurrentState;

        switch (gameState)
        {
            case GameManagerState.GameStart:
                break;
            case GameManagerState.Game:
                // ステートの変更
                if (m_RequestedState != UnitFieldState.None)
                {
                    if (CurrentState != UnitFieldState.None)
                    {
                        OnEndStateAtGame();
                    }

                    CurrentState = m_RequestedState;
                    m_RequestedState = UnitFieldState.None;

                    if (CurrentState != UnitFieldState.None)
                    {
                        OnStartStateAtGame();
                    }
                }

                OnUpdateStateAtGame();
                break;
            case GameManagerState.GameEnd:
                break;
        }
    }

    public void OnEndState()
    {
        var gameState = GameManager.Instance.CurrentState;

        switch (gameState)
        {
            case GameManagerState.GameStart:
                break;
            case GameManagerState.Game:
                break;
            case GameManagerState.GameEnd:
                break;
        }
    }

    public void RequestState(UnitFieldState state)
    {
        m_RequestedState = state;
    }

    #region StateMachine

    private void OnStartStateAtGame()
    {
        switch (CurrentState)
        {
            case UnitFieldState.None:
                break;
            case UnitFieldState.Input:
                OnStartStateAtGameInput();
                break;
            case UnitFieldState.Put:
                OnStartStateAtGamePut();
                break;
            case UnitFieldState.AutoDrop:
                OnStartStateAtGameAutoDrop();
                break;
            case UnitFieldState.ChangeColor:
                OnStartStateAtGameChangeColor();
                break;
            case UnitFieldState.StartLaser:
                OnStartStateAtGameStartLaser();
                break;
            case UnitFieldState.EndLaser:
                OnStartStateAtGameEndLaser();
                break;
            case UnitFieldState.Delete:
                OnStartStateAtGameDelete();
                break;
            case UnitFieldState.CheckGameOver:
                OnStartStateAtGameCheckGameOver();
                break;
            case UnitFieldState.GameOver:
                OnStartStateAtGameGameOver();
                break;
            case UnitFieldState.TimeUp:
                OnStartStateAtGameTimeUp();
                break;
        }

        m_StartStateAction?.Invoke();
    }

    private void OnUpdateStateAtGame()
    {
        switch (CurrentState)
        {
            case UnitFieldState.Input:
                OnUpdateStateAtGameInput();
                break;
            case UnitFieldState.AutoDrop:
                OnUpdateStateAtGameAutoDrop();
                break;
            default:
                break;
        }
    }

    private void OnEndStateAtGame()
    {
        switch (CurrentState)
        {
            case UnitFieldState.None:
                break;
            case UnitFieldState.Input:
                break;
            case UnitFieldState.Put:
                break;
            case UnitFieldState.AutoDrop:
                break;
            case UnitFieldState.ChangeColor:
                break;
            case UnitFieldState.Delete:
                break;
            case UnitFieldState.CheckGameOver:
                break;
            case UnitFieldState.GameOver:
                break;
        }
    }

    #region StartStateAtGame

    private void OnStartStateAtGameInput()
    {
        m_ComboCount = 0;
        m_AutoDropTimeCount = 0;
    }

    private void OnStartStateAtGamePut()
    {
        ComboDataList = null;

        if (CheckExistAutoDropUnit())
        {
            RequestState(UnitFieldState.AutoDrop);
        }
        else
        {
            RequestState(UnitFieldState.ChangeColor);
        }
    }

    private void OnStartStateAtGameAutoDrop()
    {
        m_AutoDropTimeCount = 0;
        ResetInputColorData();
    }

    private void OnStartStateAtGameChangeColor()
    {
        if (CheckExistLaserBlock())
        {
            // 点火していないレーザーが存在している場合は点火ステートに遷移する
            var laserState = GetAllLaserState();
            if ((laserState & LaserState.Prepare) == LaserState.Prepare)
            {
                RequestState(UnitFieldState.StartLaser);
                return;
            }
            else if ((laserState & LaserState.Fire) == LaserState.Fire)
            {
                Debug.Log("ApplyLaser");
                ApplyLaserColor();
            }
        }

        Debug.Log("Mix");
        MixUnitColor();
        if (CheckExistWhileUnit())
        {
            RequestState(UnitFieldState.Delete);
        }
        else
        {
            // 点火しているレーザーが存在している場合は消火ステートに遷移する
            if (CheckExistLaserBlock())
            {
                var laserState = GetAllLaserState();
                if ((laserState & LaserState.Fire) == LaserState.Fire)
                {
                    RequestState(UnitFieldState.EndLaser);
                    return;
                }
            }

            RequestState(UnitFieldState.CheckGameOver);
        }
    }

    private void OnStartStateAtGameStartLaser()
    {
        Debug.LogWarning("StartLaser");
        SetAllLaserState(LaserState.Fire);
        RequestState(UnitFieldState.ChangeColor);
    }

    private void OnStartStateAtGameEndLaser()
    {
        // Noneにすることでレーザーを通常ブロックとして扱うようにする
        Debug.LogWarning("EndLaser");
        SetAllLaserState(LaserState.None);
        ResetAllLaserColor();
        RequestState(UnitFieldState.ChangeColor);
    }

    private void OnStartStateAtGameDelete()
    {
        ComboDataList = DeleteWhiteUnit();
        RequestState(UnitFieldState.Put);
    }

    private void OnStartStateAtGameCheckGameOver()
    {
        // 時間切れならタイムアップに遷移
        if (GameManager.Instance.IsTimeUp())
        {
            RequestState(UnitFieldState.TimeUp);
            return;
        }

        if (CheckPiledUpToTop())
        {
            RequestState(UnitFieldState.GameOver);
        }
        else
        {
            // ミノをずらして入力待ちに戻る
            CurrentMino = NextMino;
            NextMino = NextNextMino;
            NextNextMino = m_MinoGenerator.GenerateMino();
            CurrentMino.Pos = new Vector2Int((FILED_WIDTH - MinoData.MINO_HEIGHT) / 2, 0);

            RequestState(UnitFieldState.Input);
        }
    }

    private void OnStartStateAtGameGameOver()
    {
        GameManager.Instance.RequestState(GameManagerState.GameEnd);
    }

    private void OnStartStateAtGameTimeUp()
    {

    }

    #endregion

    #region UpdateStateAtGame

    private void OnUpdateStateAtGameInput()
    {
        InputManagerState inputState;
        if (m_IsPlayer1)
        {
            inputState = GameManager.Instance.InputManager.inputPlayer1;
        }
        else
        {
            inputState = GameManager.Instance.InputManager.inputPlayer2;
        }

        switch (inputState)
        {
            case InputManagerState.Left:
                MoveLeft();
                break;
            case InputManagerState.Right:
                MoveRight();
                break;
            case InputManagerState.Rotate:
                Rotate();
                break;
            case InputManagerState.SoftDrop:
                SoftDrop();
                break;
            case InputManagerState.HardDrop:
                HardDrop();
                break;
            case InputManagerState.None:
                // 何もしない
                break;
        }

        // 自由落下も行う
        m_AutoDropTimeCount += Time.deltaTime;
        if (m_AutoDropTimeCount >= INPUT_TIME_PERIOD)
        {
            m_AutoDropTimeCount = 0;
            SoftDrop();
        }

        // 操作と自由落下の処理が終わったら、着地判定を行う
        if (CheckMinoPut())
        {
            DetermineMinoPos();
            RequestState(UnitFieldState.Put);
        }
    }

    private void OnUpdateStateAtGameAutoDrop()
    {
        m_AutoDropTimeCount += Time.deltaTime;
        if (m_AutoDropTimeCount >= AUTO_DROP_TIME_PERIOD)
        {
            m_AutoDropTimeCount = 0;
            UpdateAutoDropUnit();

            // 自動落下するものが無くなったら遷移する
            if (!CheckExistAutoDropUnit())
            {
                RequestState(UnitFieldState.ChangeColor);
            }
        }
    }

    #endregion

    #region EndStateAtGame
    #endregion

    #endregion

    /// <summary>
    /// 指定した座標がフィールドからはみ出しているかどうか。
    /// はみ出していたらtrueを返す。
    /// </summary>
    private bool IsOutOfField(int x, int y)
    {
        return x < 0 || x >= FILED_WIDTH || y < 0 || y >= FIELD_HEIGHT;
    }

    /// <summary>
    /// 指定した座標にミノがある時、はみ出していないかをチェックする。
    /// はみ出していたらfalseを返す。
    /// </summary>
    private bool CheckMinoProtrude(int checkX, int checkY)
    {
        if (CurrentMino == null)
        {
            return false;
        }

        for (var i = 0; i < MinoData.MINO_HEIGHT; i++)
        {
            for (var j = 0; j < MinoData.MINO_WIDTH; j++)
            {
                var minoUnit = CurrentMino.Units[i, j];
                if (minoUnit.CurrentColor == ColorData.None)
                {
                    continue;
                }

                // はみ出していたらアウト
                var actX = j + checkX;
                var actY = i + checkY;
                if (IsOutOfField(actX, actY))
                {
                    return false;
                }

                try
                {
                    // フィールド上のブロックが存在すればアウト
                    var fieldUnit = Units[actY, actX];
                    if (fieldUnit.CurrentColor != ColorData.None)

                        if (fieldUnit.CurrentColor != ColorData.None)

                        {
                            return false;
                        }
                }
                catch (System.Exception)
                {
                    Debug.LogErrorFormat("x:{0}, y:{1}", actX, actY);
                }
            }
        }
        return true;
    }

    /// <summary>
    /// ミノを左に動かす
    /// </summary>
    private void MoveLeft()
    {
        var pos = CurrentMino.Pos;
        if (CheckMinoProtrude(pos.x - 1, pos.y))
        {
            pos.x--;
            CurrentMino.Pos = pos;
        }
    }

    /// <summary>
    /// ミノを右に動かす
    /// </summary>
    private void MoveRight()
    {
        var pos = CurrentMino.Pos;
        if (CheckMinoProtrude(pos.x + 1, pos.y))
        {
            pos.x++;
            CurrentMino.Pos = pos;
        }
    }

    /// <summary>
    /// ミノを時計回転させる
    /// </summary>
    private void Rotate()
    {
        CurrentMino.Rotate();
    }

    /// <summary>
    /// ミノを下に動かす
    /// </summary>
    private void SoftDrop()
    {
        var pos = CurrentMino.Pos;
        if (CheckMinoProtrude(pos.x, pos.y + 1))
        {
            pos.y++;
            CurrentMino.Pos = pos;
        }
    }

    /// <summary>
    /// ミノを落とせる場所まで落とす
    /// </summary>
    private void HardDrop()
    {
        var pos = CurrentMino.Pos;
        while (CheckMinoProtrude(pos.x, pos.y + 1))
        {
            pos.y++;
        }
        CurrentMino.Pos = pos;
    }


    /// <summary>
    /// ミノが着地したかどうかを判定する
    /// 着地していたらtrueを返す
    /// </summary>
    public bool CheckMinoPut()
    {
        if (CurrentMino == null)
        {
            return false;
        }

        var pos = CurrentMino.Pos;
        for (var x = 0; x < MinoData.MINO_WIDTH; x++)
        {
            int y = MinoData.MINO_HEIGHT - 1;
            for (; y >= 0; y--)
            {
                var unit = CurrentMino.Units[y, x];
                if (unit.CurrentColor != ColorData.None)
                {
                    break;
                }
            }

            // 縦方向にブロックが無いので、スルー
            if (y < 0)
            {
                continue;
            }

            var actX = x + pos.x;
            var actY = y + pos.y + 1;
            if (actY >= FIELD_HEIGHT || Units[actY, actX].CurrentColor != ColorData.None)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// ミノの着地点を確定する
    /// </summary>
    private void DetermineMinoPos()
    {
        if (CurrentMino == null)
        {
            return;
        }

        var pos = CurrentMino.Pos;
        for (var i = 0; i < MinoData.MINO_HEIGHT; i++)
        {
            for (var j = 0; j < MinoData.MINO_WIDTH; j++)
            {
                var unit = CurrentMino.Units[i, j];
                if (unit.CurrentColor == ColorData.None)
                {
                    continue;
                }

                var actX = j + pos.x;
                var actY = i + pos.y;
                Units[actY, actX] = unit;
            }
        }
    }

    /// <summary>
    /// 自動落下するブロックが存在するかどうかを判定する
    /// 存在する場合はtrueを返す
    /// </summary>
    public bool CheckExistAutoDropUnit()
    {
        for (var i = FIELD_HEIGHT - 1; i >= 1; i--)
        {
            for (var j = 0; j < FILED_WIDTH; j++)
            {
                var unit = Units[i, j];
                var aboveUnit = Units[i - 1, j];
                var unitColor = unit.CurrentColor;
                var aboveColor = aboveUnit.CurrentColor;

                if (aboveUnit.GetOverrappedColor() == ColorData.None)
                {
                    continue;
                }

                if (aboveColor != ColorData.None && aboveUnit.InputColor == ColorData.None)
                {
                    if ((unitColor & aboveColor) == ColorData.None)
                    {
                        Debug.LogWarning("Exist Auto Drop");
                        return true;
                    }
                }
                else
                {
                    Debug.LogFormat("pos:{0}, abovePos:{1}, unitColor:{2}, unitInColor:{4}, aboveColor:{3}, aboveInColor:{5}",
                        new Vector2Int(j, i), new Vector2Int(j, i - 1), unitColor, aboveColor, unit.InputColor, aboveUnit.InputColor);
                    if ((unitColor & aboveUnit.InputColor) == ColorData.None)
                    {
                        if ((unit.InputColor & aboveUnit.InputColor) == ColorData.None)
                        {
                            Debug.LogWarning("Exist Auto Drop");
                            return true;
                        }
                    }
                }

                //// 直上のブロックと現在のブロックの成分が重なったら自動落下するブロックである
                //var multiColor = (unit.GetOverrappedColor() & aboveUnit.GetOverrappedColor());
                //var subColor = aboveUnit.GetOverrappedColor() & ~multiColor;

                //Debug.LogFormat("pos:{0}, abovePos:{1}, unitColor:{2}, unitInColor:{6}, aboveColor:{3}, aboveInColor:{7}, multiColor:{4}, subColor:{5}",
                //    new Vector2Int(j, i), new Vector2Int(j, i-1), unitColor, aboveColor, multiColor, subColor, unit.InputColor, aboveUnit.InputColor);

                //if (subColor != ColorData.None)
                //{
                //    Debug.LogWarning("Exist Auto Drop");
                //    return true;
                //}
            }
        }

        Debug.LogWarning("Non-Exist Auto Drop");
        return false;
    }

    /// <summary>
    /// 落下による色の合成フラグをリセットする
    /// </summary>
    public void ResetInputColorData()
    {
        for (var i = 0; i < FIELD_HEIGHT; i++)
        {
            for (var j = 0; j < FILED_WIDTH; j++)
            {
                Units[i, j].SetInputData(ColorData.None, LaserState.None);
            }
        }
    }

    /// <summary>
    /// 自動落下するブロックを1マス分だけ落とす
    /// </summary>
    public void UpdateAutoDropUnit()
    {
        for (var i = FIELD_HEIGHT - 2; i >= 0; i--)
        {
            for (var j = 0; j < FILED_WIDTH; j++)
            {
                var unit = Units[i, j];
                var bottomUnit = Units[i + 1, j];

                // 対象のブロックが有色で、かつ、下のブロックと同じ成分を持たない時
                if (unit.CurrentColor != ColorData.None && (unit.CurrentColor & bottomUnit.CurrentColor) == ColorData.None)
                {
                    bottomUnit.SetInputData(unit.CurrentColor, unit.CurrentLaserState);
                    unit.SetCurrentData(ColorData.None, LaserState.None);
                }

                // 対象のブロックの自動落下が有色で、かつ、下のブロックと同じ成分を持たない時
                else if (unit.InputColor != ColorData.None && (unit.InputColor & bottomUnit.GetDisplayColor()) == ColorData.None)
                {
                    bottomUnit.SetInputData(unit.InputColor, unit.InputLaserState);
                    unit.SetInputData(ColorData.None, LaserState.None);
                }
            }
        }
    }

    /// <summary>
    /// ブロックの色を混ぜ合わせる
    /// この辺は、レーザーが1つしか存在しないことを前提として簡素化した処理になっています
    /// 今後、同時に複数のレーザーが存在するような場合は厄介になります
    /// </summary>
    public void MixUnitColor()
    {
        for (var i = FIELD_HEIGHT - 1; i >= 0; i--)
        {
            for (var j = 0; j < FILED_WIDTH; j++)
            {
                var unit = Units[i, j];

                // レーザーがある時は混ぜ合わせない
                if (unit.IsExistLaser())
                {
                    Debug.LogErrorFormat("Mix:({0},{1}) ExistLaser", j, i);
                    continue;
                }

                if (unit.GetOverrappedColor() == ColorData.None)
                {
                    unit.SetCurrentData(unit.CurrentColor | unit.InputColor, LaserState.None);
                    unit.SetInputData(ColorData.None, LaserState.None);
                    unit.SetLaserColor(ColorData.None);
                }
                else
                {
                    // 空白じゃないブロックにはレーザー照射色も適用する
                    unit.SetCurrentData(unit.CurrentColor | unit.InputColor | unit.LaserColor, LaserState.None);
                    unit.SetInputData(ColorData.None, LaserState.None);
                    unit.SetLaserColor(ColorData.None);
                }
            }
        }
    }

    /// <summary>
    /// 白ブロックが存在するかどうかを判定する
    /// 存在する場合はtrueを返す
    /// </summary>
    public bool CheckExistWhileUnit()
    {
        for (var i = FIELD_HEIGHT - 1; i >= 0; i--)
        {
            for (var j = 0; j < FILED_WIDTH; j++)
            {
                var unit = Units[i, j];
                if ((unit.CurrentColor & ColorData.White) == ColorData.White)
                {
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// 白ブロックを削除する
    /// </summary>
    /// <returns>削除された白ブロックの座標リスト</returns>
    public List<ComboData> DeleteWhiteUnit()
    {
        List<ComboData> deletePosList = new List<ComboData>();
        for (var i = FIELD_HEIGHT - 1; i >= 0; i--)
        {
            for (var j = 0; j < FILED_WIDTH; j++)
            {
                var unit = Units[i, j];
                if ((unit.CurrentColor & ColorData.White) == ColorData.White)
                {
                    var score = GameManager.SCORE_BASE_POINT + GameManager.SCORE_COMBO_POINT * m_ComboCount;
                    Score += score;

                    var combo = new ComboData();
                    combo.Pos = new Vector2Int(j, i);
                    combo.ComboCount = m_ComboCount;
                    combo.Score = score;
                    deletePosList.Add(combo);
                    unit.InitData();

                    m_ComboCount++;
                }
            }
        }
        return deletePosList;
    }

    /// <summary>
    /// 上まで積み重なっているかどうかを判定する
    /// 積み重なっている場合はtrueを返す
    /// </summary>
    public bool CheckPiledUpToTop()
    {
        for (var i = 0; i < FILED_WIDTH; i++)
        {
            if (Units[FIELD_TOP_OFFSET, i].CurrentColor != ColorData.None)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// レーザーが存在するかどうかを判定する
    /// 存在する場合はtrueを返す
    /// </summary>
    public bool CheckExistLaserBlock()
    {
        for (var i = 0; i < FIELD_HEIGHT; i++)
        {
            for (var j = 0; j < FILED_WIDTH; j++)
            {
                var unit = Units[i, j];
                if (unit.GetDisplayColor() != ColorData.None && unit.IsExistLaser())
                {
                    return true;
                }
            }
        }

        return false;
    }


    /// <summary>
    /// レーザーの場所データを取得する
    /// </summary>
    public List<LaserPositionData> GetLaserPositionDataList()
    {
        var list = new List<LaserPositionData>();
        for (var i = 0; i < FIELD_HEIGHT; i++)
        {
            for (var j = 0; j < FILED_WIDTH; j++)
            {
                var unit = Units[i, j];
                if (unit.GetDisplayColor() != ColorData.None && unit.IsExistLaser())
                {
                    var data = new LaserPositionData();
                    data.Pos = new Vector2Int(j, i);
                    data.LaserUnit = unit;
                    list.Add(data);
                }
            }
        }

        return list;
    }

    /// <summary>
    /// 全てのレーザーにステートをセットする
    /// </summary>
    public void SetAllLaserState(LaserState laserState)
    {
        for (var i = 0; i < FIELD_HEIGHT; i++)
        {
            for (var j = 0; j < FILED_WIDTH; j++)
            {
                var unit = Units[i, j];
                if (unit.CurrentLaserState != LaserState.None)
                {
                    unit.CurrentLaserState = laserState;
                }
                if (unit.InputLaserState != LaserState.None)
                {
                    unit.InputLaserState = laserState;
                }
            }
        }
    }

    /// <summary>
    /// 全てのレーザーのステートを重ね合わせて取得する
    /// </summary>
    public LaserState GetAllLaserState()
    {
        LaserState state = LaserState.None;
        for (var i = 0; i < FIELD_HEIGHT; i++)
        {
            for (var j = 0; j < FILED_WIDTH; j++)
            {
                var unit = Units[i, j];
                if (unit.CurrentLaserState != LaserState.None)
                {
                    state |= unit.CurrentLaserState;
                }
                if (unit.InputLaserState != LaserState.None)
                {
                    state |= unit.InputLaserState;
                }
            }
        }

        return state;
    }

    /// <summary>
    /// レーザーブロックの色を適用する
    /// </summary>
    private void ApplyLaserColor()
    {
        for (var i = 0; i < FIELD_HEIGHT; i++)
        {
            for (var j = 0; j < FILED_WIDTH; j++)
            {
                var unit = Units[i, j];
                if (unit.CurrentLaserState == LaserState.Fire)
                {
                    ApplyLaserColor(j, i, unit.CurrentColor);
                }
                else if (unit.InputLaserState == LaserState.Fire)
                {
                    ApplyLaserColor(j, i, unit.InputColor);
                }
            }
        }
    }

    /// <summary>
    /// レーザー照射色をリセットする
    /// </summary>
    private void ResetAllLaserColor()
    {
        for (var i = 0; i < FIELD_HEIGHT; i++)
        {
            for (var j = 0; j < FILED_WIDTH; j++)
            {
                var unit = Units[i, j];
                if (unit.GetOverrappedColor() == ColorData.None)
                {
                    continue;
                }

                unit.SetLaserColor(ColorData.None);
            }
        }
    }

    /// <summary>
    /// レーザーブロックの色を横列に適用する
    /// </summary>
    private void ApplyLaserColor(int x, int y, ColorData color)
    {
        for (var i = 0; i < FILED_WIDTH; i++)
        {
            if (i == x)
            {
                continue;
            }

            Units[y, i].SetLaserColor(color);
            Debug.LogFormat("x:{0}, y:{1}, c:{2}, disp:{3}", i, y, color, Units[y, i].GetDisplayColor());
        }
    }

    /// <summary>
    /// レーザーブロックから通常ブロックへ戻す
    /// </summary>
    private void ChangeNormalFromLaesr()
    {
        for (var i = 0; i < FIELD_HEIGHT; i++)
        {
            for (var j = 0; j < FILED_WIDTH; j++)
            {
                var unit = Units[i, j];
                if (unit.GetDisplayColor() != ColorData.None && unit.IsExistLaser())
                {

                }
            }
        }
    }

    /// <summary>
    /// 現在のミノにレーザーが存在するかどうか
    /// 存在する場合はtrueを返す
    /// </summary>
    public bool IsLaserMino()
    {
        if (CurrentMino == null)
        {
            return false;
        }

        for (var i = 0; i < MinoData.MINO_HEIGHT; i++)
        {
            for (var j = 0; j < MinoData.MINO_WIDTH; j++)
            {
                var unit = CurrentMino.Units[i, j];
                if (unit.CurrentColor != ColorData.None && unit.IsExistLaser())
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// ミノの落下予測地点座標のリストを取得する
    /// </summary>
    public List<Vector2Int> GetPredictionDropPosList()
    {
        var list = new List<Vector2Int>();
        for (var i = 0; i < MinoData.MINO_HEIGHT; i++)
        {
            for (var j = 0; j < MinoData.MINO_WIDTH; j++)
            {
                var minoUnit = CurrentMino.Units[i, j];
                if (minoUnit.CurrentColor == ColorData.None)
                {
                    continue;
                }

                int predictPos = FIELD_HEIGHT - 1;
                int y = i + 1;
                for (; y < FIELD_HEIGHT; y++)
                {
                    var unit = Units[y, j];
                    if ((minoUnit.CurrentColor & unit.CurrentColor) != ColorData.None)
                    {
                        predictPos = y - 1;
                        break;
                    }
                }

                list.Add(new Vector2Int(j, y));
            }
        }
        return list;
    }
}
