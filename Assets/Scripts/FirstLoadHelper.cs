using UnityEngine;

public class FirstLoadHelper : MonoBehaviour
{
    void Awake()
    {
        //if the game is loaded for the first time, set all PlayerPrefs to default values
        if (!PlayerPrefs.HasKey("firstLoad"))
        {
            SetDefaultPlayerPrefs();
            PlayerPrefs.SetInt("firstLoad", 1);
        }
    }

    private void SetDefaultPlayerPrefs()
    {
        //GAME UNLOCKS
        PlayerPrefs.SetInt("unlockSecret_0", 1);
        for (int i = 1; i <= Manager.Instance.NumberOfLevels; i++)
        {
            //all levels but the 1st one start out locked 
            PlayerPrefs.SetInt("unlockLevel_" + i, 0);
            //all secrets except the first one start out locked
            PlayerPrefs.SetInt("unlockSecret_" + i, 0);
        }
        PlayerPrefs.GetInt("selectedSkin", 1);
        PlayerPrefs.SetInt("unlockLevel_1", 1);

        //SETTINGS
        PlayerPrefs.SetFloat("masterVolume", 5f);
        PlayerPrefs.SetInt("autoBallCam", 1);
        
        PlayerPrefs.Save();
    }
}
