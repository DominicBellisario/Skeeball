using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class BallEffects : ObjectEffects
{
    //the length constraignts for the aiming line
    [SerializeField]
    float minLineLength;
    [SerializeField]
    float maxLineLength;
    float lineLength;

    protected override void Update()
    {
        base.Update();
        //activate the aiming line when the ball is held
        if (GetComponent<ObjectControls>().IsHeld)
        {
            float angle = GetComponent<ObjectControls>().Angle;
            float powerPercent = GetComponent<ObjectControls>().PowerPercent;
            lineLength = (maxLineLength - minLineLength) * powerPercent;

            //update middle aim line
            aimLine.enabled = true;
            aimLine.SetPosition(1, AimLinePosition(angle));

            //if triball, activate the other aim lines
            if (triBallEnabled)
            {
                leftAimLine.enabled = true;
                leftAimLine.SetPosition(1, AimLinePosition(angle - triBallAngleRads));
                rightAimLine.enabled = true;
                rightAimLine.SetPosition(1, AimLinePosition(angle + triBallAngleRads));
            }
            else
            {
                leftAimLine.enabled = false;
                rightAimLine.enabled = false;
            }

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
            leftAimLine.enabled = false;
            rightAimLine.enabled = false;
        }
    }

    private Vector3 AimLinePosition(float angle)
    {
        return new Vector3(-Mathf.Sin(angle) * lineLength, 0, -Mathf.Cos(angle) * lineLength);
    }
}
