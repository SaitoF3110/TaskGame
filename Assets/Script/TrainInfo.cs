using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "路線情報", menuName = "路線情報")]
public class TrainInfo : ScriptableObject
{
    public string _name;
    public string[] _station;

}
