using CI.QuickSave;
using CI.QuickSave.Core.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;
using UnityEngine.UI;

public class ResouceManager : MonoBehaviour
{
    [SerializeField] Slider _slider;
    [SerializeField] Text _maxText;
    [SerializeField] Text _valueText;

    public int _happyMax = 10;
    public float _happyValue;//この値％分ボーナスでポイントがもらえる

    public int _population;

    private protected QuickSaveSettings m_saveSettings;

    void Awake()
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

    void Update()
    {
        //幸福度計算＆表示
        if (_happyValue > _happyMax)
        {
            _happyValue = _happyMax;
        }
        if(_happyValue < 0)
        {
            _happyValue = 0;
        }
        _slider.maxValue = _happyMax;
        _slider.value = _happyValue;
        _maxText.text = "Max:" + _happyMax;
        _valueText.text = "幸福度:" + _happyValue.ToString("F2") + "%";
    }

    public void AddMax()
    {
        _happyMax += 5;
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

        try
        {
            _happyMax = reader.Read<int>("幸福度Max");
            _happyValue = reader.Read<float>("幸福度");

        }
        catch (QuickSaveException e)
        {
            
        }

    }
    /// <summary>
    /// データセーブ
    /// </summary>
    public void SaveUserData()
    {
        QuickSaveWriter writer = QuickSaveWriter.Create("SaveData", m_saveSettings);

        writer.Write("幸福度", _happyValue);
        writer.Write("幸福度Max", _happyMax);

        writer.Commit();
    }
    private void OnApplicationQuit()
    {
        SaveUserData();
    }
}
