using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AccidentManager : MonoBehaviour
{
    [SerializeField] string[] _accident;
    [SerializeField] string[] _ward;
    [SerializeField] ArmyManager _am;
    void Start()
    {
        
    }
    void Update()
    {

    }
    public void Accident(int num)
    {
        for (int i = 0;i < num;i++)
        {
            string line = _ward[Random.Range(0, _ward.Length)] + "‚Å" + _accident[Random.Range(0, _accident.Length)] + "‚ª”­¶I";
            _am._list.Add(new System.Tuple<string, int, bool>(line, Random.Range(1, 6) * 5, true));
        }
    }
}
