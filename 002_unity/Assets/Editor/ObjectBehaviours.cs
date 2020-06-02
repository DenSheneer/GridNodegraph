using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBehaviours : MonoBehaviour
{
    [SerializeField]
    private string[] _behaviours;

    public string[] getBehaviours()
    {
        return _behaviours;
    }
}
