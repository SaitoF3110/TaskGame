using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskD : MonoBehaviour
{
    [SerializeField] Text _name;
    [SerializeField] Image _level;
    public GameManager _gm;
    public DailyTask _dailyTask;
    public TaskManager _taskManager;

    public string _taskName;
    public int _taskLevel;
    public bool _clear;
    public int point;
    void Start()
    {
        _name.text = _taskName;
        _level.color = _taskManager._colors[_taskLevel];
    }
    public void AddPoint()
    {
        _gm._cityPoint += point;
        _gm._money += point;
    }
}
