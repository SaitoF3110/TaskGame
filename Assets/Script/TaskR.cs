using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskR : MonoBehaviour
{
    [SerializeField] Text _name;
    [SerializeField] Image _level;
    [SerializeField] Text _countText;
    public Text _cycleName;
    public GameManager _gm;
    public RepetTask _repetTask;
    public TaskManager _taskManager;

    public string _taskName;
    public int _taskLevel;
    public bool _clear;
    public int point;
    public int _taskNumber;
    public int _count;
    void Start()
    {
        _name.text = _taskName;
        _level.color = _taskManager._colors[_taskLevel];
    }
    private void Update()
    {
        _countText.text = _count.ToString("000");
    }
    public void AddPoint()
    {
        _gm._cityPoint += point + (int)(point * (_gm._rm._happyValue / 100));
        _gm._money += point;
    }
    public void RemoveTask()
    {
        _repetTask.TaskRemove(_taskNumber);
    }
    public void TaskDone()
    {
        _repetTask.TaskDone(_taskNumber);
        _count++;
    }
}
