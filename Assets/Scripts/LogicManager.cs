using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicManager
{
    public UnitFieldData FieldDataPlayer1 { get; private set; }
    public UnitFieldData FieldDataPlayer2 { get; private set; }

    public void OnStart()
    {
        FieldDataPlayer1 = new UnitFieldData(true);
        FieldDataPlayer2 = new UnitFieldData(false);

        if (GameManager.Instance.IsSinglePlay)
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
        if (GameManager.Instance.IsSinglePlay)
        {
            FieldDataPlayer1.OnStartState();
        }
        else
        {
            FieldDataPlayer1.OnStartState();
            FieldDataPlayer2.OnStartState();
        }
    }

    public void OnUpdate()
    {
        if (GameManager.Instance.IsSinglePlay)
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
        if (GameManager.Instance.IsSinglePlay)
        {
            FieldDataPlayer1.OnEndState();
        }
        else
        {
            FieldDataPlayer1.OnEndState();
            FieldDataPlayer2.OnEndState();
        }
    }
}
