using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectUILogic : MonoBehaviour
{
    [SerializeField] Button[] buttons;
    [SerializeField] ColorBlock normalColors;
    [SerializeField] ColorBlock secretColors;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Manager.Instance.NumberOfLevels; i++)
        {
            if (PlayerPrefs.GetInt("unlockLevel_" + (i + 1)) == 0)
            {
                buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = "X";
                buttons[i].enabled = false;
            }

            if (PlayerPrefs.GetInt("unlockSecret_" + (i + 1)) == 0)
            {
                buttons[i].colors = normalColors;
            }
            else
            {
                buttons[i].colors = secretColors;
            }
        }
    }
}
