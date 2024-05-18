using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    [SerializeField] Color _levelL;
    [SerializeField] Color _levelM;
    [SerializeField] Color _levelH;
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public enum Task
    {
        Daily,//ウィークリー、マンスリー含む
        Repet,
        Special,
    }
}
