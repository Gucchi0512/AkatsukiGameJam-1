using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitData
{
    /// <summary>
    /// 手前側にある色
    /// </summary>
    public ColorData CurrentColor { get; private set; }

    /// <summary>
    /// すり抜けているブロックの色
    /// </summary>
    public ColorData InputColor { get; private set; }

    /// <summary>
    /// レーザーで照射されている色
    /// </summary>
    public ColorData LaserColor { get; private set; }

    public LaserState CurrentLaserState { get; set; }

    public LaserState InputLaserState { get; set; }

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

    public void SetCurrentData(ColorData color, LaserState laserState)
    {
        CurrentColor = color;
        CurrentLaserState = laserState;
    }

    public void SetInputData(ColorData color, LaserState laserState)
    {
        InputColor = color;
        InputLaserState = laserState;
    }

    public void InitData()
    {
        CurrentColor = ColorData.None;
        InputColor = ColorData.None;
        CurrentLaserState = LaserState.None;
        InputLaserState = LaserState.None;
    }

    public ColorData GetDisplayColor()
    {
        if (CurrentLaserState != LaserState.None)
        {
            return CurrentColor;
        }
        else if (InputLaserState != LaserState.None)
        {
            return InputColor;
        }
        return CurrentColor | InputColor;
    }

    public ColorData GetOverrappedColor()
    {
        return CurrentColor | InputColor;
    }

    public bool IsExistLaser()
    {
        return (CurrentLaserState | InputLaserState) != LaserState.None;
    }

    public void SetLaserColor(ColorData color)
    {
        LaserColor = color;
    }
}
