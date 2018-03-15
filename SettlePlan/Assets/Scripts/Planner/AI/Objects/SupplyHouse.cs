using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyHouse : MonoBehaviour, ObjectStatic
{
    public int GetNumLogs()
    {
        return EconomicBrainGoap.Instance.NumLogs;
    }
    public void UpdateNumLogs(int _num)
    {
        EconomicBrainGoap.Instance.NumLogs += _num;
    }

    public int GetNumOres()
    {
        return EconomicBrainGoap.Instance.NumOres;
    }
    public void UpdateNumOres(int _num)
    {
        EconomicBrainGoap.Instance.NumOres += _num;
    }

    public int GetNumBlee()
    {
        return EconomicBrainGoap.Instance.NumBlee;
    }
    public void UpdateNumBlee(int _num)
    {
        EconomicBrainGoap.Instance.NumBlee += _num;
    }

    public int GetNumBread()
    {
        return EconomicBrainGoap.Instance.NumBreads;
    }
    public void UpdateNumBread(int _num)
    {
        EconomicBrainGoap.Instance.NumBreads += _num;
    }


    [SerializeField]
    private float remainingDistance;

    public float GetRemainingDistance()
    {
        return remainingDistance;
    }

}
