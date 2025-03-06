using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarHoleVariables : MonoBehaviour
{
    //hole text prefab
    [SerializeField] GameObject holeText;

    //wether or not the ball is destroyed when passing through
    [SerializeField] bool destroyBall;
    [SerializeField] MeshRenderer holeRimMesh;

    public bool DestroyBall { get { return destroyBall; } }

    public void SpawnHoleText(int points, Vector3 ballPos)
    {
        int shownPoints = points;
        GameObject newHoleText = Instantiate(holeText);
        newHoleText.transform.position = ballPos += transform.up;
        newHoleText.GetComponent<HoleText>().SetText(Mathf.RoundToInt(shownPoints * Manager.Instance.Multiplier).ToString());
    }
}
