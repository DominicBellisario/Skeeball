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
    [SerializeField]
    GameObject holeRim;

    //all possible hole materials.  Each color has their own subset of materials
    Material[][] materials;
    [SerializeField]
    Material[] greenMaterials;
    [SerializeField]
    Material[] orangeMaterials;
    [SerializeField]
    Material[] blueMaterials;
    [SerializeField]
    Material[] redMaterials;
    [SerializeField]
    Material[] goldMaterials;

    public Material[][] Materials { get { return materials; } }

    private void Start()
    {
        materials = new Material[][] { greenMaterials, orangeMaterials, blueMaterials, redMaterials, goldMaterials };
    }

    public int Points { get { return points; } set { points = value; } }
    public bool DestroyBall { get { return destroyBall; } }
}
