using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CI.QuickSave;
using CI.QuickSave.Core.Storage;
using System;
using UnityEditor.PackageManager.Requests;
using static Unity.VisualScripting.Metadata;
using System.Globalization;

public class DailyTask : MonoBehaviour
{
    public GameManager _gm;
    public TaskManager _taskManager;
    //�Z�[�u�ݒ�
    private protected QuickSaveSettings m_saveSettings;
    public int _taskCount = 0;
    //���[�U�[��
    string userName;
    //�x�X�g�X�R�A
    int bestScore;

    [SerializeField] GameObject _taskPrefab;
    [SerializeField] GameObject _addObject;
    [SerializeField] GameObject _TaskMask;
    [SerializeField] ObjInstant _oi;

    [SerializeField] int _intarval = 100;

    bool _login = false;

    float _time;

    DateTime TodayNow;

    //����Ɩ����X�g
    //���O�A��Փx�A�B���ς݂��A�����i �f�C���[ �E�B�[�N���[  �}���X���[�j
    //true�͒B���ς݁Afalse�͖��B��
    public List<Tuple<string,int,bool,string>> _dailyTask = new List<Tuple<string,int,bool,string>>();
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

    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime;
        if (_time > 1)
        {
            SaveUserData();
            _time = 0;
        }
        
    }
    public void TaskDone(int num)
    {
        _dailyTask[num] = new Tuple<string, int, bool, string>(_dailyTask[num].Item1, _dailyTask[num].Item2, true, _dailyTask[num].Item4);
        _oi.SEInstantrate();
    }
    public void TaskDisplay()
    {

        // parentObject �̂��ׂĂ̎q�I�u�W�F�N�g���폜
        foreach (Transform child in _TaskMask.transform)
        {
            GameObject.Destroy(child.gameObject);
        }


        for (int i = 0;i < _dailyTask.Count; i++)
        {
            GameObject obj = Instantiate(_taskPrefab,new Vector3(132,_intarval * i + 450,0),this.transform.rotation,_TaskMask.transform);
            TaskD task = obj.GetComponent<TaskD>();
            task._gm = _gm;
            task._taskManager = _taskManager;
            task._taskName = _dailyTask[i].Item1;
            task._taskLevel = _dailyTask[i].Item2;
            task._cycleName.text = _dailyTask[i].Item4;
            task._taskNumber = i;
            task._dailyTask = this.gameObject.GetComponent<DailyTask>();
            task._done = _dailyTask[i].Item3;
            task.point = (_dailyTask[i].Item2 + 1) * 10;

        }
        _addObject.transform.position = new Vector3(132, _intarval * _dailyTask.Count + 450, 0);
    }
    public void TaskRemove(int num)
    {
        _dailyTask.Remove(_dailyTask[num]);
        TaskDisplay();
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
            _taskCount = int.Parse(reader.Read<string>("����Ɩ�����"));
            y = reader.Read<int>("�N");
            m = reader.Read<int>("��");
            d = reader.Read<int>("��");
        }
        catch (QuickSaveException e)
        {
            _taskCount = 0;
        }

        //���Ԃ��擾
        TodayNow = DateTime.Now;
        for (int i = 0; i < _taskCount; i++)
        {
            string name = reader.Read<string>("D���O" + i);
            int difi = reader.Read<int>("D��Փx" + i);
            bool done = reader.Read<bool>("D�B��" + i);
            string cycle = reader.Read<string>("D����" + i);
            string dateTimeString = reader.Read<string>("DateTime");
            DateTime time = DateTime.FromBinary(Convert.ToInt64(dateTimeString));
            //�f�C���[�Ȃǂ��A�����܂��������̏���
            if (cycle == "Monthly")
            {
                if (TodayNow.Year != y || TodayNow.Month != m)
                {
                    done = true;
                }
            }
            if (cycle == "Weekly")
            {
                if (GetWeekCount(y, m, d) < GetWeekCount(TodayNow.Year,TodayNow.Month,TodayNow.Day))
                {
                    done = true;
                }
            }
            if (cycle == "Daily")
            {
                if (TodayNow.Year != y || TodayNow.Month != m || TodayNow.Day != d)
                {
                    done = true;
                    _login = false;
                }
            }

            _dailyTask.Add(new Tuple<string, int, bool, string>(name, difi, done,cycle));

        }
    }
    public int GetWeekCount(int y,int m,int d)
    {
        DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
        Calendar cal = dfi.Calendar;
        return cal.GetWeekOfYear(new DateTime(y,m, d), dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
    }

    /// <summary>
    /// �f�[�^�Z�[�u
    /// </summary>
    public void SaveUserData()
    {
        QuickSaveWriter writer = QuickSaveWriter.Create("SaveData", m_saveSettings);
        for (int i = 0; i < _dailyTask.Count; i++)
        {
            writer.Write("D���O" + i, _dailyTask[i].Item1);
            writer.Write("D��Փx" + i, _dailyTask[i].Item2);
            writer.Write("D�B��" + i, _dailyTask[i].Item3);
            writer.Write("D����" + i, _dailyTask[i].Item4);
        }
        writer.Write("����Ɩ�����", _dailyTask.Count);
        //���Ԃ��擾
        TodayNow = DateTime.Now;
        writer.Write("�N", TodayNow.Year);
        writer.Write("��", TodayNow.Month);
        writer.Write("��", TodayNow.Day);
        writer.Write("���O�C��", _login);
        writer.Write("DateTime", TodayNow.ToBinary().ToString());

        writer.Commit();
    }
    void Resetyyyy()
    {
        QuickSaveWriter writer = QuickSaveWriter.Create("SaveData", m_saveSettings);
        writer.Write("����Ɩ�����",0);
        writer.Commit();
    }
}
