using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] Vector2 itemRange;
    [SerializeField] GameObject[] itemObjects;
    [SerializeField] string[] possibleItems;
    [SerializeField] int[] prices;


    List<GameObject> activeItems = new();
    int itemAmount;

    private void Start()
    {
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
            item.GetComponent<ShopObject>().SetValues(possibleItems[itemID], "$" + prices[itemID]);
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
}
