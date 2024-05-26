using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectActive : MonoBehaviour
{
    [SerializeField] GameObject _obj;
    public void UIOn(bool active)
    {
        _obj.SetActive(active);
    }
}
