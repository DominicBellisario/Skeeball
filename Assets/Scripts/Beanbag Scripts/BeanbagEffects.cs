using UnityEditor;
using UnityEngine;

public class BeanbagEffects : ObjectEffects
{
    //the number of physics frames caculated each frame for the aiming line
    [SerializeField] int steps;
    //a ghost beanbag that appears at the end of an aimline
    [SerializeField] GameObject[] ghosts;

    protected override void Update()
    {
        //activate the aiming line when the ball is held
        if (GetComponent<ObjectControls>().IsHeld)
        {
            float angle = GetComponent<ObjectControls>().Angle;
            float powerPercent = GetComponent<ObjectControls>().PowerPercent;

            //update middle aim line
            DrawAimLine(aimLine, angle, ghosts[1]);

            //update left and right if tri is enabled
            if (triBallEnabled)
            {
                DrawAimLine(leftAimLine, angle - triBallAngleRads, ghosts[0]);
                DrawAimLine(rightAimLine, angle + triBallAngleRads, ghosts[2]);
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
            dottedLineMaterial.color = new Color(1, 1 - powerPercent, 1 - powerPercent, dottedLineAlpha);
        }
        else
        {
            aimLine.enabled = false;
            leftAimLine.enabled = false;
            rightAimLine.enabled = false;
            ghosts[0].SetActive(false);
            ghosts[1].SetActive(false);
            ghosts[2].SetActive(false);
        }
    }

    private void DrawAimLine(LineRenderer line, float angle, GameObject ghost)
    {
        line.enabled = true;
        Vector3[] positions = UpdateAimLine(angle);
        line.positionCount = positions.Length;
        line.SetPositions(positions);
        //make a linecast for the aimline
        for (int i = 0; i < positions.Length - 1; i++)
        {
            if (Physics.Linecast(positions[i] + transform.position, positions[i + 1] + transform.position, out RaycastHit hit))
            {
                Debug.Log(line.name + " Hit: " + hit.collider.name);
                ghost.SetActive(true);
                ghost.transform.position = hit.point;
                //stop casting when it hits
                return;
            }
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
