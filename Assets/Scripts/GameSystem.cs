using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSystem : MonoBehaviour
{

    public void Start1PGame()
    {
        SceneManager.LoadScene("MainOP");
    }

    public void Start2PGame()
    {
        PlayerPrefs.SetInt(ConstKeys.IS_SINGLE_PLAY_MODE_KEY, 1);
        SceneManager.LoadScene("Main2P");
    }

    public void ReturnTitle()
    {
        PlayerPrefs.SetInt(ConstKeys.IS_SINGLE_PLAY_MODE_KEY, 0);
        SceneManager.LoadScene("Title");
    }
}
