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
    //�Z�[�u�ݒ�
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

        Bonus(_gameManager._cityLevel);
    }

    // Update is called once per frame
    void Update()
    {
        //�{�[�i�X�����v�Z
        _bounus = Bonus(_gameManager._cityLevel);
        _bounsText.text = "���݂̃{�[�i�X" + _bounus.ToString("F2") + "%";

        if (_onWork)
        {
            _time += Time.deltaTime;
        }

        //�l���|�C���g�̌v�Z
        _timeText.text = TimeConbert((int)_time);
        int point = (int)(_time / 600);
        _lastPoint = (int)(point + point * (_bounus / 100));
        _lastPointText.text = "���݂̒~�σ|�C���g�F" + point + "\n�ŏI�|�C���g:" + _lastPoint.ToString();
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
            y = reader.Read<int>("�N");
            m = reader.Read<int>("��");
            d = reader.Read<int>("��");
        }
        catch (QuickSaveException e)
        {
            _taskCount = 0;
            Debug.Log("�Ł[���Ȃ�");
        }
        //���Ԃ��擾
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
        writer.Write("�~�ώ���",_time);
        //���Ԃ��擾
        TodayNow = DateTime.Now;
        writer.Write("�N", TodayNow.Year);
        writer.Write("��", TodayNow.Month);
        writer.Write("��", TodayNow.Day);
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
