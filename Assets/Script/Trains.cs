using CI.QuickSave;
using CI.QuickSave.Core.Storage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Trains : MonoBehaviour
{
    [SerializeField] TrainInfo _trainInfo;
    [SerializeField] Slider _slider;
    [SerializeField] Text _text;
    public List<string> _stations = new List<string>();
    //セーブ設定
    private protected QuickSaveSettings m_saveSettings;
    void Start()
    {

        TrainRate();
    }
    private void OnEnable()
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

        TrainRate();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void TrainRate()
    {
        _stations.Clear();
        //ファイルが無ければ無視
        if (FileAccess.Exists("SaveData", false) == false)
        {
            return;
        }
        // QuickSaveReaderのインスタンスを作成
        QuickSaveReader reader = QuickSaveReader.Create("SaveData", m_saveSettings);

        for (int i = 0;i < _trainInfo._station.Length;i++)
        {
            bool isStation = true;
            try
            {
                isStation = reader.Read<bool>(_trainInfo._station[i].Trim());
            }
            catch (QuickSaveException e)
            {
                Debug.Log("でーたなし");
            }
            if (!isStation)
            {
                _stations.Add(_trainInfo._station[i]);
            }
        }
        _slider.maxValue = _trainInfo._station.Length;
        _slider.value = _stations.Count;
        _text.text = _stations.Count + "/" + _trainInfo._station.Length;
    }
}
