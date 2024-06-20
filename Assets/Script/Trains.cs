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
    //�Z�[�u�ݒ�
    private protected QuickSaveSettings m_saveSettings;
    void Start()
    {

        TrainRate();
    }
    private void OnEnable()
    {
        // QuickSaveSettings�̃C���X�^���X���쐬
        m_saveSettings = new QuickSaveSettings();
        // �Í����̕��@ 
        m_saveSettings.SecurityMode = SecurityMode.Aes;
        // Aes�̈Í����L�[
        m_saveSettings.Password = "Password";
        // ���k�̕��@
        m_saveSettings.CompressionMode = CompressionMode.Gzip;
        // �f�[�^�̕ۑ����Application.dataPath�ɕύX
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
        //�t�@�C����������Ζ���
        if (FileAccess.Exists("SaveData", false) == false)
        {
            return;
        }
        // QuickSaveReader�̃C���X�^���X���쐬
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
                Debug.Log("�Ł[���Ȃ�");
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
