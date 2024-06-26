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
    public float _happyValue;//���̒l�����{�[�i�X�Ń|�C���g�����炦��

    public int _population;

    private protected QuickSaveSettings m_saveSettings;

    void Awake()
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
        LoadUserData();
        
    }

    void Update()
    {
        //�K���x�v�Z���\��
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
        _valueText.text = "�K���x:" + _happyValue.ToString("F2") + "%";
    }

    public void AddMax()
    {
        _happyMax += 5;
    }
    public void LoadUserData()
    {
        //�t�@�C����������Ζ���
        if (FileAccess.Exists("SaveData", false) == false)
        {
            return;
        }

        // QuickSaveReader�̃C���X�^���X���쐬
        QuickSaveReader reader = QuickSaveReader.Create("SaveData", m_saveSettings);

        try
        {
            _happyMax = reader.Read<int>("�K���xMax");
            _happyValue = reader.Read<float>("�K���x");

        }
        catch (QuickSaveException e)
        {
            
        }

    }
    /// <summary>
    /// �f�[�^�Z�[�u
    /// </summary>
    public void SaveUserData()
    {
        QuickSaveWriter writer = QuickSaveWriter.Create("SaveData", m_saveSettings);

        writer.Write("�K���x", _happyValue);
        writer.Write("�K���xMax", _happyMax);

        writer.Commit();
    }
    private void OnApplicationQuit()
    {
        SaveUserData();
    }
}
