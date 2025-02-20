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
    
    public void SetValues(int ID, string _itemText, int _price)
    {
        price = _price;
        itemText.text = _itemText;
        priceText.text = "$" + _price;
        if (ID == 0)
        {
            button.onClick.AddListener(delegate { BuyGoldBall(); });
        }
        else if (ID == 1)
        {
            button.onClick.AddListener(delegate { BuyMarkedBall(); });
        }
        else if (ID == 2)
        {
            button.onClick.AddListener(delegate { BuyTriBall(); });
        }
        else if (ID == 3)
        {
            button.onClick.AddListener(delegate { BuyLobBall(); });
        }

    }

    public void BuyGoldBall()
    {
        if (Manager.Instance.Coins >= price)
        {
            shopManager.UpdateCoinText(-price);
            Manager.Instance.Coins -= price;
            Manager.Instance.GoldBallPow++;
            shopManager.UpdateGoldText(1);
        }
        else { Debug.Log("not enough money"); }
    }
    public void BuyMarkedBall()
    {
        if (Manager.Instance.Coins >= price)
        {
            shopManager.UpdateCoinText(-price);
            Manager.Instance.Coins -= price;
            Manager.Instance.MarkedBallPow++;
            shopManager.UpdateMarkedText(1);
        }
        else { Debug.Log("not enough money"); }
    }
    public void BuyTriBall()
    {
        if (Manager.Instance.Coins >= price)
        {
            shopManager.UpdateCoinText(-price);
            Manager.Instance.Coins -= price;
            Manager.Instance.TriBallPow++;
            shopManager.UpdateTriText(1);
        }
        else { Debug.Log("not enough money"); }
    }
    public void BuyLobBall()
    {
        if (Manager.Instance.Coins >= price)
        {
            shopManager.UpdateCoinText(-price);
            Manager.Instance.Coins -= price;
            Manager.Instance.LobBallPow++;
            shopManager.UpdateLobText(1);
        }
        else { Debug.Log("not enough money"); }
    }
}
