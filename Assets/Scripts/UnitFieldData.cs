using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public const float AUTO_DROP_TIME_PERIOD = 0.5f;


    public UnitData[,] Units { get; private set; }
    public MinoData CurrentMino { get; private set; }
    public MinoData NextMino { get; private set; }
    public MinoData NextNextMino { get; private set; }

    /// <summary>
    /// これはプレイヤー1のデータである
    /// </summary>
    private bool m_IsPlayer1;

    private float m_AutoDropTimeCount;

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

        CurrentMino = GenerateMinoData(false);
        NextMino = GenerateMinoData(false);
        NextNextMino = GenerateMinoData(false);

        m_AutoDropTimeCount = 0;
    }

    public void OnUpdate()
    {
        var gameState = GameManager.Instance.CurrentState;

        // 入力ステート受付
        switch (gameState)
        {
            case GameManagerState.Input:
                // この中で操作に対する処理を行う
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
                if (m_AutoDropTimeCount >= AUTO_DROP_TIME_PERIOD)
                {
                    m_AutoDropTimeCount = 0;
                    SoftDrop();
                }

                // 操作と自由落下の処理が終わったら、着地判定を行う
                if (CheckMinoPut())
                {
                    // ミノの配置を確定する
                    DetermineMinoPos();

                    // ゲームマネージャにPutステートをリクエストする
                }
                break;
            case GameManagerState.Put:
                // 自動落下処理?
                // 多分ここは、Putになった初回のフレームだけ呼び出すことになる
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// ミノ生成(仮置き)
    /// </summary>
    private MinoData GenerateMinoData(bool isEnableSingleMino)
    {
        var mino = new MinoData();

        var color = (ColorData)(Random.Range(0, 3) + 1);
        MinoData.MinoShape shape;
        if (isEnableSingleMino)
        {
            shape = (MinoData.MinoShape)Random.Range(0, 3);
        }
        else
        {
            shape = (MinoData.MinoShape)Random.Range(0, 2);
        }
        switch (shape)
        {
            case MinoData.MinoShape._4_1:
                // 4×1のやつ
                for (var i = 0; i < 4; i++)
                {
                    mino.Units[i, 1].SetCurrentData(color, false);
                }
                break;
            case MinoData.MinoShape._2_2:
                // 2×2のやつ
                for (var i = 0; i < 2; i++)
                {
                    for (var j = 0; j < 2; j++)
                    {
                        mino.Units[i + 1, j + 1].SetCurrentData(color, false);
                    }
                }
                break;
            case MinoData.MinoShape._1_1:
                mino.Units[1, 1].SetCurrentData(color, true);
                break;
        }

        return mino;
    }

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
                var actX = j + checkX;
                var actY = i + checkY;

                // はみ出していたらアウト
                var minoUnit = CurrentMino.Units[i, j];
                if (minoUnit.CurrentColor != ColorData.None && IsOutOfField(actX, actY))
                {
                    return false;
                }

                // フィールド上にあるブロックと同じ色成分があればアウト
                var fieldUnit = Units[actY, actX];
                if ((minoUnit.CurrentColor & fieldUnit.CurrentColor) != ColorData.None)
                {
                    return false;
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
        Debug.Log("Move Left");
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
        Debug.Log("Move Right");
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
        Debug.Log("Rotate");
        CurrentMino.Rotate();
    }

    /// <summary>
    /// ミノを下に動かす
    /// </summary>
    private void SoftDrop()
    {
        Debug.Log("Soft Drop");
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
        Debug.Log("Hard Drop");
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
            var actY = y + pos.y;
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

                // 直上に何も無ければスルー
                if (aboveColor == ColorData.None)
                {
                    continue;
                }

                // 直上のブロックと現在のブロックの成分が重なったら自動落下するブロックである
                if ((aboveColor & (unitColor | unit.InputColor)) == ColorData.None)
                {
                    return true;
                }
            }
        }
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
                Units[i, j].SetInputData(ColorData.None, false);
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
                    bottomUnit.SetInputData(unit.CurrentColor, unit.IsCurrentLight);
                    unit.SetCurrentData(ColorData.None, false);
                }

                // 対象のブロックの自動落下が有色で、かつ、下のブロックと同じ成分を持たない時
                else if (unit.InputColor != ColorData.None && (unit.InputColor & bottomUnit.CurrentColor) == ColorData.None)
                {
                    bottomUnit.SetInputData(unit.InputColor, unit.IsInputLight);
                    unit.SetInputData(ColorData.None, false);
                }
            }
        }
    }

    /// <summary>
    /// ブロックの色を混ぜ合わせる
    /// </summary>
    public void MixUnitColor()
    {
        for (var i = FIELD_HEIGHT - 1; i >= 0; i--)
        {
            for (var j = 0; j < FILED_WIDTH; j++)
            {
                var unit = Units[i, j];
                unit.SetCurrentData(unit.CurrentColor | unit.InputColor, unit.IsCurrentLight || unit.IsInputLight);
                unit.SetInputData(ColorData.None, false);
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
    public List<Vector2Int> DeleteWhiteUnit()
    {
        List<Vector2Int> deletePosList = new List<Vector2Int>();
        for (var i = FIELD_HEIGHT - 1; i >= 0; i--)
        {
            for (var j = 0; j < FILED_WIDTH; j++)
            {
                var unit = Units[i, j];
                if ((unit.CurrentColor & ColorData.White) == ColorData.White)
                {
                    deletePosList.Add(new Vector2Int(j, i));
                    unit.InitData();
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
}
