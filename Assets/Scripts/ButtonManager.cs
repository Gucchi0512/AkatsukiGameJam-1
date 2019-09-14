using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public GameObject startButton;
    public GameObject button1P;
    public GameObject button2P;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.At) || Input.GetKeyDown(KeyCode.Joystick1Button9))
        {
            startButton.SetActive(false);
            button1P.SetActive(true);
            button2P.SetActive(true);
        }
    }
}
