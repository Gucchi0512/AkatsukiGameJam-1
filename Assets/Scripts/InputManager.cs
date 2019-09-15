using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InputManager
{
    public InputManagerState inputPlayer1 { get; private set; }
    public InputManagerState inputPlayer2 { get; private set; }

    // Start is called before the first frame update
    public void OnStart()
    {
    }

    // Update is called once per frame
    public void OnUpdate()
    {
        GameManagerState state = GameManager.Instance.CurrentState;

        if (state == GameManagerState.Game)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Joystick2Button2))
            {
                //Debug.Log("hard drop");
                inputPlayer1 = InputManagerState.HardDrop;
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Joystick2Button0))
            {
                //Debug.Log("left");
                inputPlayer1 = InputManagerState.Left;
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.Joystick2Button1))
            {
                //Debug.Log("soft drop");
                inputPlayer1 = InputManagerState.SoftDrop;
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.Joystick2Button3))
            {
                //Debug.Log("right");
                inputPlayer1 = InputManagerState.Right;

            }
            else if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Joystick2Button15))
            {
                //Debug.Log("rotate");
                inputPlayer1 = InputManagerState.Rotate;
            }
            else
            {
                //Debug.Log("None");
                inputPlayer1 = InputManagerState.None;
            }

            if (Input.GetKeyDown(KeyCode.At)|| Input.GetKeyDown(KeyCode.Joystick1Button1))
            {
                //Debug.Log("hard drop");
                inputPlayer2 = InputManagerState.HardDrop;
            }
            else if (Input.GetKeyDown(KeyCode.Semicolon) || Input.GetKeyDown(KeyCode.Joystick1Button3))
            {
                //Debug.Log("left");
                inputPlayer2 = InputManagerState.Left;
            }
            else if (Input.GetKeyDown(KeyCode.Colon) || Input.GetKeyDown(KeyCode.Joystick1Button2))
            {
                //Debug.Log("soft drop");
                inputPlayer2 = InputManagerState.SoftDrop;
            }
            else if (Input.GetKeyDown(KeyCode.RightBracket) || Input.GetKeyDown(KeyCode.Joystick1Button0))
            {
                //Debug.Log("right");
                inputPlayer2 = InputManagerState.Right;

            }
            else if (Input.GetKeyDown(KeyCode.LeftBracket) || Input.GetKeyDown(KeyCode.Joystick1Button15))
            {
                //Debug.Log("rotate");
                inputPlayer2 = InputManagerState.Rotate;
            }
            else
            {
                //Debug.Log("None");
                inputPlayer2 = InputManagerState.None;
            }
        }
    }
}
