using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleInventory : MonoBehaviour
{
    [SerializeField]
    private int numLogs;
    [SerializeField]
    private int numOres;
    [SerializeField]
    private int numFoods;
    [SerializeField]
    private int numBlee;
    [SerializeField]
    private int numBreads;

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
    public int NumBlee
    {
        get
        {
            return numBlee;
        }

        set
        {
            numBlee = value;
        }
    }
    public int NumBreads
    {
        get
        {
            return numBreads;
        }

        set
        {
            numBreads = value;
        }
    }
}
