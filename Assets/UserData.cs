using UnityEngine;
using CI.QuickSave;
using CI.QuickSave.Core.Storage;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;

public class UserData : MonoBehaviour
{
    //���[�U�[��
    string userName;
    private protected string a = "Wow";
    //�x�X�g�X�R�A
    int bestScore;
    [SerializeField] GameObject _taskM;
    float _time = 0;
    //�Z�[�u�ݒ�
    private protected QuickSaveSettings m_saveSettings;

    public void Start()
    {
        // QuickSaveSettings�̃C���X�^���X���쐬
        m_saveSettings = new QuickSaveSettings();
        // �Í����̕��@ 
        m_saveSettings.SecurityMode = SecurityMode.Aes;
        // Aes�̈Í����L�[
        m_saveSettings.Password = "Password";
        // ���k�̕��@
        m_saveSettings.CompressionMode = CompressionMode.Gzip;
        StartOver();
        Load();
    }
    public void Update()
    {
        _time += Time.deltaTime;
        if (_time > 5)
        {
            Save();
            Debug.Log("�Z�[�u���܂���");
            _time = 0;
        }
    }

    /// <summary>
    /// �Z�[�u�f�[�^�ǂݍ���
    /// </summary>
    public void LoadUserData()
    {
        //�t�@�C����������Ζ���
        if (FileAccess.Exists("SaveData", false) == false)
        {
            return;
        }

        // QuickSaveReader�̃C���X�^���X���쐬
        QuickSaveReader reader = QuickSaveReader.Create("SaveData", m_saveSettings);

        // �f�[�^��ǂݍ���
        userName = reader.Read<string>("UserName");
        bestScore = reader.Read<int>("BestScore");
    }

    /// <summary>
    /// �f�[�^�Z�[�u
    /// </summary>
    public void SaveUserData()
    {
        Debug.Log("�Z�[�u�f�[�^�ۑ���:" + Application.persistentDataPath);

        // QuickSaveWriter�̃C���X�^���X���쐬
        QuickSaveWriter writer = QuickSaveWriter.Create("SaveData", m_saveSettings);

        // �f�[�^����������
        writer.Write(a, userName);
        writer.Write("BestScore", bestScore);

        // �ύX�𔽉f
        writer.Commit();
    }

    public void SaveCharactorData()
    {
        QuickSaveWriter writer = QuickSaveWriter.Create("SaveData", m_saveSettings);

        //�L�����N�^�[���̃L�[�Ƀ��x���A�����A�M���x�A���틭���̎l��ۑ�
        for (int i = 0; i < 1; i++)//�S�L�����̕����[�v
        {

        }
    }
    //���z���\�b�h����
    public virtual void StartOver()//�X�^�[�g���ɌĂяo�����B
    {

    }
    public virtual void Save()
    {
        QuickSaveWriter writer = QuickSaveWriter.Create("SaveData", m_saveSettings);
        writer.Write("���x��", 15);
        writer.Commit();
    }
    public virtual void Load()
    {
        //�t�@�C����������Ζ���
        if (FileAccess.Exists("SaveData", false) == false)
        {
            return;
        }

        // QuickSaveReader�̃C���X�^���X���쐬
        QuickSaveReader reader = QuickSaveReader.Create("SaveData", m_saveSettings);
        string a;
        // �f�[�^��ǂݍ���
        a = reader.Read<string>("���x��");
    }
    public void TestSave()
    {
        QuickSaveWriter writer = QuickSaveWriter.Create("SaveData", m_saveSettings);
        int a = 0;
        //writer.Write("����Ɩ�����", dailyTask._dailyDic.Count);
        writer.Commit();
        Debug.Log("�Z�[�u����");
    }
    public void TestLoad()
    {
        //�t�@�C����������Ζ���
        if (FileAccess.Exists("SaveData", false) == false)
        {
            return;
        }

        // QuickSaveReader�̃C���X�^���X���쐬
        QuickSaveReader reader = QuickSaveReader.Create("SaveData", m_saveSettings);
        string num;
        a = reader.Read<string>("����Ɩ�����");
        for (int i = 0; i < int.Parse(a); i++)
        {
            string name = (reader.Read<string>(i + "���O"));
            int difi = int.Parse(reader.Read<string>(i + "��Փx"));
            int time = int.Parse(reader.Read<string>(i + "��"));
            //dailyTask._dailyDic.Add((reader.Read<string>(i + "���O"), 
            //    int.Parse(reader.Read<string>(i + "��Փx"))), 
            //    int.Parse(reader.Read<string>(i + "��")));
            Debug.Log(reader.Read<string>(i + "���O"));
        }
        Debug.Log("���[�h�����B�ق��ɉ����\������Ȃ���Ύ��s");
    }
}

