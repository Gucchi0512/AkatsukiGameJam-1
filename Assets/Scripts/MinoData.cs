using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 4×4のミノデータ
/// </summary>
public class MinoData
{
    public const int MINO_WIDTH = 4;
    public const int MINO_HEIGHT = 4;

    public UnitData[,] Units { get; private set; }

    public MinoData()
    {
        Units = new UnitData[MINO_HEIGHT, MINO_WIDTH];
        for (var i = 0; i < MINO_HEIGHT; i++)
        {
            for (var j = 0; j < MINO_WIDTH; j++)
            {
                Units[i, j] = new UnitData();
            }
        }
    }
}
