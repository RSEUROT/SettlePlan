using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldComponent : MonoBehaviour, ObjectStatic
{
    [SerializeField]
    private float remainingDistance;


    public float GetRemainingDistance()
    {
        return remainingDistance;
    }
}
