using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    [SerializeField]
    private Text m_WinnerText;

    [SerializeField]
    private Text m_ScoreText;

    private void Start()
    {
        var isSingele = PlayerPrefs.GetInt(ConstKeys.IS_SINGLE_PLAY_MODE_KEY, 0) == 0;
        if (isSingele)
        {
            m_WinnerText.gameObject.SetActive(false);
            m_ScoreText.gameObject.SetActive(true);

            var score = PlayerPrefs.GetInt(ConstKeys.SCORE, 0);
            m_ScoreText.text = score.ToString();
        }
        else
        {
            m_ScoreText.gameObject.SetActive(false);
            m_WinnerText.gameObject.SetActive(true);
            var winner = PlayerPrefs.GetInt(ConstKeys.WINNER, 0);
            if (winner == 1)
            {
                m_WinnerText.text = "Player1 Win!";
            } else if (winner == 2)
            {
                m_WinnerText.text = "Player2 Win!";
            } else
            {
                m_WinnerText.text = "Draw!";
                m_WinnerText.color = Color.black;
            }
        }
    }
}
