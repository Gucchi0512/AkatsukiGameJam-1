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
        SceneManager.LoadScene("2PMainOP");
    }

    public void ReturnTitle()
    {
        SceneManager.LoadScene("Title");
    }
}
