using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopObject : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] TextMesh itemText;
    [SerializeField] TextMesh priceText;
    
    public void SetValues(string _itemText, string _priceText)
    {
        itemText.text = _itemText;
        priceText.text = _priceText;
    }
}
