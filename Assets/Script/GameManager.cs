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
        Screen.SetResolution(270, 608, false);
    }

    // Update is called once per frame
    void Update()
    {
        _levelText.text = _cityLevel.ToString();
        _pointText.text = "ìsésÉ|ÉCÉìÉgÅF" + _cityPoint.ToString();
        _moneyText.text = "éëã‡ÅF" + YenConvert(_money) + "â~";

        Exp();
    }
    string YenConvert(int money)
    {
        string ms = money.ToString();
        List<char> list = new List<char>();
        string ans = "";
        for(int i = ms.Length - 1;i >= 0; i--)
        {
            if(ms.Length - i - 1 == 4)
            {
                list.Add('ñú');
            }
            if (ms.Length - i - 1 == 8)
            {
                list.Add('â≠');
            }
            list.Add(ms[i]);
        }
        list.Reverse();
        ans = string.Join("", list.ToArray());
        return ans;
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
    public void AddPoints(int point)
    {
        _cityPoint += point;
        _money += point;
    }
    public void AddExp(int exp)
    {
        _nowExp += exp;
    }
}
