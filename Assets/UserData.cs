using UnityEngine;
using CI.QuickSave;
using CI.QuickSave.Core.Storage;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;

public class UserData : MonoBehaviour
{
    //ユーザー名
    string userName;
    private protected string a = "Wow";
    //ベストスコア
    int bestScore;
    [SerializeField] GameObject _taskM;
    float _time = 0;
    //セーブ設定
    private protected QuickSaveSettings m_saveSettings;

    public void Start()
    {
        // QuickSaveSettingsのインスタンスを作成
        m_saveSettings = new QuickSaveSettings();
        // 暗号化の方法 
        m_saveSettings.SecurityMode = SecurityMode.Aes;
        // Aesの暗号化キー
        m_saveSettings.Password = "Password";
        // 圧縮の方法
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
            Debug.Log("セーブしました");
            _time = 0;
        }
    }

    /// <summary>
    /// セーブデータ読み込み
    /// </summary>
    public void LoadUserData()
    {
        //ファイルが無ければ無視
        if (FileAccess.Exists("SaveData", false) == false)
        {
            return;
        }

        // QuickSaveReaderのインスタンスを作成
        QuickSaveReader reader = QuickSaveReader.Create("SaveData", m_saveSettings);

        // データを読み込む
        userName = reader.Read<string>("UserName");
        bestScore = reader.Read<int>("BestScore");
    }

    /// <summary>
    /// データセーブ
    /// </summary>
    public void SaveUserData()
    {
        Debug.Log("セーブデータ保存先:" + Application.persistentDataPath);

        // QuickSaveWriterのインスタンスを作成
        QuickSaveWriter writer = QuickSaveWriter.Create("SaveData", m_saveSettings);

        // データを書き込む
        writer.Write(a, userName);
        writer.Write("BestScore", bestScore);

        // 変更を反映
        writer.Commit();
    }

    public void SaveCharactorData()
    {
        QuickSaveWriter writer = QuickSaveWriter.Create("SaveData", m_saveSettings);

        //キャラクター名のキーにレベル、強化、信頼度、武器強化の四つを保存
        for (int i = 0; i < 1; i++)//全キャラの分ループ
        {

        }
    }
    //仮想メソッド↓↓
    public virtual void StartOver()//スタート時に呼び出される。
    {

    }
    public virtual void Save()
    {
        QuickSaveWriter writer = QuickSaveWriter.Create("SaveData", m_saveSettings);
        writer.Write("レベル", 15);
        writer.Commit();
    }
    public virtual void Load()
    {
        //ファイルが無ければ無視
        if (FileAccess.Exists("SaveData", false) == false)
        {
            return;
        }

        // QuickSaveReaderのインスタンスを作成
        QuickSaveReader reader = QuickSaveReader.Create("SaveData", m_saveSettings);
        string a;
        // データを読み込む
        a = reader.Read<string>("レベル");
    }
    public void TestSave()
    {
        QuickSaveWriter writer = QuickSaveWriter.Create("SaveData", m_saveSettings);
        int a = 0;
        //writer.Write("日常業務総数", dailyTask._dailyDic.Count);
        writer.Commit();
        Debug.Log("セーブ完了");
    }
    public void TestLoad()
    {
        //ファイルが無ければ無視
        if (FileAccess.Exists("SaveData", false) == false)
        {
            return;
        }

        // QuickSaveReaderのインスタンスを作成
        QuickSaveReader reader = QuickSaveReader.Create("SaveData", m_saveSettings);
        string num;
        a = reader.Read<string>("日常業務総数");
        for (int i = 0; i < int.Parse(a); i++)
        {
            string name = (reader.Read<string>(i + "名前"));
            int difi = int.Parse(reader.Read<string>(i + "難易度"));
            int time = int.Parse(reader.Read<string>(i + "回数"));
            //dailyTask._dailyDic.Add((reader.Read<string>(i + "名前"), 
            //    int.Parse(reader.Read<string>(i + "難易度"))), 
            //    int.Parse(reader.Read<string>(i + "回数")));
            Debug.Log(reader.Read<string>(i + "名前"));
        }
        Debug.Log("ロード完了。ほかに何も表示されなければ失敗");
    }
}

