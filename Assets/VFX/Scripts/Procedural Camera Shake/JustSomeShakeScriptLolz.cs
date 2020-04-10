using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustSomeShakeScriptLolz : MonoBehaviour
{
    public ShakeTransformEventData data;
    public void OnGUI()
    {
        if (GUILayout.Button("Shake"))
        {
            FindObjectOfType<ShakeTransform>().AddShakeEvent(data);
        }
    }
}