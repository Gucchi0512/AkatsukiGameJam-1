using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitFieldState
{
    None,

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
    AutoDrop,

    /// <summary>
    /// 色替え時
    /// </summary>
    ChangeColor,

    /// <summary>
    /// レーザー開始時 レーザー演出を入れる可能性があるのでステートとして切り出した
    /// </summary>
    StartLaser,

    /// <summary>
    /// レーザー終了時 レーザー演出を入れる可能背があるのでステートとして切り出した
    /// </summary>
    EndLaser,

    /// <summary>
    /// 白ミノ消滅時
    /// </summary>
    Delete,

    /// <summary>
    /// ゲームオーバーチェック時
    /// </summary>
    CheckGameOver,

    /// <summary>
    /// ゲームオーバー時
    /// </summary>
    GameOver,

    /// <summary>
    /// 時間切れ時
    /// </summary>
    TimeUp,
}
