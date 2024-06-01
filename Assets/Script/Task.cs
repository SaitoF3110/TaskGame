using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Task : MonoBehaviour
{
    [SerializeField] Text _name;
    [SerializeField] Image _level;
    [SerializeField] GameObject _count;
    public GameManager _gm;

    public string _taskName;
    public int _taskLevel;
    public TaskManager.Task _taskType;
    public int _clearCount;
    public int num;
    void Start()
    {
        _name.text = _taskName;
        
    }
    public void AddPoint()
    {
        _gm._cityPoint += num;
        _gm._money += num;
    }
}
