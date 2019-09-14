using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LaserState
{
    None = 1 << 0,
    Prepare = 1 << 1,
    Fire = 1 << 2,
    Stop = 1 << 3,
}
