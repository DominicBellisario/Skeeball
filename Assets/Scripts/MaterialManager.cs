using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MaterialManager : MonoBehaviour
{
    float timer = 0;
    [SerializeField] Material[] pulseMaterials;
    [SerializeField] float pulseSpeed;
    [SerializeField] float pulseIntensity;
    float pulseValue;

    public static MaterialManager Instance { get; private set; }

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
    }

    // Update is called once per frame
    void Update()
    {
        timer += pulseSpeed * Time.deltaTime;
        pulseValue = Mathf.Sin(timer) * pulseIntensity;
        foreach (Material material in pulseMaterials)
        {
            material.SetColor("_EmissionColor", new Color(pulseValue, pulseValue, pulseValue));
            Debug.Log(material.color);
        }
    }

    private void OnApplicationQuit()
    {
        foreach (Material material in pulseMaterials)
        {
            material.SetColor("_EmissionColor", new Color(0, 0, 0));
        }
    }
}
