using CI.QuickSave;
using CI.QuickSave.Core.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class RepetTask : MonoBehaviour
{
    public GameManager _gm;
    public TaskManager _taskManager;
    //�Z�[�u�ݒ�
    private protected QuickSaveSettings m_saveSettings;
    public int _taskCount = 0;

    [SerializeField] GameObject _taskPrefab;
    [SerializeField] GameObject _addObject;
    [SerializeField] GameObject _TaskMask;
    [SerializeField] ObjInstant _oi;

    [SerializeField] int _intarval = 100;

    bool _login = false;

    float _time;

    DateTime TodayNow;

    //�J��Ԃ��Ɩ����X�g
    //���O�A��Փx�A�B����
    public List<Tuple<string, int,int>> _repetTask = new List<Tuple<string, int, int>>();
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
        TaskDisplay();
        _login = true;
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
        int count = _repetTask[num].Item3;
        count++;
        _repetTask[num] = new Tuple<string, int,int>(_repetTask[num].Item1, _repetTask[num].Item2, count);
        _oi.SEInstantrate();
    }
    public void TaskDisplay()
    {
        _taskManager.Move(1);
        // parentObject �̂��ׂĂ̎q�I�u�W�F�N�g���폜
        foreach (Transform child in _TaskMask.transform)
        {
            GameObject.Destroy(child.gameObject);
        }


        for (int i = 0; i < _repetTask.Count; i++)
        {
            GameObject obj = Instantiate(_taskPrefab, new Vector3(395, _intarval * i + 450, 0), this.transform.rotation, _TaskMask.transform);
            TaskR task = obj.GetComponent<TaskR>();
            task._gm = _gm;
            task._taskManager = _taskManager;
            task._taskName = _repetTask[i].Item1;
            task._taskLevel = _repetTask[i].Item2;
            task._taskNumber = i;
            task._repetTask = this.gameObject.GetComponent<RepetTask>();
            task._count = _repetTask[i].Item3;
            task.point = (_repetTask[i].Item2 + 1) * 10;

        }
        _addObject.transform.position = new Vector3(395, _intarval * _repetTask.Count + 450, 0);
        _taskManager.Move(2);
    }
    public void TaskRemove(int num)
    {
        _repetTask.Remove(_repetTask[num]);
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
        try
        {
            _taskCount = int.Parse(reader.Read<string>("�J�ԋƖ�����"));
            y = reader.Read<int>("�N");
            m = reader.Read<int>("��");
        }
        catch (QuickSaveException e)
        {
            _taskCount = 0;
            Debug.Log("�Ł[���Ȃ�");
        }
        //���Ԃ��擾
        TodayNow = DateTime.Now;

        for (int i = 0; i < _taskCount; i++)
        {
            string name = reader.Read<string>("R���O" + i);
            int difi = reader.Read<int>("R��Փx" + i);
            int done = reader.Read<int>("R��" + i);

            if (TodayNow.Year != y  || TodayNow.Month != m)
            {
                done = 0;
            }

            _repetTask.Add(new Tuple<string, int, int>(name, difi, done));

        }
    }
    /// <summary>
    /// �f�[�^�Z�[�u
    /// </summary>
    public void SaveUserData()
    {
        QuickSaveWriter writer = QuickSaveWriter.Create("SaveData", m_saveSettings);
        for (int i = 0; i < _repetTask.Count; i++)
        {
            writer.Write("R���O" + i, _repetTask[i].Item1);
            writer.Write("R��Փx" + i, _repetTask[i].Item2);
            writer.Write("R��" + i, _repetTask[i].Item3);
        }
        writer.Write("�J�ԋƖ�����", _repetTask.Count);
        //���Ԃ��擾
        TodayNow = DateTime.Now;
        writer.Write("�N", TodayNow.Year);
        writer.Write("��", TodayNow.Month);

        writer.Commit();
        Debug.Log("�Z�[�u����");
    }
}
