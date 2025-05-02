using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstLoadHelper : MonoBehaviour
{
    void Awake()
    {
        for (int i = 1; i <= Manager.Instance.NumberOfLevels; i++)
        {
            //all levels but the 1st one start out locked 
            PlayerPrefs.GetInt("unlockLevel_" + i, 0);
            //all secrets except the first one start out locked
            PlayerPrefs.GetInt("unlockSecret_" + i, 0);
        }
        PlayerPrefs.GetInt("selectedSkin", 1);
        PlayerPrefs.SetInt("unlockLevel_1", 1);
        
        PlayerPrefs.Save();
    }
}
