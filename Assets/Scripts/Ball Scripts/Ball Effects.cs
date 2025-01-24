using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
    [SerializeField]
    GameObject particleTrail;

    //the balls that appear when triball is active
    [SerializeField]
    GameObject leftTriBall;
    [SerializeField]
    GameObject rightTriBall;

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

    //a list of all materials cirrently applied to the ball
    Material[] materials;

    //the base skin for the ball
    [SerializeField]
    Material defaultMaterial;
    //when a powerup is disabled, this material replaces the powerup material
    [SerializeField]
    Material transMaterial;

    //powerup toggles and materials
    bool goldBallEnabled;
    [SerializeField]
    Material goldBallMaterial;
    bool markedBallEnabled;
    [SerializeField]
    Material markedBallMaterial;
    bool triBallEnabled;

    public bool GoldBallEnabled {  get { return goldBallEnabled; } set { goldBallEnabled = value; } }
    public bool MarkedBallEnabled { get { return markedBallEnabled; } set { markedBallEnabled = value; } }
    public bool TriBallEnabled { get { return triBallEnabled; } set { triBallEnabled = value; } }


    private void Awake()
    {
        //linerenderer is in a line, which is a child of the ball
        aimLine.enabled = false;

        offsetDifference = maxOffsetSpeed - minOffsetSpeed;
        totalOffset = 0;

        materials = GetComponent<MeshRenderer>().materials;
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

        //turn on new particle trail and destroy old one once ball is launched
        if (GetComponent<BallControls>().IsLaunched)
        {
            particleTrail.SetActive(true);

            GameObject oldParticles = GameObject.Find("Ball Path Particles");
            if (oldParticles != particleTrail && oldParticles != null)
            {
                Destroy(oldParticles);
            }
        }
    }

    /// <summary>
    /// makes the particle system on ball a top-level object so it does not get destroyed along with ball
    /// </summary>
    public void SeparateParticleSystem()
    {
        particleTrail.transform.SetParent(null);
    }

    //toggles for powerups
    public void ToggleGoldBall()
    {
        goldBallEnabled = !goldBallEnabled;
        if (goldBallEnabled) { materials[1] = goldBallMaterial; }
        else { materials[1] = transMaterial; }
        UpdateMaterials();
    }
    public void ToggleMarkedBall()
    {
        markedBallEnabled = !markedBallEnabled;
        if (markedBallEnabled) { materials[2] = markedBallMaterial; }
        else { materials[2] = transMaterial; }
        UpdateMaterials();
    }
    public void ToggleTriBall()
    {
        triBallEnabled = !triBallEnabled;
        leftTriBall.SetActive(triBallEnabled);
        rightTriBall.SetActive(triBallEnabled);
    }

    public void DisableTriBalls()
    {
        leftTriBall.SetActive(false);
        rightTriBall.SetActive(false);
    }

    /// <summary>
    /// updates the ball's materials with the current material array
    /// </summary>
    private void UpdateMaterials()
    {
        GetComponent<MeshRenderer>().materials = materials;
        leftTriBall.GetComponent<MeshRenderer>().materials = materials;
        rightTriBall.GetComponent <MeshRenderer>().materials = materials;
    }

    //reset the material when the application closes
    private void OnApplicationQuit()
    {
        dottedLineMaterial.mainTextureOffset = Vector2.zero;
        dottedLineMaterial.color = Color.white;
    }
}
