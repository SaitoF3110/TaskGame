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

        //�f�[�^�ǂݍ���
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
        _pointText.text = "�s�s�|�C���g�F" + _cityPoint.ToString();
        _moneyText.text = "�����F" + YenConvert(_money) + "�~";

        Exp();
    }
    public void SaveUserData()
    {
        // QuickSaveWriter�̃C���X�^���X���쐬
        QuickSaveWriter writer = QuickSaveWriter.Create("SaveData", m_saveSettings);

        // �f�[�^����������
        writer.Write("�s�s�|�C���g", _cityPoint);
        writer.Write("����", _money);
        writer.Write("�o���l", _nowExp);

        // �ύX�𔽉f
        writer.Commit();
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

        try
        {
            // �f�[�^��ǂݍ���
            _cityPoint = reader.Read<int>("�s�s�|�C���g");
            _money = reader.Read<int>("����");
            _nowExp = reader.Read<int>("�o���l");
        }
        catch (QuickSaveException e)
        {
            Debug.Log("�Z�[�u�f�[�^������YO");
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
                list.Add('��');
            }
            if (ms.Length - i - 1 == 8)
            {
                list.Add('��');
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
