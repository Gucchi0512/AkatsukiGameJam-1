using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI; // UIコンポーネントの使用

public class menu : MonoBehaviour
{
    Button start;
    Button play1P;
    Button play2P;

    void Start()
    {
        // ボタンコンポーネントの取得
        start = GameObject.Find("/TitleWindow/Buttons/StartButton").GetComponent<Button>();
        play1P = GameObject.Find("/TitleWindow/Buttons/1PButton").GetComponent<Button>();
        play2P = GameObject.Find("/TitleWindow/Buttons/2PButton").GetComponent<Button>();

        //// 最初に選択状態にしたいボタンの設定
        //start.Select();
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.At) || Input.GetKeyDown(KeyCode.Joystick1Button9))
        //{ 
        //play1P.Select();
        //}
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Joystick2Button15))
        {
            ExecuteEvents.Execute
            (
                target: start.gameObject,
                eventData: new PointerEventData(EventSystem.current),
                functor: ExecuteEvents.pointerClickHandler
            );
        }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Joystick2Button15))
        {
            ExecuteEvents.Execute
            (
                target: play1P.gameObject,
                eventData: new PointerEventData(EventSystem.current),
                functor: ExecuteEvents.pointerClickHandler
            );
        }
        if (Input.GetKeyDown(KeyCode.At) || Input.GetKeyDown(KeyCode.Joystick1Button15))
        {
            ExecuteEvents.Execute
            (
                target: play2P.gameObject,
                eventData: new PointerEventData(EventSystem.current),
                functor: ExecuteEvents.pointerClickHandler
            );
        }
    }
}