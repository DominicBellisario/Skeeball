using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HoleVariables : MonoBehaviour
{
    //how many points the hole is worth
    [SerializeField]
    int points;

    //wether or not the ball is destroyed when passing through
    [SerializeField]
    bool destroyBall;

    public int Points { get { return points; } set { points = value; } }
    public bool DestroyBall { get { return destroyBall; } }
}
