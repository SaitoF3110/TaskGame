using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResouceIntroduse : MonoBehaviour
{
    [SerializeField] Text _text;
    public int _level = 1;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        double _water = 1200 * Mathf.Pow(1.1f, _level);
        _text.text = _water.ToString("F0");
    }
}
