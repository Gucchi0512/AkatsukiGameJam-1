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

        if (m_IsSinglePlay)
        {
            FieldDataPlayer1.OnStart();
        } else
        {
            FieldDataPlayer1.OnStart();
            FieldDataPlayer2.OnStart();
        }
    }

    public void OnStateState()
    {
        FieldDataPlayer1.OnStartState();
    }

    public void OnUpdate()
    {
        if (m_IsSinglePlay)
        {
            FieldDataPlayer1.OnUpdate();
        } else
        {
            FieldDataPlayer1.OnUpdate();
            FieldDataPlayer2.OnUpdate();
        }
    }

    public void OnEndState()
    {
        FieldDataPlayer1.OnEndState();
    }
}
