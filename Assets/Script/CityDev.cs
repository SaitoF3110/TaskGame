using CI.QuickSave;
using CI.QuickSave.Core.Storage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CityDev : MonoBehaviour
{
    public WardInfo _wardInfo;
    public GameManager _gameManager;
    [SerializeField] Text _wardText;
    [SerializeField] Dropdown _wardDropdown;
    [SerializeField] Text _popHText;
    [SerializeField] Text _popCText;
    [SerializeField] Text _popIText;
    [SerializeField] Text _percentage;
    [SerializeField] Slider _gage;
    [SerializeField] Text _ddText;
    [SerializeField] ResouceManager _resouceManager;

    [SerializeField] GameObject _addSE;
    [SerializeField] GameObject _failSE;

    [SerializeField] Text _label;
    public int _popH = 0;//住宅人口
    public int _popC = 0;//商業人口
    public int _popI = 0;//工業人口

    bool _longPress = false;
    string _pressType;
    float _pressTime;
    int _count = 0;
    int _interval = 3;

    List<string> _sightList = new List<string>();

    float _totalPop;

    //長押ししたとき用
    int _popAddCount = 1;

    private protected QuickSaveSettings m_saveSettings;
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

        LoadUserData(_wardInfo);
        WardChange(_wardInfo);
    }
    void Update()
    {
        _popHText.text = _popH + "/" + _wardInfo._populationH;
        _popCText.text = _popC + "/" + _wardInfo._populationC;
        _popIText.text = _popI + "/" + _wardInfo._populationI;

        _totalPop = _wardInfo._populationH + _wardInfo._populationC + _wardInfo._populationI;
        float nowPop = _popH + _popC + _popI;
        float ue = _wardInfo._sights.Length - _sightList.Count;
        float shita = _wardInfo._sights.Length;
        float pa = (float)(((nowPop / _totalPop) * 100) * 0.8) + (float)(((ue / shita) * 100) * 0.2f);
        _percentage.text = "達成率　" + pa.ToString("F2") + "%";
        _gage.value = pa;

        if (Input.GetKeyDown(KeyCode.S))
        {
            QuickSaveWriter writer = QuickSaveWriter.Create("SaveData", m_saveSettings);
            for (int i = 0; i < _wardInfo._sights.Length; i++)
            {
                writer.Write(_wardInfo._sights[i], true);
            }
            writer.Commit();
        }
    }
    private void FixedUpdate()
    {
        if (_longPress)
        {
            _pressTime += Time.deltaTime;
        }
        if (_pressTime > 0.6f)
        {
            _count++;
            if (_count % _interval == 0)
            {
                PopAdd(_pressType);
            }
        }
        if (_pressTime > 10)
        {
            _interval = 1;
        }
        _popAddCount = (int)_pressTime + 1;
    }
    public void SaveUserData(WardInfo wi)
    {
        // QuickSaveWriterのインスタンスを作成
        QuickSaveWriter writer = QuickSaveWriter.Create("SaveData", m_saveSettings);

        // データを書き込む
        writer.Write(wi._name + "住宅", _popH);
        writer.Write(wi._name + "商業", _popC);
        writer.Write(wi._name + "工業", _popI);

        

        // 変更を反映
        writer.Commit();
    }
    public void OnDisable()
    {
        SaveUserData(_wardInfo);
    }
    public void LoadUserData(WardInfo wi)
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
            _popH = reader.Read<int>(wi._name + "住宅");
            _popC = reader.Read<int>(wi._name + "商業");
            _popI = reader.Read<int>(wi._name + "工業");

            for(int i = 0;i < wi._sights.Length; i++)
            {
                //開発可能状態(true)だったら名所リストに追加
                if (reader.Read<bool>(wi._sights[i]))
                {
                    _sightList.Add(wi._sights[i].Trim());
                    //スペース　によるバグ防止
                }
            }
        }
        catch (QuickSaveException e)
        {
            Debug.Log("セーブデータが無いYO");
            _popH = 0;
            _popC = 0;
            _popI = 0;

            // QuickSaveWriterのインスタンスを作成
            QuickSaveWriter writer = QuickSaveWriter.Create("SaveData", m_saveSettings);

            // データを書き込む
            for (int i = 0; i < wi._sights.Length; i++)
            {
                //全部リストに追加
                _sightList.Add(wi._sights[i]);
                writer.Write(wi._sights[i], true);
            }

            // 変更を反映
            writer.Commit();
        }

    }
    public void SightBuild()
    {
        //ポイント、経験値の操作
        if (_gameManager._cityPoint >= 500 && _sightList.Count > 0)
        {
            // QuickSaveWriterのインスタンスを作成
            QuickSaveWriter writer = QuickSaveWriter.Create("SaveData", m_saveSettings);
            writer.Write(_sightList[_wardDropdown.value], false);
            writer.Commit();
            
            //削除
            _sightList.Remove(_sightList[_wardDropdown.value]);

            _resouceManager.AddMax();
            _gameManager._cityPoint -= 500;
            _gameManager.AddExp(1000);
            Instantiate(_addSE);
            _wardDropdown.value = 0;
            WardChange(_wardInfo);
        }
        else
        {
            Instantiate(_failSE);
        }
    }
    public void WardChange(WardInfo wi)
    {
        //↓↓データセーブ

        SaveUserData(_wardInfo);
        //↓↓読み込み
        _sightList.Clear();//名所リスト削除
        LoadUserData(wi);
        _wardText.text = wi._name;
        _wardInfo = wi;
        _wardDropdown.options.Clear();//ドロップダウンのリスト削除
        for(int i = 0;i < _sightList.Count; i++)
        {
            _wardDropdown.options.Add(new Dropdown.OptionData { text = _sightList[i] });
        }
        _label.text = "";
        if (_sightList.Count > 0)
        {
            _ddText.text = _sightList[0];
        }

    }
    public void PopAdd(string type)
    {
        if(_gameManager._cityPoint >= _popAddCount)
        {
            if (type == "H")
            {
                _popH+= _popAddCount;
                _gameManager._cityPoint -= _popAddCount;
            }
            if (type == "C")
            {
                _popC += _popAddCount;
                _gameManager._cityPoint -= _popAddCount;
            }
            if (type == "I")
            {
                _popI += _popAddCount;
                _gameManager._cityPoint -= _popAddCount;
            }
            _gameManager.AddExp(_popAddCount);
            Instantiate(_addSE);
        }
        else
        {
            Instantiate(_failSE);
        }
    }
    public void Press(string type)
    {
        _pressType = type;
        _longPress = true;
        PopAdd(_pressType);
    }
    public void UP()
    {
        _longPress = false;
        _pressTime = 0;
        _interval = 3;
        _popAddCount = 1;
    }
}
