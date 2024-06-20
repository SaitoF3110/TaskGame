using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public Text _text;
    public Text _valueText;

    public ShopManager _shopManager;
    public string _name;
    public int _value;
    public int _itemNumber;
    public bool _isLimit;
    void Start()
    {
        _text.text = _name;
        _valueText.text = _value + "‰~";
    }
    public void Delete()
    {
        _shopManager.Delete(_itemNumber);
        
    }
    public void Buy()
    {
        _shopManager.Buy(_value,_itemNumber);
    }
}
