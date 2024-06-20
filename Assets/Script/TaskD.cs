using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskD : MonoBehaviour
{
    [SerializeField] Text _name;
    [SerializeField] Image _level;
    [SerializeField] GameObject _checkButton;
    public Text _cycleName;
    public GameManager _gm;
    public DailyTask _dailyTask;
    public TaskManager _taskManager;

    public string _taskName;
    public int _taskLevel;
    public bool _clear;
    public int point;
    public int _taskNumber;
    public bool _done;
    void Start()
    {
        _name.text = _taskName;
        _level.color = _taskManager._colors[_taskLevel];
        if (_done)
        {
            _checkButton.SetActive(false);
        }
    }
    public void AddPoint()
    {
        _gm._cityPoint += point;
        _gm._money += point;
    }
    public void RemoveTask()
    {
        _dailyTask.TaskRemove(_taskNumber);
    }
    public void TaskDone()
    {
        _dailyTask.TaskDone(_taskNumber);
    }
}
