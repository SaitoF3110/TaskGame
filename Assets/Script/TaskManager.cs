using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public Color[] _colors;

    [SerializeField] int _dailyX;
    [SerializeField] int _repetX;
    [SerializeField] int _specialX;
    [SerializeField] int _timeX;

    [SerializeField] GameObject _taskObj;
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void Move(int num)
    {
        if(num == 1)
        {
            _taskObj.transform.position = new Vector2(_dailyX + 550,_taskObj.transform.position.y);
        }
        else if (num == 2)
        {
            _taskObj.transform.position = new Vector2(_repetX + 550, _taskObj.transform.position.y);
        }
        else if (num == 3)
        {
            _taskObj.transform.position = new Vector2(_specialX + 550, _taskObj.transform.position.y);
        }
        else if (num == 4)
        {
            _taskObj.transform.position = new Vector2(_timeX + 550, _taskObj.transform.position.y);
        }
    }
    public enum Task
    {
        Daily,//ウィークリー、マンスリー含む
        Repet,
        Special,
    }
}
