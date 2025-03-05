using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] Vector2 itemRange;

    [SerializeField] GameObject[] itemObjects;
    [SerializeField] string[] possibleItems;
    [SerializeField] int[] prices;

    #region UI Objects and Values
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] TextMeshProUGUI scoreTxt;
    [SerializeField] TextMeshProUGUI multiTxt;
    [SerializeField] TextMeshProUGUI goldBallTxt;
    [SerializeField] TextMeshProUGUI markedBallTxt;
    [SerializeField] TextMeshProUGUI triBallTxt;
    [SerializeField] TextMeshProUGUI lobBallTxt;
    #endregion


    List<GameObject> activeItems = new();
    int itemAmount;

    private void Start()
    {
        //inventory starts disabled
        ToggleInventory();

        //set UI text
        scoreTxt.text = "Score: " + Manager.Instance.TotalPoints;
        UpdateUI();

        //start with all objects deactivated
        foreach (GameObject item in itemObjects)
        {
            item.SetActive(false);
        }

        //gets a random number of item objects that will be shown
        itemAmount = Helper.Instance.RandomInt(itemRange.x, itemRange.y);
        //activates this number of item objects in the shop
        for (int i = 0; i < itemAmount; i++)
        {
            ActivateItem();
        }

        //assigns the item object its good and price
        foreach (GameObject item in activeItems)
        {
            //picks a random item
            int itemID = Helper.Instance.RandomInt(0, possibleItems.Length - 1);
            //assign values
            item.GetComponent<ShopObject>().SetValues(itemID, possibleItems[itemID], prices[itemID]);
        }
    }

    private void ActivateItem()
    {
        int itemIndex = Helper.Instance.RandomInt(0, itemRange.y - 1);
        //if this item object is already active, pick another one
        if (!itemObjects[itemIndex].activeSelf)
        {
            itemObjects[itemIndex].SetActive(true);
            activeItems.Add(itemObjects[itemIndex]);
        }
        else
        {
            ActivateItem();
        }
    }

    public void ToggleInventory()
    {
        goldBallTxt.gameObject.SetActive(!goldBallTxt.gameObject.activeSelf);
        markedBallTxt.gameObject.SetActive(!markedBallTxt.gameObject.activeSelf);
        triBallTxt.gameObject.SetActive(!triBallTxt.gameObject.activeSelf);
        lobBallTxt.gameObject.SetActive(!lobBallTxt.gameObject.activeSelf);
    }

    //functions that update ui
    public void UpdateUI()
    {
        coinText.text = Manager.Instance.Coins.ToString();
        goldBallTxt.text = "Gold Ball: " + Manager.Instance.GoldBallPow;
        markedBallTxt.text = "Marked Ball: " + Manager.Instance.MarkedBallPow;
        triBallTxt.text = "Triball: " + Manager.Instance.TriBallPow;
        lobBallTxt.text = "Lobball: " + Manager.Instance.LobBallPow;
        multiTxt.text = "Multi: " + Manager.Instance.Multiplier;
    }

    //coin counter flashes red
    public void NotEnoughMoney()
    {
        coinText.color = Color.red;
        StartCoroutine(ResetUIColor());
    }
    IEnumerator ResetUIColor()
    {
        yield return new WaitForSeconds(1);
        coinText.color = Color.white;
        goldBallTxt.color = Color.white;
        markedBallTxt.color = Color.white;
        triBallTxt.color = Color.white;
        lobBallTxt.color = Color.white;
    }

    //powerup counter in question flashes red
    public void AlreadyAtMaxPowerups(int ID)
    {
        if (ID == 0) { goldBallTxt.color = Color.red; }
        else if (ID == 1) { markedBallTxt.color = Color.red; }
        else if (ID == 2) { triBallTxt.color = Color.red; }
        else if (ID == 3) { lobBallTxt.color = Color.red; }
        StartCoroutine(ResetUIColor());
    }

    
}
