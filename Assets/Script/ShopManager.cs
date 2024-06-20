using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class ShopManager : MonoBehaviour
{
    public bool _isLimit = false;//限定購入かどうか
    public List<Tuple<string,int>> _items = new List<Tuple<string,int>>();//商品リスト
    [SerializeField] InputField _input_text;
    [SerializeField] InputField _input_value;
    [SerializeField] GameObject _itemPrefab;
    [SerializeField] GameObject _addObj;
    [SerializeField] GameObject _parentObj;
    [SerializeField] GameManager _gm;

    [SerializeField] int _intarval = 0;

    [SerializeField] int _hight;
    void Start()
    {
        ItemDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ItemDisplay()
    {
        // parentObject のすべての子オブジェクトを削除
        foreach (Transform child in _parentObj.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        for (int i = 0;i < _items.Count;i++)
        {
            GameObject obj = Instantiate(_itemPrefab, new Vector3(65 + _intarval * (i / 2), -_intarval * (i % 2) + _hight, 0), this.transform.rotation, _parentObj.transform);
            Item item = obj.GetComponent<Item>();
            item._name = _items[i].Item1;
            item._value = _items[i].Item2;
            item._itemNumber = i;
            item._isLimit = _isLimit;
            item._shopManager = this.gameObject.GetComponent<ShopManager>();
        }
        _addObj.transform.position = new Vector3(65 + _intarval * (_items.Count / 2), -_intarval * (_items.Count % 2) + _hight, 0);
    }
    public void AddItem()
    {
        _items.Add(new Tuple<string, int>(_input_text.text, int.Parse(_input_value.text)));
        ItemDisplay();
    }
    public void Delete(int num)
    {
        _items.Remove(_items[num]);
        ItemDisplay();
    }
    public void Buy(int value,int num)
    {
        if (_gm._money >= value)
        {
            _gm._money -= value;
            if (_isLimit)
            {
                Delete(num);
            }
        }
    }
}
