using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "ScriptableObject/MinoGenerateRoundData", fileName = "MinoGenerateRoundData")]
public class MinoGenerateRoundData : ScriptableObject
{
    public List<MinoGenerateData> NormalList;

    public List<MinoGenerateData> LaserList;
}
