using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicManager
{
    /// <summary>
    /// 一人プレイ用か
    /// </summary>
    private bool m_IsSinglePlay;

    public UnitFieldData FieldDataPlayer1 { get; private set; }
    public UnitFieldData FieldDataPlayer2 { get; private set; }

    public void OnStart(bool isSinglePlay)
    {
        m_IsSinglePlay = isSinglePlay;
        FieldDataPlayer1 = new UnitFieldData(true);
        FieldDataPlayer2 = new UnitFieldData(false);
    }

    public void OnUpdate()
    {

    }
}
