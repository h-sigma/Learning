using System.Collections;
using System.Collections.Generic;
using DOTS.System_Comparison;
using UnityEngine;

public class ClassicShipDescent : MonoBehaviour
{
    void Update()
    {
        Vector3 pos = transform.position;
        pos += Vector3.down * (GameManager.Instance.enemySpeed * Time.deltaTime);
        if (pos.y < GameManager.Instance.bottomBound)
            pos.y = GameManager.Instance.topBound;
        transform.position = pos;
    }
}