using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TaskS : MonoBehaviour
{
    [SerializeField] Text _name;
    [SerializeField] Image _level;
    public GameManager _gm;
    public SpecialTask _specialTask;
    public TaskManager _taskManager;

    public string _taskName;
    public int _taskLevel;
    public bool _clear;
    public int point;
    public int _taskNumber;
    void Start()
    {
        _name.text = _taskName;
        _level.color = _taskManager._colors[_taskLevel];
    }
    public void AddPoint()
    {
        _gm._cityPoint += point + (int)(point * (_gm._rm._happyValue / 100));
        _gm._money += point;
    }
    public void RemoveTask()
    {
        _specialTask.TaskRemove(_taskNumber);
    }
    public void TaskDone()
    {
        _specialTask.TaskDone(_taskNumber);
    }
}
