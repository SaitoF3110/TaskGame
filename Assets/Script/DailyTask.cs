using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CI.QuickSave;
using CI.QuickSave.Core.Storage;
using System;

public class DailyTask : MonoBehaviour
{
    public GameManager _gm;
    //�Z�[�u�ݒ�
    private protected QuickSaveSettings m_saveSettings;
    int _taskCount = 0;
    //���[�U�[��
    string userName;
    //�x�X�g�X�R�A
    int bestScore;
    
    //����Ɩ����X�g
    //���O�A��Փx�A�B���ς݂�
    List<Tuple<string,int,int>> _dailyTask = new List<Tuple<string,int,int>>();
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
        LoadUserData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddTask()
    {

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
        string num;
        _taskCount = int.Parse(reader.Read<string>("����Ɩ�����"));
        for (int i = 0; i < _taskCount; i++)
        {
            string name = (reader.Read<string>(i + "���O"));
            int difi = int.Parse(reader.Read<string>(i + "��Փx"));
            int done = int.Parse(reader.Read<string>(i + "�B��"));
            _dailyTask.Add(new Tuple<string, int, int>(name, difi, done));

        }
        Debug.Log("���[�h�����B�ق��ɉ����\������Ȃ���Ύ��s");
    }

    /// <summary>
    /// �f�[�^�Z�[�u
    /// </summary>
    public void SaveUserData()
    {
        QuickSaveWriter writer = QuickSaveWriter.Create("SaveData", m_saveSettings);
        int a = 0;
        foreach (Tuple<string,int,int> task in _dailyTask)
        {
            writer.Write(a.ToString() + "���O", task.Item1);
            writer.Write(a.ToString() + "��Փx", task.Item2);
            writer.Write(a.ToString() + "��", task.Item3);
            a++;
        }
        writer.Write("����Ɩ�����", _dailyTask.Count);
        writer.Commit();
        Debug.Log("�Z�[�u����");
    }
}
