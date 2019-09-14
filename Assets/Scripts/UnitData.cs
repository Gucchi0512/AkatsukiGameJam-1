using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitData
{
    public ColorData CurrentColor { get; private set; }
    public ColorData InputColor { get; private set; }

    public bool IsLigth { get; private set; }

    public UnitData()
    {
        CurrentColor = ColorData.None;
        InputColor = ColorData.None;
        IsLigth = false;
    }

    /// <summary>
    /// 表示するミノとして空かどうか。
    /// </summary>
    public bool IsEmptyDisplayData()
    {
        return CurrentColor == ColorData.None;
    }

    public void SetCurrentData(ColorData color, bool isLight)
    {
        CurrentColor = color;
        IsLigth = isLigth;
    }
}
