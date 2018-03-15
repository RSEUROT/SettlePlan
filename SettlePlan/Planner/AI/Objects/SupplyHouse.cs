using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyHouse : MonoBehaviour, ObjectStatic
{
    [SerializeField]
    private int numLogs;
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

    [SerializeField]
    private float remainingDistance;


    public float GetRemainingDistance()
    {
        return remainingDistance;
    }

}
