using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "����", menuName = "����")]
public class WardInfo : ScriptableObject
{
    public string _name;
    public int _populationH;//�Z��n��
    public int _populationC;//���ƒn��
    public int _populationI;//�H�ƒn��
    public string[] _sights;
}
