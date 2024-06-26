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
    public int _popH = 0;//�Z��l��
    public int _popC = 0;//���Ɛl��
    public int _popI = 0;//�H�Ɛl��

    bool _longPress = false;
    string _pressType;
    float _pressTime;
    int _count = 0;
    int _interval = 3;

    List<string> _sightList = new List<string>();

    float _totalPop;

    //�����������Ƃ��p
    int _popAddCount = 1;

    private protected QuickSaveSettings m_saveSettings;
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
        _percentage.text = "�B�����@" + pa.ToString("F2") + "%";
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
        // QuickSaveWriter�̃C���X�^���X���쐬
        QuickSaveWriter writer = QuickSaveWriter.Create("SaveData", m_saveSettings);

        // �f�[�^����������
        writer.Write(wi._name + "�Z��", _popH);
        writer.Write(wi._name + "����", _popC);
        writer.Write(wi._name + "�H��", _popI);

        

        // �ύX�𔽉f
        writer.Commit();
    }
    public void OnDisable()
    {
        SaveUserData(_wardInfo);
    }
    public void LoadUserData(WardInfo wi)
    {
        //�t�@�C����������Ζ���
        if (FileAccess.Exists("SaveData", false) == false)
        {
            return;
        }

        // QuickSaveReader�̃C���X�^���X���쐬
        QuickSaveReader reader = QuickSaveReader.Create("SaveData", m_saveSettings);

        try
        {
            // �f�[�^��ǂݍ���
            _popH = reader.Read<int>(wi._name + "�Z��");
            _popC = reader.Read<int>(wi._name + "����");
            _popI = reader.Read<int>(wi._name + "�H��");

            for(int i = 0;i < wi._sights.Length; i++)
            {
                //�J���\���(true)�������疼�����X�g�ɒǉ�
                if (reader.Read<bool>(wi._sights[i]))
                {
                    _sightList.Add(wi._sights[i].Trim());
                    //�X�y�[�X�@�ɂ��o�O�h�~
                }
            }
        }
        catch (QuickSaveException e)
        {
            Debug.Log("�Z�[�u�f�[�^������YO");
            _popH = 0;
            _popC = 0;
            _popI = 0;

            // QuickSaveWriter�̃C���X�^���X���쐬
            QuickSaveWriter writer = QuickSaveWriter.Create("SaveData", m_saveSettings);

            // �f�[�^����������
            for (int i = 0; i < wi._sights.Length; i++)
            {
                //�S�����X�g�ɒǉ�
                _sightList.Add(wi._sights[i]);
                writer.Write(wi._sights[i], true);
            }

            // �ύX�𔽉f
            writer.Commit();
        }

    }
    public void SightBuild()
    {
        //�|�C���g�A�o���l�̑���
        if (_gameManager._cityPoint >= 500 && _sightList.Count > 0)
        {
            // QuickSaveWriter�̃C���X�^���X���쐬
            QuickSaveWriter writer = QuickSaveWriter.Create("SaveData", m_saveSettings);
            writer.Write(_sightList[_wardDropdown.value], false);
            writer.Commit();
            
            //�폜
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
        //�����f�[�^�Z�[�u

        SaveUserData(_wardInfo);
        //�����ǂݍ���
        _sightList.Clear();//�������X�g�폜
        LoadUserData(wi);
        _wardText.text = wi._name;
        _wardInfo = wi;
        _wardDropdown.options.Clear();//�h���b�v�_�E���̃��X�g�폜
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
