using CI.QuickSave;
using CI.QuickSave.Core.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class TimeTask : MonoBehaviour
{
    //セーブ設定
    private protected QuickSaveSettings m_saveSettings;
    public int _taskCount = 0;

    [SerializeField] GameManager _gameManager;
    [SerializeField] Text _bounsText;
    [SerializeField] Text _timeText;
    [SerializeField] Text _lastPointText;
    float _bounus;
    int _lastPoint;
    bool _onWork = false;
    public float _time;


    DateTime TodayNow;
    void Start()
    {
        // QuickSaveSettingsのインスタンスを作成
        m_saveSettings = new QuickSaveSettings();
        // 暗号化の方法 
        m_saveSettings.SecurityMode = SecurityMode.Aes;
        // Aesの暗号化キー
        m_saveSettings.Password = "Password";
        // 圧縮の方法
        m_saveSettings.CompressionMode = CompressionMode.Gzip;
        // データの保存先をApplication.dataPathに変更
        QuickSaveGlobalSettings.StorageLocation = Application.temporaryCachePath;

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
    public void LoadUserData()
    {
        //ファイルが無ければ無視
        if (FileAccess.Exists("SaveData", false) == false)
        {
            return;
        }

        // QuickSaveReaderのインスタンスを作成
        QuickSaveReader reader = QuickSaveReader.Create("SaveData", m_saveSettings);
        int y = 0;
        int m = 0;
        int d = 0;
        try
        {
            y = reader.Read<int>("年");
            m = reader.Read<int>("月");
            d = reader.Read<int>("日");
        }
        catch (QuickSaveException e)
        {
            _taskCount = 0;
            Debug.Log("でーたなし");
        }
        //時間を取得
        TodayNow = DateTime.Now;
        if (TodayNow.Year != y || TodayNow.Month != m || TodayNow.Day != d)
        {
            TimeConversion();
        }
    }
    public void TimeConversion()
    {
        _gameManager._cityPoint += _lastPoint;
        _gameManager._money += _lastPoint;
        _time = 0;
    }
    public void SaveUserData()
    {
        QuickSaveWriter writer = QuickSaveWriter.Create("SaveData", m_saveSettings);
        writer.Write("蓄積時間",_time);
        //時間を取得
        TodayNow = DateTime.Now;
        writer.Write("年", TodayNow.Year);
        writer.Write("月", TodayNow.Month);
        writer.Write("日", TodayNow.Day);
        writer.Commit();
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
