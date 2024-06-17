using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CI.QuickSave;
using CI.QuickSave.Core.Storage;
using System;

public class DailyTask : MonoBehaviour
{
    public GameManager _gm;
    //セーブ設定
    private protected QuickSaveSettings m_saveSettings;
    int _taskCount = 0;
    //ユーザー名
    string userName;
    //ベストスコア
    int bestScore;

    float _time;

    //日常業務リスト
    //名前、難易度、達成済みか
    List<Tuple<string,int,bool>> _dailyTask = new List<Tuple<string,int,bool>>();
    void Start()
    {
        // QuickSaveSettingsのインスタンスを作成
        m_saveSettings = new QuickSaveSettings();
        // 暗号化の方法 
        m_saveSettings.SecurityMode = SecurityMode.Aes;
        // Aesの暗号化キー
        m_saveSettings.Password = "Password";
        // 圧縮の方法
        m_saveSettings.CompressionMode = CompressionMode.Gzip;
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
    public void AddTask()
    {

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
        string num;
        try
        {
            _taskCount = int.Parse(reader.Read<string>("日常業務総数"));
        }
        catch (QuickSaveException e)
        {
            _taskCount = 0;
        }
        for (int i = 0; i < _taskCount; i++)
        {
            string name = (reader.Read<string>(i + "名前"));
            int difi = reader.Read<int>(i + "難易度");
            bool done = reader.Read<bool>(i + "達成");
            _dailyTask.Add(new Tuple<string, int, bool>(name, difi, done));

        }
        Debug.Log("ロード完了。");
    }

    /// <summary>
    /// データセーブ
    /// </summary>
    public void SaveUserData()
    {
        QuickSaveWriter writer = QuickSaveWriter.Create("SaveData", m_saveSettings);
        int a = 0;
        foreach (Tuple<string,int,bool> task in _dailyTask)
        {
            writer.Write(a.ToString() + "名前", task.Item1);
            writer.Write(a.ToString() + "難易度", task.Item2);
            writer.Write(a.ToString() + "回数", task.Item3);
            a++;
        }
        writer.Write("日常業務総数", _dailyTask.Count);
        writer.Commit();
        Debug.Log("セーブ完了");
    }
}
