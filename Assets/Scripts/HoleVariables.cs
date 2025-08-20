using System.Collections;
using UnityEngine;

public class HoleVariables : MonoBehaviour
{
    //hole text prefab
    [SerializeField] GameObject holeText;
    //how many points the hole is worth
    [SerializeField] int points;
    int startingPoints;
    bool marked = false;
    //wether or not the ball is destroyed when passing through
    [SerializeField] bool destroyBall;
    [SerializeField] MeshRenderer holeRimMesh;
    [SerializeField] ParticleSystem holeScoreParticles;

    //the cylinder's renderer
    [SerializeField] Renderer holeRenderer;
    Color defaultColor;
    int defaultMaterialIndex = 0;
    [SerializeField] Color normalPulseColor;
    [SerializeField] Color badPulseColor;
    //how long the cylinder will pulse for
    [SerializeField] float pulseTime;

    //all possible hole materials.  Each color has their own subset of materials
    [SerializeField] Material[] greenMaterials;
    [SerializeField] Material[] orangeMaterials;
    [SerializeField] Material[] blueMaterials;
    [SerializeField] Material[] redMaterials;
    [SerializeField] Material[] goldMaterials;



    public int Points { get { return points; } }
    public bool Marked { get { return marked; } }
    public bool DestroyBall { get { return destroyBall; } }

    private void Start()
    {
        startingPoints = points;
        // switch the default material of colorblindness mode is on
        if (PlayerPrefs.GetInt("colorblind") == 1) { defaultMaterialIndex = 3; }
        // get the starting color of the hole
        if (holeRenderer != null) { defaultColor = holeRenderer.material.color; }

        //make it a normal hole
        MakeNormalHole();
    }

    public void MakeNormalHole()
    {
        if (startingPoints == 10 || startingPoints == -10) { ChangeMaterial(greenMaterials, defaultMaterialIndex); }
        else if (startingPoints == 20) { ChangeMaterial(orangeMaterials, defaultMaterialIndex); }
        else if (startingPoints == 30) { ChangeMaterial(blueMaterials, defaultMaterialIndex); }
        else if (startingPoints == 50) { ChangeMaterial(redMaterials, defaultMaterialIndex); }
        else if (startingPoints == 100) { ChangeMaterial(goldMaterials, defaultMaterialIndex); }
    }

    /// <summary>
    /// double the points and make the hole rim glow
    /// </summary>
    /// <param name="points"></param>
    public void MakeMarkedHole(bool doublePoints)
    {
        marked = true;
        if (doublePoints) { points *= 2; }
        if (startingPoints == 10) { ChangeMaterial(greenMaterials, 1); }
        else if (startingPoints == 20) { ChangeMaterial(orangeMaterials, 1); }
        else if (startingPoints == 30) { ChangeMaterial(blueMaterials, 1); }
        else if (startingPoints == 50) { ChangeMaterial(redMaterials, 1); }
        else if (startingPoints == 100) { ChangeMaterial(goldMaterials, 1); }
    }

    public void MakeMultiHole()
    {
        if (startingPoints == 10) { ChangeMaterial(greenMaterials, 2); }
        else if (startingPoints == 20) { ChangeMaterial(orangeMaterials, 2); }
        else if (startingPoints == 30) { ChangeMaterial(blueMaterials, 2); }
        else if (startingPoints == 50) { ChangeMaterial(redMaterials, 2); }
        else if (startingPoints == 100) { ChangeMaterial(goldMaterials, 2); }
    }

    public void PlayHoleSound()
    {
        if (startingPoints == 10) { SoundManager.Instance.PlaySound(2, 1); }
        else if (startingPoints == 20) { SoundManager.Instance.PlaySound(2, 2); }
        else if (startingPoints == 30) { SoundManager.Instance.PlaySound(2, 3); }
        else if (startingPoints == 50) { SoundManager.Instance.PlaySound(2, 4); }
        else if (startingPoints == 100) { SoundManager.Instance.PlaySound(2, 5); }
        else if (startingPoints < 0) { SoundManager.Instance.PlaySound(2, 7); }
    }

    private void ChangeMaterial(Material[] color, int materialIndex)
    {
        holeRimMesh.material = color[materialIndex];
    }

    public void SpawnHoleEffects(Color particleColor)
    {
        //spawn ring particle
        ParticleSystem newParticles = Instantiate(holeScoreParticles);
        newParticles.gameObject.transform.parent = transform;
        newParticles.gameObject.transform.position = transform.position;
        newParticles.startRotation3D = transform.rotation.eulerAngles * Mathf.Deg2Rad;
        newParticles.startSize *= transform.localScale.x;
        newParticles.startColor = particleColor;
        newParticles.Play();

        //pulse cylinder if it is not a ring hole
        if (holeRenderer != null)
        {
            StopAllCoroutines();
            holeRenderer.material.color = defaultColor;
            if (points >= 0) { StartCoroutine(PulseCylinder(normalPulseColor)); }
            else { StartCoroutine(PulseCylinder(badPulseColor)); }
        }

    }

    private IEnumerator PulseCylinder(Color startColor)
    {
        //initilize varibales
        float timer = 0f;
        Color endColor = holeRenderer.material.color;
        Vector3 startColorRGB;
        Vector3 endColorRGB;
        Vector3 currentColorRGB;
        float t;

        //change cylinder color to start color
        holeRenderer.material.color = startColor;
        //lerp color to end color
        while (timer < pulseTime)
        {
            timer += Time.deltaTime;
            t = timer / pulseTime;
            startColorRGB = new(startColor.r, startColor.g, startColor.b);
            endColorRGB = new(endColor.r, endColor.g, endColor.b);
            currentColorRGB = Vector3.Lerp(startColorRGB, endColorRGB, t);

            holeRenderer.material.color = new Color(currentColorRGB.x, currentColorRGB.y, currentColorRGB.z, 1);
            yield return new WaitForEndOfFrame();
        }

    }
}
