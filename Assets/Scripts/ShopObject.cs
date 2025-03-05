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

    public void SetValues(int ID, string _itemText, int _price)
    {
        //set variables
        price = _price;
        itemText.text = _itemText;
        priceText.text = "$" + _price;

        //determine what the button will do when clicked
        if (ID == 0) { button.onClick.AddListener(delegate { BuyGoldBall(); }); }
        else if (ID == 1) { button.onClick.AddListener(delegate { BuyMarkedBall(); }); }
        else if (ID == 2) { button.onClick.AddListener(delegate { BuyTriBall(); }); }
        else if (ID == 3) { button.onClick.AddListener(delegate { BuyLobBall(); }); }
        else if (ID == 4) { button.onClick.AddListener(delegate { BuyMultiplier(); }); }
    }

    public void BuyGoldBall()
    {
        if (Manager.Instance.Coins >= price && !sold && Helper.Instance.HasMaxPowerups(Manager.Instance.GoldBallPow))
        {
            Manager.Instance.Coins -= price;
            Manager.Instance.GoldBallPow++;
            shopManager.UpdateUI();
            SoldOut();
        }
        else { shopManager.NotEnoughMoney(); }
    }
    public void BuyMarkedBall()
    {
        if (Manager.Instance.Coins >= price && !sold && Helper.Instance.HasMaxPowerups(Manager.Instance.MarkedBallPow))
        {
            Manager.Instance.Coins -= price;
            Manager.Instance.MarkedBallPow++;
            shopManager.UpdateUI();
            SoldOut();
        }
        else { shopManager.NotEnoughMoney(); }
    }
    public void BuyTriBall()
    {
        if (Manager.Instance.Coins >= price && !sold && Helper.Instance.HasMaxPowerups(Manager.Instance.TriBallPow))
        {
            Manager.Instance.Coins -= price;
            Manager.Instance.TriBallPow++;
            shopManager.UpdateUI();
            SoldOut();
        }
        else { shopManager.NotEnoughMoney(); }
    }
    public void BuyLobBall()
    {
        if (Manager.Instance.Coins >= price && !sold && Helper.Instance.HasMaxPowerups(Manager.Instance.LobBallPow))
        {
            Manager.Instance.Coins -= price;
            Manager.Instance.LobBallPow++;
            shopManager.UpdateUI();
            SoldOut();
        }
        else { shopManager.NotEnoughMoney(); }
    }
    public void BuyMultiplier()
    {
        if (Manager.Instance.Coins >= price && !sold)
        {
            Manager.Instance.Coins -= price;
            Manager.Instance.Multiplier++;
            shopManager.UpdateUI();
            SoldOut();
        }
        else { shopManager.NotEnoughMoney(); }
    }

    private void SoldOut()
    {
        itemText.text = "Sold Out!";
        priceText.text = "X";
        sold = true;
    }
}
