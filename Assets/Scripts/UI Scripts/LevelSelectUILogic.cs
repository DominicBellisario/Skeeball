using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectUILogic : MonoBehaviour
{
    [SerializeField] GameObject[] pages;
    [SerializeField] Button[] buttons;

    [SerializeField] Sprite disabledSprite;
    [SerializeField] Sprite normalSprite;
    [SerializeField] Sprite secretSprite;

    int page = 1;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Manager.Instance.NumberOfLevels; i++)
        {
            //if the level is not unlocked, disable the button
            if (PlayerPrefs.GetInt("unlockLevel_" + (i + 1)) == 0)
            {
                buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = "X";
                buttons[i].GetComponent<Image>().sprite = disabledSprite;
                buttons[i].enabled = false;
            }
            //level is unlocked
            else
            {
                buttons[i].GetComponent<Image>().sprite = normalSprite;
            }

            //secret is unlocked
            if (PlayerPrefs.GetInt("unlockSecret_" + (i + 1)) == 1)
            {
                buttons[i].GetComponent<Image>().sprite = secretSprite;
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
                if (i == page) { pages[i - 1].SetActive(true); }
                else { pages[i - 1].SetActive(false); }

                //play high ui sound
                if (change > 0) { SoundManager.Instance.PlayUISound(19); }
                //play low ui sound
                else { SoundManager.Instance.PlayUISound(20); }
            }
        }
    }
}
