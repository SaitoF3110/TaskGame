using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Task : MonoBehaviour
{
    [SerializeField] Text _name;
    [SerializeField] Text _level;
    [SerializeField] GameObject _count;

    public string _taskName;
    public int _taskLevel;
    public TaskManager.Task _taskType;
    public int _clearCount;
    void Start()
    {
        _name.text = _taskName;
    }

}
