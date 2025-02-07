using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanVariables : MonoBehaviour
{
    [SerializeField] int forceMultiplyer;

    public int ForceMultiplyer { get { return forceMultiplyer; } }
}
