using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[ExecuteInEditMode]
public class ShowOff : MonoBehaviour
{
    public bool reset;
    public bool rotate;
    public Vector3 euler;

    private Quaternion _initialRotation;

    private void Start()
    {
        _initialRotation = transform.rotation;
    }
    
    public void Update()
    {
        if (reset)
        {
            reset = false;
            rotate = false;
            transform.rotation = _initialRotation;
        }
        if (rotate)
        { 
            transform.Rotate(euler);
        }
    }
}
