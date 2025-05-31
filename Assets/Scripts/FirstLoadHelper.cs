using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstLoadHelper : MonoBehaviour
{
    void Awake()
    {
        //GAME UNLOCKS
        PlayerPrefs.GetInt("unlockSecret_0", 1);
        for (int i = 1; i <= Manager.Instance.NumberOfLevels; i++)
        {
            //all levels but the 1st one start out locked 
            PlayerPrefs.GetInt("unlockLevel_" + i, 0);
            //all secrets except the first one start out locked
            PlayerPrefs.GetInt("unlockSecret_" + i, 0);
        }
        PlayerPrefs.GetInt("selectedSkin", 1);
        PlayerPrefs.SetInt("unlockLevel_1", 1);

        //SETTINGS
        PlayerPrefs.GetFloat("masterVolume", 5f);
        PlayerPrefs.GetInt("autoBallCam", 1);
        
        PlayerPrefs.Save();
    }
}
