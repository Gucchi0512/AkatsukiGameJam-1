using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CountdownHandler : MonoBehaviour
{
    public UnityEngine.UI.Text count;
    public GameObject canvas;

    void Start()
    {
        GameManager.Instance.AddGameStartCountDownCallBack(countdown);
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}
    public void countdown(int currentCount)
    {
        if (currentCount > 0)
        {
            count.text = currentCount.ToString();
        }
        else if (currentCount == 0)
        {
            count.text = "Start!!";
        }
        else
        {
            canvas.SetActive(false);
        }
    }
}

