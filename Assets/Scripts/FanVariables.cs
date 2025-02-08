using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanVariables : MonoBehaviour
{
    [SerializeField] ParticleSystem ps;
    [SerializeField] int forceMultiplier;

    public int ForceMultiplier { get { return forceMultiplier; } }

    private void Start()
    {
        //set the particle lifetime.  they disapear once they reach the end of the fan's effect
        var main = ps.main;
        main.startLifetime = (GetComponent<BoxCollider>().size.y - 1) / ps.main.startSpeed.constant;
        ps.Play();
    }
}
