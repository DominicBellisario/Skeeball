using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting.Dependencies.NCalc;

public class ShopObject : MonoBehaviour
{
    [SerializeField] ShopManager shopManager;
    [SerializeField] Button button;
    [SerializeField] TextMeshProUGUI itemText;
    [SerializeField] TextMeshProUGUI priceText;
    int price;
    bool sold = false;
    int ID;

    public void SetValues(int _ID, string _itemText, int _price)
    {
        //set variables
        price = _price;
        itemText.text = _itemText;
        priceText.text = "$" + _price;
        ID = _ID;

        //determine what the button will do when clicked
        if (ID == 0) { button.onClick.AddListener(delegate { BuyPowerup(ref Manager.Instance.goldBallPow); }); }
        else if (ID == 1) { button.onClick.AddListener(delegate { BuyPowerup(ref Manager.Instance.markedBallPow); }); }
        else if (ID == 2) { button.onClick.AddListener(delegate { BuyPowerup(ref Manager.Instance.triBallPow); }); }
        else if (ID == 3) { button.onClick.AddListener(delegate { BuyPowerup(ref Manager.Instance.lobBallPow); }); }
        else if (ID == 4) { button.onClick.AddListener(delegate { BuyMultiplier(); }); }
        else if (ID == 5) { button.onClick.AddListener(delegate { BuyStarHoleChanceUpgrade(); }); }
    }

    public void BuyPowerup(ref int powerupReference)
    {
        //sold already
        if (sold) { return; }
        //not enough money
        else if (Manager.Instance.Coins < price) { shopManager.NotEnoughMoney(); }
        //already at max powerups
        else if (Helper.Instance.HasMaxPowerups(powerupReference)) { shopManager.AlreadyAtMaxPowerups(ID); }
        //can be bought
        else
        {
            Manager.Instance.Coins -= price;
            powerupReference++;
            shopManager.UpdateUI();
            SoldOut();
        }
    }

    public void BuyMultiplier()
    {
        if (sold) { return; }
        else if (Manager.Instance.Coins < price) { shopManager.NotEnoughMoney(); }
        else
        {
            Manager.Instance.Coins -= price;
            Manager.Instance.Multiplier++;
            shopManager.UpdateUI();
            SoldOut();
        }
    }

    public void BuyStarHoleChanceUpgrade()
    {
        if (sold) { return; }
        else if (Manager.Instance.Coins < price) { shopManager.NotEnoughMoney(); }
        else
        {
            Manager.Instance.Coins -= price;
            Manager.Instance.StarHoleChanceUpgradesBought++;
            //Debug.Log(Manager.Instance.StarHoleChanceUpgradesBought);
            shopManager.UpdateUI();
            SoldOut();
        }
    }

    private void SoldOut()
    {
        itemText.text = "Sold Out!";
        priceText.text = "X";
        sold = true;
    }
}
