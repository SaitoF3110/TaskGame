using CI.QuickSave;
using CI.QuickSave.Core.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ArmyManager : MonoBehaviour
{
    public List<Tuple<string,int,bool>> _list = new List<Tuple<string,int,bool>>();
    [SerializeField] AccidentManager _accidentM;
    [SerializeField] ResouceManager _resouceM;
    [SerializeField] GameManager _gameManager;

    public int _accidentCount;

    DateTime TodayNow;

    [SerializeField] GameObject _itemPrefab;
    [SerializeField] GameObject _TaskMask;
    public int _intarval = -100;

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
        // �f�[�^�̕ۑ����Application.dataPath�ɕύX
        QuickSaveGlobalSettings.StorageLocation = Application.temporaryCachePath;
        LoadUserData();
        ItemDisplay();
    }
    public void Done(int num)
    {
        if (_gameManager._cityPoint >= _list[num].Item2)
        {
            _gameManager._cityPoint -= _list[num].Item2;
            _resouceM._happyValue += 0.1f;

            _list[num] = new Tuple<string, int, bool>(_list[num].Item1, _list[num].Item2, false);
            ItemDisplay();
        }
    }
    public void ItemDisplay()
    {
        // parentObject �̂��ׂĂ̎q�I�u�W�F�N�g���폜
        foreach (Transform child in _TaskMask.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        int num = 0;
        for (int i = 0; i < _list.Count; i++)
        {
            if (_list[i].Item3)
            {
                GameObject obj = Instantiate(_itemPrefab, new Vector3(135, _intarval * (i - num) + 400, 0), this.transform.rotation, _TaskMask.transform);
                AccidentItem acc = obj.GetComponent<AccidentItem>();
                acc._itemCount = i;
                acc._name = _list[i].Item1;
                acc._point = _list[i].Item2;
                acc._am = this.GetComponent<ArmyManager>();
            }
            else
            {
                num++;
            }

        }
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
            _accidentCount = reader.Read<int>("������");
            y = reader.Read<int>("�NA");
            m = reader.Read<int>("��A");
            d = reader.Read<int>("��A");
        }
        catch (QuickSaveException e)
        {
            _accidentCount = 0;
        }
        for (int i = 0; i < _accidentCount; i++)
        {
            _list.Add(new Tuple<string, int, bool>(reader.Read<string>("��������" + i), reader.Read<int>("�����|�C���g" + i), reader.Read<bool>("��������" + i)));

        }
        //���Ԃ��擾
        TodayNow = DateTime.Now;
        if (TodayNow.Year != y || TodayNow.Month != m || TodayNow.Day != d)
        {
            for (int i = 0; i < _accidentCount; i++)
            {
                if (_list[i].Item3)
                {
                    _resouceM._happyValue -= 0.1f;
                }
            }
            _list.Clear();
            _accidentM.Accident(UnityEngine.Random.Range(4, 10));
        }
        else
        {

        }

    }
    /// <summary>
    /// �f�[�^�Z�[�u
    /// </summary>
    public void SaveUserData()
    {
        QuickSaveWriter writer = QuickSaveWriter.Create("SaveData", m_saveSettings);

        writer.Write("������", _list.Count);
        for (int i = 0;i < _list.Count;i++)
        {
            writer.Write("��������" + i, _list[i].Item1);
            writer.Write("�����|�C���g" + i, _list[i].Item2);
            writer.Write("��������" + i, _list[i].Item3);
        }
        //���Ԃ��擾
        TodayNow = DateTime.Now;
        writer.Write("�NA",TodayNow.Year);
        writer.Write("��A", TodayNow.Month);
        writer.Write("��A", TodayNow.Day);

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
