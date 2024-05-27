using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int _cityLevel = 1;
    public int _cityPoint = 0;
    public int _money = 0;

    [SerializeField] Text _levelText;
    [SerializeField] Text _pointText;
    [SerializeField] Text _moneyText;

    public int _nowExp = 10;
    public int _nextExp;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _levelText.text = _cityLevel.ToString();
        _pointText.text = "都市ポイント：" + _cityPoint.ToString();
        _moneyText.text = "資金：" + _money.ToString();


        if (Input.GetKeyDown(KeyCode.Space))
        {
            Exp();
        }
    }
    void Exp()
    {
        int lv = 0;
        int experience = _nowExp;
        while (true)
        {
            int xp = (int)(Mathf.Pow(1.01f,lv) * 50);
            experience -= xp;
            if(experience < 0)
            {
                experience += xp;
                _nextExp = xp - experience;
                break;
            }
            lv++;
        }
        _cityLevel = lv + 1;
        
    }
    void AddPoints(int point)
    {
        _cityPoint += point;
        _money += point;
    }
    void AddExp(int exp)
    {
        _nowExp += exp;
    }
}
