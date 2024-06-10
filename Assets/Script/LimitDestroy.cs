using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitDestroy : MonoBehaviour
{
    [SerializeField] float _limitTime;
    float _time = 0;
    void Update()
    {
        _time += Time.deltaTime;
        if(_time > _limitTime)
        {
            Destroy(gameObject);
        }
    }
}
