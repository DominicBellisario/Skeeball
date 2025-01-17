using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class BallEffects : MonoBehaviour
{
    //shows the trajectory of the ball while aiming
    LineRenderer aimLine;

    //the length constraignts for the aiming line
    [SerializeField]
    float minLineLength;
    [SerializeField]
    float maxLineLength;

    // Start is called before the first frame update
    void Start()
    {
        //linerenderer is in a line, which is a child of the ball
        aimLine = GetComponentInChildren<LineRenderer>();
        aimLine.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //activate the aiming line when the ball is held
        if (GetComponent<BallControls>().IsHeld)
        {
            float angle = GetComponent<BallControls>().Angle;
            aimLine.enabled = true;

            float lineLength = (maxLineLength - minLineLength) * GetComponent<BallControls>().PowerPercent;

            aimLine.SetPosition(1, new Vector3(-Mathf.Sin(angle) * lineLength, 0, -Mathf.Cos(angle) * lineLength));
            //Debug.Log(aimLine.GetPosition(1));
        }
        else
        {
            aimLine.enabled = false;
        }
    }
}
