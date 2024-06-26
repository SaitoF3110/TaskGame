using CI.QuickSave;
using CI.QuickSave.Core.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class SpecialTask : MonoBehaviour
{
    public GameManager _gm;
    public TaskManager _taskManager;
    //セーブ設定
    private protected QuickSaveSettings m_saveSettings;
    public int _taskCount = 0;

    [SerializeField] GameObject _taskPrefab;
    [SerializeField] GameObject _addObject;
    [SerializeField] GameObject _TaskMask;
    [SerializeField] ObjInstant _oi;

    [SerializeField] int _intarval = 100;





    //日常業務リスト
    //名前、難易度、達成済みか
    //trueは達成済み、falseは未達成
    public List<Tuple<string, int, bool>> _specialTask = new List<Tuple<string, int, bool>>();
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


    public void TaskDone(int num)
    {
        _specialTask[num] = new Tuple<string, int, bool>(_specialTask[num].Item1, _specialTask[num].Item2, true);
        _oi.SEInstantrate();
    }
    public void TaskDisplay()
    {
        _taskManager.Move(1);
        // parentObject のすべての子オブジェクトを削除
        foreach (Transform child in _TaskMask.transform)
        {
            GameObject.Destroy(child.gameObject);
        }


        for (int i = 0; i < _specialTask.Count; i++)
        {
            GameObject obj = Instantiate(_taskPrefab, new Vector3(658, _intarval * i + 450, 0), this.transform.rotation, _TaskMask.transform);
            TaskS task = obj.GetComponent<TaskS>();
            task._gm = _gm;
            task._taskManager = _taskManager;
            task._taskName = _specialTask[i].Item1;
            task._taskLevel = _specialTask[i].Item2;
            task._taskNumber = i;
            task.point = (_specialTask[i].Item2 + 1) * 20;
            task._specialTask = this.gameObject.GetComponent<SpecialTask>();

        }
        _addObject.transform.position = new Vector3(658, _intarval * _specialTask.Count + 450, 0);
        _taskManager.Move(3);
    }
    public void TaskRemove(int num)
    {
        _specialTask.Remove(_specialTask[num]);
        TaskDisplay();
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
        int y = 0;
        int m = 0;
        int d = 0;
        try
        {
            _taskCount = int.Parse(reader.Read<string>("特別業務総数"));
        }
        catch (QuickSaveException e)
        {
            _taskCount = 0;
        }
        for (int i = 0; i < _taskCount; i++)
        {
            string name = reader.Read<string>("S名前" + i);
            int difi = reader.Read<int>("S難易度" + i);
            bool done = reader.Read<bool>("S達成" + i);

            _specialTask.Add(new Tuple<string, int, bool>(name, difi, done));

        }
    }
    public int GetWeekCount(int y, int m, int d)
    {
        DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
        Calendar cal = dfi.Calendar;
        return cal.GetWeekOfYear(new DateTime(y, m, d), dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
    }

    /// <summary>
    /// データセーブ
    /// </summary>
    public void SaveUserData()
    {
        QuickSaveWriter writer = QuickSaveWriter.Create("SaveData", m_saveSettings);
        for (int i = 0; i < _specialTask.Count; i++)
        {
            writer.Write("S名前" + i, _specialTask[i].Item1);
            writer.Write("S難易度" + i, _specialTask[i].Item2);
            writer.Write("S達成" + i, _specialTask[i].Item3);
        }
        writer.Write("特別業務総数", _specialTask.Count);


        writer.Commit();
    }
    private void OnDisable()
    {
        SaveUserData();
    }
    private void OnApplicationQuit()
    {
        SaveUserData();
    }
}
