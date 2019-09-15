using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    [SerializeField]
    private Text m_TimeText;

    private int preTime;

    private void Start()
    {
        m_TimeText.text = GameManager.GAME_TIME.ToString();
        preTime = (int)GameManager.GAME_TIME;
    }

    private void Update()
    {
        var time = (int)GameManager.Instance.RemainTime;
        if (time != preTime)
        {
            m_TimeText.text = time.ToString();
            preTime = time;
        }
    }
}
