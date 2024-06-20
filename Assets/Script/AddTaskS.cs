using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddTaskS : MonoBehaviour
{
    [SerializeField] SpecialTask _specialTask;
    [SerializeField] InputField _inputField;
    [SerializeField] Slider _slider;

    public void TaskAddButton()
    {
        _specialTask._specialTask.Add(new Tuple<string, int, bool>(_inputField.text, (int)_slider.value, true));
    }
}
