using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUILogic : MonoBehaviour
{
    [SerializeField] GameObject[] pages;
    [SerializeField] GameObject skinSphere;
    [SerializeField] TextMeshProUGUI skinDescription;
    [SerializeField] Button[] buttons;

    [SerializeField] Sprite normalSprite;
    [SerializeField] Sprite secretSprite;
    [SerializeField] Sprite selectedSprite;
    int page = 1;

    // Start is called before the first frame update
    void Start()
    {
        UpdateButtons(PlayerPrefs.GetInt("selectedSkin"));
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
            }

            //play high ui sound
            if (change > 0) { SoundManager.Instance.PlayUISound(19); }
            //play low ui sound
            else { SoundManager.Instance.PlayUISound(20); }
        }
    }

    public void UpdateButtons(int selectedButtonIndex)
    {
        //update the selected button index to the one that was clicked
        PlayerPrefs.SetInt("selectedSkin", selectedButtonIndex);
        PlayerPrefs.Save();

        //update the description and skin on the display sphere to the name and material of the selected skin
        skinDescription.text = MaterialManager.Instance.GetBallMaterialSet(selectedButtonIndex)[0].name;
        skinSphere.GetComponent<MeshRenderer>().material = MaterialManager.Instance.GetBallMaterialSet(selectedButtonIndex)[0];

        //look at each button
        for (int i = 0; i <= Manager.Instance.NumberOfLevels; i++)
        {
            //deactivate locked buttons and make their sphere black
            if (PlayerPrefs.GetInt("unlockSecret_" + i) == 0)
            {
                buttons[i].GetComponentInChildren<MeshRenderer>().material = MaterialManager.Instance.PitchBlack;
                buttons[i].GetComponent<Image>().sprite = normalSprite;
                buttons[i].enabled = false;
            }
            //activate unlocked buttons and make their sphere the color of the skin
            else
            {
                buttons[i].GetComponentInChildren<MeshRenderer>().material = MaterialManager.Instance.GetBallMaterialSet(i)[0];
                buttons[i].GetComponent<Image>().sprite = secretSprite;
                buttons[i].enabled = true;
            }
            //make the selected button color selectedColor
            if (i == selectedButtonIndex)
            {
                buttons[i].GetComponent<Image>().sprite = selectedSprite;
            }
        }
    }
}