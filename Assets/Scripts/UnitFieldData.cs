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


    public UnitData[,] Units { get; private set; }
    public MinoData CurrentMino { get; private set; }
    public MinoData NextMino { get; private set; }
    public MinoData NextNextMino { get; private set; }

    /// <summary>
    /// これはプレイヤー1のデータである
    /// </summary>
    private bool m_IsPlayer1;

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
    }

    public void OnUpdate()
    {
        var gameState = GameManager.Instance.CurrentState;

        // 入力ステート受付
        switch (gameState)
        {
            case GameManagerState.Input:
                break;
            default:
                break;
        }

        // 操作処理
        switch(gameState)
        {
            case GameManagerState.Input:
                break;
            default:
                break;
        }

        // 
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

        for(var i=0;i<MinoData.MINO_HEIGHT;i++)
        {
            for (var j=0;j<MinoData.MINO_WIDTH;j++)
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
}
