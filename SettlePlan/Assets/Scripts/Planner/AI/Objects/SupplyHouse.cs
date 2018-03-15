using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyHouse : MonoBehaviour, ObjectStatic
{
    [SerializeField]
    private int numLogs;
    [SerializeField]
    private int numOres;
    [SerializeField]
    private int numFoods;

    public int NumLogs
    {
        get
        {
            return numLogs;
        }

        set
        {
            numLogs = value;
        }
    }
    public int NumFoods
    {
        get
        {
            return numFoods;
        }

        set
        {
            numFoods = value;
        }
    }
    public int NumOres
    {
        get
        {
            return numOres;
        }

        set
        {
            numOres = value;
        }
    }

    [SerializeField]
    private float remainingDistance;

    public float GetRemainingDistance()
    {
        return remainingDistance;
    }

}
