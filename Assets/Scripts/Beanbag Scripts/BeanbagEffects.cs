using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class BeanbagEffects : ObjectEffects
{
    //the number of physics frames caculated each frame for the aiming line
    [SerializeField]
    int steps;

    protected override void Update()
    {
        base.Update();
        //activate the aiming line when the ball is held
        if (GetComponent<ObjectControls>().IsHeld)
        {
            float angle = GetComponent<ObjectControls>().Angle;
            float powerPercent = GetComponent<ObjectControls>().PowerPercent;

            //update middle aim line
            aimLine.enabled = true;
            Vector3[] positions = UpdateAimLine(angle);
            aimLine.positionCount = positions.Length;
            aimLine.SetPositions(positions);

            if (triBallEnabled)
            {
                leftAimLine.enabled = true;
                positions = UpdateAimLine(angle - triBallAngleRads);
                leftAimLine.positionCount = positions.Length;
                leftAimLine.SetPositions(positions);

                rightAimLine.enabled = true;
                positions = UpdateAimLine(angle + triBallAngleRads);
                rightAimLine.positionCount = positions.Length;
                rightAimLine.SetPositions(positions);
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

    private Vector3[] UpdateAimLine(float angle)
    {
        Vector3 position = Vector3.zero;

        //an array of points that make up the aiming line
        Vector3[] results = new Vector3[steps];

        //caluclate physics variables
        float timeStep = Time.fixedDeltaTime / Physics.defaultSolverVelocityIterations;
        Vector3 gravityAccel = timeStep * Physics.gravity;
        Vector3 moveStep = GetComponent<BeanbagControls>().PredictVelocity(angle) * timeStep;

        //fill in the array with each point
        results[0] = position;
        for (int i = 1; i < steps - 1; i++)
        {
            moveStep += gravityAccel;
            position += moveStep;
            results[i] = position;
        }

        return results;
    }
}
