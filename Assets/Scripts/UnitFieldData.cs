using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// フィールドとプレイヤーが動かすミノのデータなどを管理する。
/// </summary>
public class UnitFieldData
{
    private const int FILED_WIDTH = 8;
    private const int FIELD_HEIGHT = 16;

    public UnitData[,] Units { get; private set; }
    public MinoData CurrentMino { get; private set; }
    public MinoData NextMino { get; private set; }
    public MinoData NextNextMino { get; private set; }

    public UnitFieldData()
    {
        Units = new UnitData[FIELD_HEIGHT, FILED_WIDTH];
        for (var i=0;i<FIELD_HEIGHT;i++)
        {
            for (var j=0;j<FILED_WIDTH;j++)
            {
                Units[i, j] = new UnitData();
            }
        }

        CurrentMino = new MinoData();
        NextMino = new MinoData();
        NextNextMino = new MinoData();
    }
}
