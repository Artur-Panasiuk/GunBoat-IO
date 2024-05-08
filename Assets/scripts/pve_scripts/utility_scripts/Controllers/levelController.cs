using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelController : MonoBehaviour
{
    public bool upgradeStatistic(ref int stat, ref int avPoints)
    {
        bool funcOut = false;
        if(avPoints > 0)
        {
            stat++;
            avPoints--;
            funcOut = true;
        }

        return funcOut;
    }

    public float calculateScalableValue(int level, float basePoints, float multiplier)
    {
        float funcOut = basePoints * Mathf.Pow(multiplier, level);

        return funcOut;
    }
}
