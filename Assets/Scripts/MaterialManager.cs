using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MaterialManager : MonoBehaviour
{
    float pulseTimer = 0;

    [SerializeField] Material[] pulseMaterials;
    [SerializeField] float pulseSpeed;
    [SerializeField] float pulseIntensity;
    float pulseValue;

    [SerializeField] Material rainbowFastMaterial;
    [SerializeField] float rainbowFastCycleSpeed;
    [SerializeField] Material rainbowSlowMaterial;
    [SerializeField] float rainbowSlowCycleSpeed;

    [SerializeField] Color32[] rainbowColors;

    [SerializeField] Material[] ballNormalMaterials;
    [SerializeField] Material[] ballGoldMaterials;
    [SerializeField] Material pitchBlack;

    public static MaterialManager Instance { get; private set; }
    public Material PitchBlack { get { return pitchBlack; } }

    // Start is called before the first frame update
    void Awake()
    {
        //create singleton instance
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }

        StartCoroutine(CycleRainbow(rainbowFastCycleSpeed, rainbowFastMaterial));
        StartCoroutine(CycleRainbow(rainbowSlowCycleSpeed, rainbowSlowMaterial));
    }

    // Update is called once per frame
    void Update()
    {
        //pulse all pulse materials
        pulseTimer += pulseSpeed * Time.deltaTime;
        pulseValue = Mathf.Sin(pulseTimer) * pulseIntensity;
        foreach (Material material in pulseMaterials)
        {
            material.SetColor("_EmissionColor", new Color(pulseValue, pulseValue, pulseValue));
        }
    }

    IEnumerator CycleRainbow(float cycleSpeed, Material rainbowMaterial)
    {
        int i = 0;
        while (true)
        {
            for (float interpolant = 0; interpolant < 1f; interpolant += cycleSpeed * Time.deltaTime)
            {
                rainbowMaterial.SetColor("_Color", Color.Lerp(rainbowColors[i % rainbowColors.Length], rainbowColors[(i + 1) % rainbowColors.Length], interpolant));
                yield return null;
            }
            i++; 
        }
    }

    private void OnApplicationQuit()
    {
        //reset all materials
        foreach (Material material in pulseMaterials)
        {
            material.SetColor("_EmissionColor", new Color(0, 0, 0));
        }
        rainbowFastMaterial.SetColor("_Color", Color.red);
        rainbowSlowMaterial.SetColor("_Color", Color.red);
    }

    public Material[] GetBallMaterialSet(int materialNumber)
    {
        return new Material[] { ballNormalMaterials[materialNumber], ballGoldMaterials[materialNumber] };
    }
}
