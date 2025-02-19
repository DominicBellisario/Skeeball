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
            Debug.Log("shop has gold pow");
            button.onClick.AddListener(delegate { BuyGoldBall(); });
        }
        
    }

    public void BuyGoldBall()
    {
        if (Manager.Instance.Coins >= price)
        {
            shopManager.UpdateCoinText(-price);
            Manager.Instance.Coins -= price;
            Manager.Instance.GoldBallPow++;
            Debug.Log("bought gold pow");
        }
        else { Debug.Log("not enough money"); }
    }
}
