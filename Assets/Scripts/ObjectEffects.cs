using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectEffects : MonoBehaviour
{
    //shows the trajectory of the object while aiming
    [SerializeField]
    protected LineRenderer aimLine;
    [SerializeField]
    protected LineRenderer leftAimLine;
    [SerializeField]
    protected LineRenderer rightAimLine;
    //the aim line material
    [SerializeField]
    protected Material dottedLineMaterial;

    //the particle trail
    [SerializeField]
    protected GameObject particleTrail;

    //the objects that appear when triball is active
    [SerializeField]
    protected GameObject leftTriBall;
    [SerializeField]
    protected GameObject rightTriBall;

    protected float triBallAngleRads;

    //the min and max texture speed
    [SerializeField]
    protected float minOffsetSpeed;
    [SerializeField]
    protected float maxOffsetSpeed;
    protected float offsetDifference;
    protected float totalOffset;

    //a list of all materials cirrently applied to the object
    protected Material[] materials;

    //the base skin for the ball
    [SerializeField]
    protected Material defaultMaterial;
    //when a powerup is disabled, this material replaces the powerup material
    [SerializeField]
    protected Material transMaterial;

    //powerup toggles and materials
    protected bool goldBallEnabled;
    [SerializeField]
    protected Material goldBallMaterial;
    protected bool markedBallEnabled;
    [SerializeField]
    protected Material markedBallMaterial;
    protected bool triBallEnabled;

    public bool GoldBallEnabled { get { return goldBallEnabled; } set { goldBallEnabled = value; } }
    public bool MarkedBallEnabled { get { return markedBallEnabled; } set { markedBallEnabled = value; } }
    public bool TriBallEnabled { get { return triBallEnabled; } set { triBallEnabled = value; } }


    private void Awake()
    {
        offsetDifference = maxOffsetSpeed - minOffsetSpeed;
        totalOffset = 0;

        materials = GetComponent<MeshRenderer>().materials;
    }

    private void Start()
    {
        triBallAngleRads = GetComponent<ObjectControls>().TriBallAngleRads;
    }

    // Update is called once per frame
    void Update()
    {
        //activate the aiming line when the ball is held
        if (GetComponent<ObjectControls>().IsHeld)
        {
            float angle = GetComponent<ObjectControls>().Angle;
            float powerPercent = GetComponent<ObjectControls>().PowerPercent;
            aimLine.enabled = true;

            UpdateAimLine(angle, powerPercent);

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

    public void ResetParticleTrail()
    {
        //activates the new particle trail
        ActivateParticleTrail();

        //gets all particles in the scene and destroys the ones that were created by the last round of balls
        GameObject[] particles = GameObject.FindGameObjectsWithTag("ObjectPathParticles");
        List<GameObject> oldParticles = new List<GameObject>();
        foreach (GameObject particle in particles)
        {
            if (particle != particleTrail && particle != null)
            {
                oldParticles.Add(particle);
            }
        }
        foreach (GameObject particle in oldParticles)
        {
            Destroy(particle);
        }
    }

    public void ActivateParticleTrail()
    {
        particleTrail.SetActive(true);
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
        //turns triball visual effects on or off
        leftTriBall.SetActive(triBallEnabled);
        rightTriBall.SetActive(triBallEnabled);
        leftAimLine.gameObject.SetActive(triBallEnabled);
        rightAimLine.gameObject.SetActive(triBallEnabled);

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
        rightTriBall.GetComponent<MeshRenderer>().materials = materials;
    }

    protected virtual void UpdateAimLine(float angle, float powerPercent)
    {

    }

    //reset the material when the application closes
    private void OnApplicationQuit()
    {
        dottedLineMaterial.mainTextureOffset = Vector2.zero;
        dottedLineMaterial.color = Color.white;
    }
}
