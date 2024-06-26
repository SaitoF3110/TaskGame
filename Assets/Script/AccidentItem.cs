using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccidentItem : MonoBehaviour
{
    [SerializeField] Text _text;
    [SerializeField] Text _numberText;
    public ArmyManager _am;
    public string _name;
    public int _point;
    public int _itemCount;
    void Start()
    {
        _text.text = _name;
        _numberText.text = _point + "P";
    }
    public void Done()
    {
        _am.Done(_itemCount);
    }
}
