using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreHandler : MonoBehaviour
{
    public UnityEngine.UI.Text scoreLabel;
    private int resultScore;
    private int score;
    const string RESULT_SCORE_KEY = "ResultScore";

    // Start is called before the first frame update
    void Start()
    {
        //リザルトスコアをロード
        score = 10230;
        PlayerPrefs.SetInt(RESULT_SCORE_KEY,score);
        resultScore = PlayerPrefs.GetInt(RESULT_SCORE_KEY, -1);
        scoreLabel.text = resultScore.ToString();
    }

}
