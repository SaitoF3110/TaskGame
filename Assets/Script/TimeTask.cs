using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeTask : MonoBehaviour
{
    [SerializeField] GameManager _gameManager;
    [SerializeField] Text _bounsText;
    [SerializeField] Text _timeText;
    [SerializeField] Text _lastPointText;
    float _bounus;
    int _lastPoint;
    bool _onWork = false;
    public float _time;
    void Start()
    {
        Bonus(_gameManager._cityLevel);
    }

    // Update is called once per frame
    void Update()
    {
        //ボーナス割合計算
        _bounus = Bonus(_gameManager._cityLevel);
        _bounsText.text = "現在のボーナス" + _bounus.ToString("F2") + "%";

        if (_onWork)
        {
            _time += Time.deltaTime;
        }

        //獲得ポイントの計算
        _timeText.text = TimeConbert((int)_time);
        int point = (int)(_time / 600);
        _lastPoint = (int)(point + point * (_bounus / 100));
        _lastPointText.text = "現在の蓄積ポイント：" + point + "\n最終ポイント:" + _lastPoint.ToString();
    }
    float Bonus(int level)
    {
        float sam = 1;
        sam += 0.65f * level;
        return sam;
    }
    string TimeConbert(int time)
    {
        string line = "";
        line += (time / 3600).ToString("00");
        line += ":";
        time = time % 3600;
        line += (time / 60).ToString("00");
        line += ":";
        line += (time % 60).ToString("00");
        return line;
    }
    public void Button()
    {
        _onWork = !_onWork;
    }
}
