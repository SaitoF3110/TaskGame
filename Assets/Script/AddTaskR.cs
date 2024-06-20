using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddTaskR : MonoBehaviour
{
    [SerializeField] RepetTask _repetTask;
    [SerializeField] InputField _inputField;
    [SerializeField] Slider _slider;

    public void TaskAddButton()
    {
        _repetTask._repetTask.Add(new Tuple<string, int, int>(_inputField.text,(int)_slider.value,0));
    }
}
