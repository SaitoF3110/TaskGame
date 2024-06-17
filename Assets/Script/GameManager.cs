using CI.QuickSave;
using CI.QuickSave.Core.Storage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class GameManager : MonoBehaviour
{
    public int _cityLevel = 1;
    public int _cityPoint = 0;
    public int _money = 0;

    [SerializeField] Text _levelText;
    [SerializeField] Text _pointText;
    [SerializeField] Text _moneyText;

    [SerializeField] Slider _expSlider;

    private protected QuickSaveSettings m_saveSettings;

    public int _nowExp = 10;
    public int _nextExp;

    public int _sliderMaxExp;
    public int _sliderExp;
    float _time;
    void Start()
    {
        Screen.SetResolution(270, 608, false);
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

        //データ読み込み
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
        _levelText.text = _cityLevel.ToString();
        _pointText.text = "都市ポイント：" + _cityPoint.ToString();
        _moneyText.text = "資金：" + YenConvert(_money) + "円";

        Exp();
    }
    public void SaveUserData()
    {
        // QuickSaveWriterのインスタンスを作成
        QuickSaveWriter writer = QuickSaveWriter.Create("SaveData", m_saveSettings);

        // データを書き込む
        writer.Write("都市ポイント", _cityPoint);
        writer.Write("資金", _money);
        writer.Write("経験値", _nowExp);

        // 変更を反映
        writer.Commit();
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

        try
        {
            // データを読み込む
            _cityPoint = reader.Read<int>("都市ポイント");
            _money = reader.Read<int>("資金");
            _nowExp = reader.Read<int>("経験値");
        }
        catch (QuickSaveException e)
        {
            Debug.Log("セーブデータが無いYO");
            _cityPoint = 0;
            _money = 0;
        }

    }
    string YenConvert(int money)
    {
        string ms = money.ToString();
        List<char> list = new List<char>();
        string ans = "";
        for(int i = ms.Length - 1;i >= 0; i--)
        {
            if(ms.Length - i - 1 == 4)
            {
                list.Add('万');
            }
            if (ms.Length - i - 1 == 8)
            {
                list.Add('億');
            }
            list.Add(ms[i]);
        }
        list.Reverse();
        ans = string.Join("", list.ToArray());
        return ans;
    }
    void Exp()
    {
        int lv = 0;
        int experience = _nowExp;
        int xp;
        while (true)
        {
            xp = (int)(Mathf.Pow(1.01f,lv) * 50);
            experience -= xp;
            if(experience < 0)
            {
                experience += xp;
                _nextExp = xp - experience;
                break;
            }
            lv++;
        }
        _sliderMaxExp = xp;
        _sliderExp = xp - _nextExp;
        _expSlider.maxValue = _sliderMaxExp;
        _expSlider.value = _sliderExp;
        _cityLevel = lv + 1;
        
    }
    public void AddPoints(int point)
    {
        _cityPoint += point;
        _money += point;
    }
    public void AddExp(int exp)
    {
        _nowExp += exp;
    }
}
