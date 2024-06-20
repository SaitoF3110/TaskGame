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
    //セーブ設定
    private protected QuickSaveSettings m_saveSettings;
    public int _taskCount = 0;
    //ユーザー名
    string userName;
    //ベストスコア
    int bestScore;

    [SerializeField] GameObject _taskPrefab;
    [SerializeField] GameObject _addObject;
    [SerializeField] GameObject _TaskMask;
    [SerializeField] ObjInstant _oi;

    [SerializeField] int _intarval = 100;

    bool _login = false;

    float _time;

    DateTime TodayNow;

    //日常業務リスト
    //名前、難易度、達成済みか、周期（ デイリー ウィークリー  マンスリー）
    //trueは達成済み、falseは未達成
    public List<Tuple<string,int,bool,string>> _dailyTask = new List<Tuple<string,int,bool,string>>();
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

        // parentObject のすべての子オブジェクトを削除
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
            _taskCount = int.Parse(reader.Read<string>("日常業務総数"));
            y = reader.Read<int>("年");
            m = reader.Read<int>("月");
            d = reader.Read<int>("日");
        }
        catch (QuickSaveException e)
        {
            _taskCount = 0;
        }

        //時間を取得
        TodayNow = DateTime.Now;
        for (int i = 0; i < _taskCount; i++)
        {
            string name = reader.Read<string>("D名前" + i);
            int difi = reader.Read<int>("D難易度" + i);
            bool done = reader.Read<bool>("D達成" + i);
            string cycle = reader.Read<string>("D周期" + i);
            string dateTimeString = reader.Read<string>("DateTime");
            DateTime time = DateTime.FromBinary(Convert.ToInt64(dateTimeString));
            //デイリーなどが、日をまたいだ時の処理
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
    /// データセーブ
    /// </summary>
    public void SaveUserData()
    {
        QuickSaveWriter writer = QuickSaveWriter.Create("SaveData", m_saveSettings);
        for (int i = 0; i < _dailyTask.Count; i++)
        {
            writer.Write("D名前" + i, _dailyTask[i].Item1);
            writer.Write("D難易度" + i, _dailyTask[i].Item2);
            writer.Write("D達成" + i, _dailyTask[i].Item3);
            writer.Write("D周期" + i, _dailyTask[i].Item4);
        }
        writer.Write("日常業務総数", _dailyTask.Count);
        //時間を取得
        TodayNow = DateTime.Now;
        writer.Write("年", TodayNow.Year);
        writer.Write("月", TodayNow.Month);
        writer.Write("日", TodayNow.Day);
        writer.Write("ログイン", _login);
        writer.Write("DateTime", TodayNow.ToBinary().ToString());

        writer.Commit();
    }
    void Resetyyyy()
    {
        QuickSaveWriter writer = QuickSaveWriter.Create("SaveData", m_saveSettings);
        writer.Write("日常業務総数",0);
        writer.Commit();
    }
}
