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
    int numberOfCoins = 0;
    [SerializeField] TextMeshProUGUI scoreTxt;
    [SerializeField] TextMeshProUGUI multiTxt;
    [SerializeField] TextMeshProUGUI goldBallTxt;
    int goldBallCount = 0;
    [SerializeField] TextMeshProUGUI markedBallTxt;
    int markedBallCount = 0;
    [SerializeField] TextMeshProUGUI triBallTxt;
    int triBallCount = 0;
    [SerializeField] TextMeshProUGUI lobBallTxt;
    int lobBallCount = 0;
    #endregion


    List<GameObject> activeItems = new();
    int itemAmount;

    private void Start()
    {
        //inventory starts disabled
        ToggleInventory();

        //set UI text
        Debug.Log(Manager.Instance.TotalPoints);
        scoreTxt.text = "Score: " + Manager.Instance.TotalPoints;
        multiTxt.text = "Multi: " + Manager.Instance.Multiplier;
        UpdateCoinText(Manager.Instance.Coins);
        UpdateGoldText(Manager.Instance.GoldBallPow);
        UpdateMarkedText(Manager.Instance.MarkedBallPow);
        UpdateTriText(Manager.Instance.TriBallPow);
        UpdateLobText(Manager.Instance.LobBallPow);

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
    public void UpdateCoinText(int amount)
    {
        numberOfCoins += amount;
        coinText.text = numberOfCoins.ToString();
    }
    public void UpdateGoldText(int amount)
    {
        goldBallCount += amount;
        goldBallTxt.text = "Gold Ball: " + goldBallCount;
    }
    public void UpdateMarkedText(int amount)
    {
        markedBallCount += amount;
        markedBallTxt.text = "Marked Ball: " + markedBallCount;
    }
    public void UpdateTriText(int amount)
    {
        triBallCount += amount;
        triBallTxt.text = "Triball: " + triBallCount;
    }
    public void UpdateLobText(int amount)
    {
        lobBallCount += amount;
        lobBallTxt.text = "Lobball: " + lobBallCount;
    }
}
