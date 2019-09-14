using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameManagerState
{
    /// <summary>
    /// ゲーム開始時
    /// </summary>
    GameStart,

    /// <summary>
    /// プレイヤー入力可能時
    /// </summary>
    Input,

    /// <summary>
    /// ミノ着地時
    /// </summary>
    Put,

    /// <summary>
    /// 自動的に落下する時
    /// </summary>
    DropDown,

    /// <summary>
    /// 色替え時
    /// </summary>
    ChangeColor,

    /// <summary>
    /// 白ミノ消滅時
    /// </summary>
    Disapper,

    /// <summary>
    /// ゲームオーバーチェック時
    /// </summary>
    CheckGameOver,

    /// <summary>
    /// ゲームオーバー時
    /// </summary>
    GameEnd,
}
