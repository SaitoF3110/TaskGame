using CI.QuickSave;
using CI.QuickSave.Core.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Numerics;

public class ResouceIn : MonoBehaviour
{
    [SerializeField] Text _text;
    [SerializeField] Text _text2;
    [SerializeField] Text _needsText;
    [SerializeField] Text _title;
    [SerializeField] Slider _slider;
    [SerializeField] Image _image;
    [SerializeField] Color _color;  
    [SerializeField] Color _overColor;

    [SerializeField] GameManager _gm;
    [SerializeField] ResouceManager _resouceManager;

    public string _name;
    public string _unit;
    public int _level;
    public int _exp;
    public ulong _needsSum;
    public int _needs;

    public int _intro;

    public double _resouce;

    float _time;

    private protected QuickSaveSettings m_saveSettings;
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
        LoadUserData();
    }

    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime;
        if (_time > 1)
        {
            SaveUserData();
            _time = 0;
        }

        _title.text = _name +" Lv." + _level;
        _exp = (int)(20 + _level / 10);
        _resouce = _intro *  Mathf.Pow(1.3f, _level); ;
        _text.text = "生産量" + _resouce.ToString("F0") + _unit + "/日";
        _text2.text = _exp.ToString("F0") + "point";
        _needsText.text = "必要量" + _needsSum.ToString() + _unit +"/日";

        _slider.value = (float)_needsSum;
        _slider.maxValue = (float)_resouce;
        if (_resouce < _needsSum)
        {
            _image.color = _overColor;
        }
        else
        {
            _image.color = _color;
        }
        Value();
    }
    public void Value()
    {
        _needsSum = (ulong)(_needs * _gm._population);
    }
    public void LevelUp()
    {
        if (_gm._cityPoint >= _exp)
        {
            _level++;
            _gm._cityPoint -= _exp;
        }
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
            _level = int.Parse(reader.Read<string>(_name + "レベル"));
            y = reader.Read<int>("年R");
            m = reader.Read<int>("月R");
            d = reader.Read<int>("日R");
        }
        catch (QuickSaveException e)
        {
            _level = 1;
        }
        //時間を取得
        TodayNow = DateTime.Now;

        if (TodayNow.Year != y || TodayNow.Month != m || TodayNow.Day != d)
        {
            if (_resouce < _needsSum)
            {
                _resouceManager._happyValue -= 0.2f;
            }
            else
            {
                _resouceManager._happyValue += 0.2f;
            }
        }
    }
    /// <summary>
    /// データセーブ
    /// </summary>
    public void SaveUserData()
    {
        QuickSaveWriter writer = QuickSaveWriter.Create("SaveData", m_saveSettings);

        writer.Write(_name + "レベル", _level);
        //時間を取得
        TodayNow = DateTime.Now;
        writer.Write("年R", TodayNow.Year);
        writer.Write("月R", TodayNow.Month);
        writer.Write("日R", TodayNow.Day);

        writer.Commit();
    }
}
