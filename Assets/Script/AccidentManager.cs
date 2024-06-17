using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccidentManager : MonoBehaviour
{
    [SerializeField] string[] _accident;
    [SerializeField] string[] _ward;
    void Start()
    {
        
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Accident();
        }
    }
    void Accident()
    {
        Debug.Log(_ward[Random.Range(0, _ward.Length)] + "Ç≈" +_accident[Random.Range(0,_accident.Length)] + "Ç™î≠ê∂ÅI");
    }
}
