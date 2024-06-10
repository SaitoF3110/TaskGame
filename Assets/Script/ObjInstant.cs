using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjInstant : MonoBehaviour
{
    [SerializeField] GameObject _obj;
    public void SEInstantrate()
    {
        Instantiate( _obj );
    }
}
