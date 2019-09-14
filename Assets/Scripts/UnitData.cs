using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitData
{
    public ColorData CurrentColor { get; private set; }
    public ColorData InputColor { get; private set; }

    public bool IsCurrentLight { get; private set; }
    public bool IsInputLight { get; private set; }

    public UnitData()
    {
        InitData();
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
        IsCurrentLight = isLight;
    }

    public void SetInputData(ColorData color, bool isLight)
    {
        InputColor = color;
        IsInputLight = isLight;
    }

    public void InitData()
    {
        CurrentColor = ColorData.None;
        InputColor = ColorData.None;
        IsCurrentLight = false;
        IsInputLight = false;
    }

    public ColorData GetDisplayColor()
    {
        return CurrentColor | InputColor;
    }
}
