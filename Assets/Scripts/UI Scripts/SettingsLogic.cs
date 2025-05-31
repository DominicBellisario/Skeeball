using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsLogic : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;
    [SerializeField] Toggle autoBallCamToggle;

    void Start()
    {
        // Load settings from PlayerPrefs and apply them to the UI elements
        volumeSlider.value = PlayerPrefs.GetFloat("masterVolume");
        autoBallCamToggle.isOn = PlayerPrefs.GetInt("autoBallCam") == 1;
    }
}
