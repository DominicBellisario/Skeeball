using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class BallEffects : MonoBehaviour
{
    //shows the trajectory of the ball while aiming
    [SerializeField]
    LineRenderer aimLine;
    //the aim line material
    [SerializeField]
    Material dottedLineMaterial;

    //the min and max texture speed
    [SerializeField]
    float minOffsetSpeed;
    [SerializeField]
    float maxOffsetSpeed;
    float offsetDifference;
    float totalOffset;


    //the length constraignts for the aiming line
    [SerializeField]
    float minLineLength;
    [SerializeField]
    float maxLineLength;

    // Start is called before the first frame update
    void Start()
    {
        //linerenderer is in a line, which is a child of the ball
        aimLine.enabled = false;

        offsetDifference = maxOffsetSpeed - minOffsetSpeed;
        totalOffset = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //activate the aiming line when the ball is held
        if (GetComponent<BallControls>().IsHeld)
        {
            float angle = GetComponent<BallControls>().Angle;
            float powerPercent = GetComponent<BallControls>().PowerPercent;
            aimLine.enabled = true;

            float lineLength = (maxLineLength - minLineLength) * powerPercent;

            aimLine.SetPosition(1, new Vector3(-Mathf.Sin(angle) * lineLength, 0, -Mathf.Cos(angle) * lineLength));

            //dotted line effects
            //offset the texture.  Speed of offset is determined by strength of launch
            totalOffset -= ((offsetDifference * powerPercent) + minOffsetSpeed) * Time.deltaTime;
            dottedLineMaterial.mainTextureOffset = new Vector2(totalOffset, 0);

            //change the color from white to red depending on the power percent
            dottedLineMaterial.color = new Color(1, 1 - powerPercent, 1 - powerPercent);
        }
        else
        {
            aimLine.enabled = false;
        }
    }

    //reset the material when the application closes
    private void OnApplicationQuit()
    {
        dottedLineMaterial.mainTextureOffset = Vector2.zero;
        dottedLineMaterial.color = Color.white;
    }
}
