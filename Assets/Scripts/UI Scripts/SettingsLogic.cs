using UnityEngine;
using UnityEngine.UI;

public class SettingsLogic : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;
    [SerializeField] Toggle autoBallCamToggle;
    [SerializeField] Toggle colorBlindToggle;
    [SerializeField] Toggle ballPathToggle;

    void Start()
    {
        // Load settings from PlayerPrefs and apply them to the UI elements
        volumeSlider.value = PlayerPrefs.GetFloat("masterVolume");
        SoundManager.Instance.StopSound(0);
        autoBallCamToggle.isOn = PlayerPrefs.GetInt("autoBallCam") == 1;
        colorBlindToggle.isOn = PlayerPrefs.GetInt("colorblind") == 1;
        ballPathToggle.isOn = PlayerPrefs.GetInt("enablePaths") == 1;
    }
}
