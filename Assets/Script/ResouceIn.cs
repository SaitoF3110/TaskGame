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
        _text.text = "���Y��" + _resouce.ToString("F0") + _unit + "/��";
        _text2.text = _exp.ToString("F0") + "point";
        _needsText.text = "�K�v��" + _needsSum.ToString() + _unit +"/��";

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
        //�t�@�C����������Ζ���
        if (FileAccess.Exists("SaveData", false) == false)
        {
            return;
        }

        // QuickSaveReader�̃C���X�^���X���쐬
        QuickSaveReader reader = QuickSaveReader.Create("SaveData", m_saveSettings);
        int y = 0;
        int m = 0;
        int d = 0;
        try
        {
            _level = int.Parse(reader.Read<string>(_name + "���x��"));
            y = reader.Read<int>("�NR");
            m = reader.Read<int>("��R");
            d = reader.Read<int>("��R");
        }
        catch (QuickSaveException e)
        {
            _level = 1;
        }
        //���Ԃ��擾
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
    /// �f�[�^�Z�[�u
    /// </summary>
    public void SaveUserData()
    {
        QuickSaveWriter writer = QuickSaveWriter.Create("SaveData", m_saveSettings);

        writer.Write(_name + "���x��", _level);
        //���Ԃ��擾
        TodayNow = DateTime.Now;
        writer.Write("�NR", TodayNow.Year);
        writer.Write("��R", TodayNow.Month);
        writer.Write("��R", TodayNow.Day);

        writer.Commit();
    }
}
