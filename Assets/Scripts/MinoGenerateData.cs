using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(menuName = "ScriptableObject/MinoGenerateData", fileName = "MinoGenerateData")]
public class MinoGenerateData : ScriptableObject
{
    [Serializable]
    public struct MinoGenerateUnitData
    {
        public ColorData ColorData;
        public bool IsLaser;
    }

    [Serializable]
    public struct MinoGenerateLineData
    {
        public MinoGenerateUnitData[] LineUnits;
    }

    public MinoData.MinoShape MinoShape;
    public MinoGenerateLineData[] MinoUnits;
}
