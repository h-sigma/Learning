using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshData : MonoBehaviour
{
    void Start()
    {
        var mesh = GetComponent<MeshFilter>().mesh;
        List<Vector3> vertices = new List<Vector3>();
        mesh.GetVertices(vertices);
        foreach (var vertex in vertices)
        {
            Debug.Log(vertex);
        }
    }
}
