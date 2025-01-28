using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeanbagEffects : ObjectEffects
{
    //the length constraignts for the aiming line
    [SerializeField]
    float minLineLength;
    [SerializeField]
    float maxLineLength;

    protected override void UpdateAimLine(float angle, float powerPercent)
    {
        base.UpdateAimLine(angle, powerPercent);

        float lineLength = (maxLineLength - minLineLength) * powerPercent;
        //adjust the aimline
        aimLine.SetPosition(1, new Vector3(-Mathf.Sin(angle) * lineLength, 0, -Mathf.Cos(angle) * lineLength));
        //if triball is on, adjust the other aimlines
        if (triBallEnabled)
        {
            leftAimLine.enabled = true;
            rightAimLine.enabled = true;

            leftAimLine.SetPosition(1, new Vector3(-Mathf.Sin(angle - triBallAngleRads) * lineLength, 0, -Mathf.Cos(angle - triBallAngleRads) * lineLength));
            rightAimLine.SetPosition(1, new Vector3(-Mathf.Sin(angle + triBallAngleRads) * lineLength, 0, -Mathf.Cos(angle + triBallAngleRads) * lineLength));
        }
        else
        {
            leftAimLine.enabled = false;
            rightAimLine.enabled = false;
        }
    }
}
