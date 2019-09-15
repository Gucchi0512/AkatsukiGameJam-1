using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

/// <summary>
/// ミノ生成器
/// </summary>
public class MinoGenerator
{
    private MinoGenerateRoundData m_MinoGenerateRoundData;
    private Queue<MinoGenerateData> m_NormalGenerateQueue;
    private Queue<MinoGenerateData> m_LaserGenerateQueue;

    public MinoGenerator(MinoGenerateRoundData generateRoundData)
    {
        m_MinoGenerateRoundData = generateRoundData;
        LaserShuffle();
        NormalShuffle();
    }

    private void NormalShuffle()
    {
        var list = m_MinoGenerateRoundData.NormalList.ToList();
        list = list.OrderBy(i => Guid.NewGuid()).ToList();
        m_NormalGenerateQueue = new Queue<MinoGenerateData>(list);
    }

    private void LaserShuffle()
    {
        var list = m_MinoGenerateRoundData.LaserList.ToList();
        list = list.OrderBy(i => Guid.NewGuid()).ToList();
        m_LaserGenerateQueue = new Queue<MinoGenerateData>(list);
    }

    public MinoData GenerateMino()
    {
        MinoGenerateData generateData = null;
        if (m_NormalGenerateQueue.Count < 1)
        {
            NormalShuffle();
            if (m_LaserGenerateQueue.Count < 1)
            {
                generateData = m_NormalGenerateQueue.Dequeue();
            }
            else
            {
                generateData = m_LaserGenerateQueue.Dequeue();
            }
        }
        else
        {
            generateData = m_NormalGenerateQueue.Dequeue();
        }

        if (generateData == null)
        {
            return null;
        }

        var mino = new MinoData();
        mino.Shape = generateData.MinoShape;

        for (var i = 0; i < MinoData.MINO_HEIGHT; i++)
        {
            for (var j = 0; j < MinoData.MINO_WIDTH; j++)
            {
                var unitData = generateData.MinoUnits[i].LineUnits[j];
                mino.Units[i, j].SetCurrentData(unitData.ColorData, unitData.IsLaser ? LaserState.Prepare : LaserState.None);
            }
        }

        if (m_LaserGenerateQueue.Count < 1)
        {
            LaserShuffle();
        }

        return mino;
    }
}
