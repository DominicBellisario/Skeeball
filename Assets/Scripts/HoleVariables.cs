using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HoleVariables : MonoBehaviour
{
    //hole text prefab
    [SerializeField] GameObject holeText;
    //how many points the hole is worth
    [SerializeField] int points;
    int startingPoints;
    //wether or not the ball is destroyed when passing through
    [SerializeField] bool destroyBall;
    [SerializeField] MeshRenderer holeRimMesh;

    //all possible hole materials.  Each color has their own subset of materials
    [SerializeField] Material[] greenMaterials;
    [SerializeField] Material[] orangeMaterials;
    [SerializeField] Material[] blueMaterials;
    [SerializeField] Material[] redMaterials;
    [SerializeField] Material[] goldMaterials;

    public int Points { get { return points; } }
    public bool DestroyBall { get { return destroyBall; } }

    private void Start()
    {
        startingPoints = points;
    }

    /// <summary>
    /// double the points and make the hole rim glow
    /// </summary>
    /// <param name="points"></param>
    public void MarkHole()
    {
        points *= 2;
        if (startingPoints == 10) { ChangeMaterial(greenMaterials, 1); }
        else if (startingPoints == 20) { ChangeMaterial(orangeMaterials, 1); }
        else if (startingPoints == 30) { ChangeMaterial(blueMaterials, 1); }
        else if (startingPoints == 50) { ChangeMaterial(redMaterials, 1); }
        else if (startingPoints == 100) { ChangeMaterial(goldMaterials, 1); }
    }

    private void ChangeMaterial(Material[] color, int materialIndex)
    {
        holeRimMesh.material = color[materialIndex];
    }

    public void SpawnHoleText(bool isGold)
    {
        GameObject newHoleText = Instantiate(holeText);
        newHoleText.transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        newHoleText.GetComponent<HoleText>().SetText(points, isGold, !(startingPoints == points));
        
    }
}
