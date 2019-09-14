using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ブロックの色情報
/// </summary>
public enum ColorData
{
    None = 0,
    Blue = 1,
    Green = 2,
    Red = 4,
    Cyan = Blue | Green,
    Magenta = Red | Blue,
    Yellow = Red | Green,
    White = Red | Green | Blue,
}
