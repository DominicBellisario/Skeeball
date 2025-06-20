using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectEffects : MonoBehaviour
{
    [SerializeField] protected Rigidbody rb;
    //shows the trajectory of the object while aiming
    [SerializeField] protected LineRenderer aimLine;
    [SerializeField] protected LineRenderer leftAimLine;
    [SerializeField] protected LineRenderer rightAimLine;
    //the aim line material
    [SerializeField] protected Material dottedLineMaterial;
    [SerializeField] protected float dottedLineAlpha = 0.8f;

    //the particle trail
    [SerializeField] protected GameObject particleTrail;
    [SerializeField] protected ParticleSystem activatePowerupParticles;
    [SerializeField] protected ParticleSystem deactivatePowerupParticles;
    [SerializeField] protected ParticleSystem deathParticles;

    //the objects that appear when triball is active
    [SerializeField] protected GameObject leftTriBall;
    [SerializeField] protected GameObject rightTriBall;

    protected float triBallAngleRads;

    //the min and max texture speed
    [SerializeField] protected float minOffsetSpeed;
    [SerializeField] protected float maxOffsetSpeed;
    protected float offsetDifference;
    protected float totalOffset;

    //a list of all materials cirrently applied to the object
    protected Material[] materials;

    //the base skin for the ball
    [SerializeField] protected Material defaultMaterial;
    //when a powerup is disabled, this material replaces the powerup material
    [SerializeField] protected Material transMaterial;

    //powerup toggles and materials
    protected bool goldBallEnabled;
    [SerializeField] protected Material goldBallMaterial;
    protected bool markedBallEnabled;
    [SerializeField] protected Material markedBallMaterial;
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

        //update ball materials to the selected skin
        Material[] getBallSkins = MaterialManager.Instance.GetBallMaterialSet(PlayerPrefs.GetInt("selectedSkin"));
        defaultMaterial = getBallSkins[0];
        goldBallMaterial = getBallSkins[1];
        if(!goldBallEnabled){materials[0] = defaultMaterial;}
        else{materials[0] = goldBallMaterial;}
        UpdateMaterials();
    }

    // Update is called once per frame
    protected abstract void Update();

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
        particleTrail.transform.localScale = Vector3.one;
    }

    //toggles for powerups
    public void ToggleGoldBall(bool buttonPressed)
    {
        
        goldBallEnabled = !goldBallEnabled;
        if (goldBallEnabled)
        {
            materials[0] = goldBallMaterial;
            if (buttonPressed)
            {
                SpawnParticles(activatePowerupParticles, Color.yellow, transform);
                //play powerup activate sound
                SoundManager.Instance.PlaySound(7, 26);
            }
        }
        else
        {
            materials[0] = defaultMaterial;
            if (buttonPressed)
            {
                SpawnParticles(deactivatePowerupParticles, Color.yellow, transform);
                //play powerup deactivate sound
                SoundManager.Instance.PlaySound(7, 9);
            }
        }
        UpdateMaterials();
    }
    public void ToggleMarkedBall(bool buttonPressed)
    {
        markedBallEnabled = !markedBallEnabled;
        if (markedBallEnabled)
        {
            materials[1] = markedBallMaterial;
            if (buttonPressed)
            {
                SpawnParticles(activatePowerupParticles, Color.red, transform);
                SoundManager.Instance.PlaySound(8, 27);
            }
        }
        else
        {
            materials[1] = transMaterial;
            if (buttonPressed)
            {
                SpawnParticles(deactivatePowerupParticles, Color.red, transform);
                SoundManager.Instance.PlaySound(8, 9);
            }
        }
        UpdateMaterials();
    }
    public void ToggleTriBall(bool buttonPressed)
    {
        triBallEnabled = !triBallEnabled;
        if (buttonPressed)
        {
            if (triBallEnabled)
            {
                SpawnParticles(activatePowerupParticles, Color.blue, transform);
                SoundManager.Instance.PlaySound(9, 30);
            }
            else
            {
                SpawnParticles(deactivatePowerupParticles, Color.blue, transform);
                SoundManager.Instance.PlaySound(9, 9);
            }
        }
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

    public void ToggleLobBallEffects(bool enabled)
    {
        if (enabled)
        {
            SpawnParticles(activatePowerupParticles, Color.green, transform);
            SoundManager.Instance.PlaySound(10, 28, 1.5f, 1f);
        }
        else
        {
            SpawnParticles(deactivatePowerupParticles, Color.green, transform);
            SoundManager.Instance.PlaySound(10, 9, 1.5f, 1f);
        }
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

    /// <summary>
    /// spawns particles at the balls position and changes their color
    /// </summary>
    /// <param name="particles"></param>
    /// <param name="color"></param>
    private void SpawnParticles(ParticleSystem particles, Color color, Transform parent)
    {
        ParticleSystem newParticles = Instantiate(particles);
        newParticles.gameObject.transform.parent = parent;
        newParticles.gameObject.transform.localScale = Vector3.one;
        newParticles.gameObject.transform.position = transform.position;
        newParticles.startColor = color;
        newParticles.Play();
    }

    public void SpawnDeathParticles()
    {
        SpawnParticles(deathParticles, Color.white, null);
    }

    //reset the material when the application closes
    private void OnApplicationQuit()
    {
        dottedLineMaterial.mainTextureOffset = Vector2.zero;
        dottedLineMaterial.color = Color.white;
    }
}
