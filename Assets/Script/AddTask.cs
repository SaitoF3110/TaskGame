using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddTask : MonoBehaviour
{
    [SerializeField] DailyTask _dailyTask;
    [SerializeField] InputField _inputField;
    [SerializeField] Slider _slider;
    [SerializeField] Slider _cycleSlider;
    void Start()
    {
        
    }

    public void TaskAddButton()
    {
        string cyc;
        if (_cycleSlider.value == 0)
        {
            cyc = "Daily";
        }
        else if (_cycleSlider.value == 1)
        {
            cyc = "Weekly";
        }
        else
        {
            cyc = "Monthly";
        }
        _dailyTask._dailyTask.Add(new Tuple<string, int, bool,string>(_inputField.text,(int)_slider.value,false,cyc));
    }
}
