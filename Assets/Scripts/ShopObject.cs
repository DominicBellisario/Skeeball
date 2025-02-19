using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopObject : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] TextMeshProUGUI itemText;
    [SerializeField] TextMeshProUGUI priceText;
    
    public void SetValues(string _itemText, string _priceText)
    {
        itemText.text = _itemText;
        priceText.text = _priceText;
    }
}
