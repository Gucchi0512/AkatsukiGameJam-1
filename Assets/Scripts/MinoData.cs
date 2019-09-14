using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 4×4のミノデータ
/// </summary>
public class MinoData
{
    public enum MinoShape
    {
        _4_1 = 0,
        _2_2 = 1,
        _1_1 = 2,
    }

    public const int MINO_WIDTH = 4;
    public const int MINO_HEIGHT = 4;

    /// <summary>
    /// 4×4の左上部分の座標
    /// </summary>
    public Vector2Int Pos { get; set; }

    public UnitData[,] Units { get; private set; }
    public MinoShape Shape { get; set; }

    public MinoData()
    {
        Pos = new Vector2Int(-1, -1);
        Units = new UnitData[MINO_HEIGHT, MINO_WIDTH];
        for (var i = 0; i < MINO_HEIGHT; i++)
        {
            for (var j = 0; j < MINO_WIDTH; j++)
            {
                Units[i, j] = new UnitData();
            }
        }
    }

    /// <summary>
    /// ミノを回転させる
    /// </summary>
    public void Rotate()
    {
        switch (Shape)
        {
            case MinoShape._4_1:
            case MinoShape._2_2:
                var rotateUnits = new UnitData[MINO_WIDTH, MINO_HEIGHT];
                for (var i = 0; i < MINO_WIDTH; i++)
                {
                    for (var j = 0; j < MINO_HEIGHT; j++)
                    {
                        rotateUnits[i, j] = Units[MINO_HEIGHT - 1 - j, i];
                    }
                }
                Units = rotateUnits;
                break;
            case MinoShape._1_1:
                // 何もしない
                break;
        }
    }
}
