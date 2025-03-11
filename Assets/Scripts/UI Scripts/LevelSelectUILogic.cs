using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectUILogic : MonoBehaviour
{
    [SerializeField] GameObject[] pages;
    [SerializeField] Button[] buttons;
    [SerializeField] ColorBlock normalColors;
    [SerializeField] ColorBlock secretColors;
    int page = 1;

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

    public void UpdatePageNumber(int change)
    {
        //page must be between 1 and number of pages
        if (page + change > 0 && page + change <= pages.Length)
        {
            page += change;

            //deactivate all pages exept the current one
            for (int i = 1; i <= pages.Length; i++)
            {
                if (i == page) {pages[i - 1].SetActive(true); }
                else { pages[i - 1].SetActive(false); }
            }
        }
    }
}
