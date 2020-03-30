using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    private Vector3[] vertices;
    private int[] triangles;

    public int xSize;
    public int zSize;

    public float terrainRandomness = 20.0f;

    public float minHeight = 0.0f;
    public float maxHeight = 10.0f;

    private Mesh mesh;
    
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        
        CreateMesh();
        UpdateMesh();
    }

    private void CreateMesh()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        float subtractOpMovingOutsideLoop = maxHeight - minHeight;

        for (int z = 0, i = 0; z <= zSize; z++)
        {
            for (var x = 0; x <= xSize; x++)
            {
                float y = Mathf.PerlinNoise(x / (xSize * terrainRandomness), z / (zSize * terrainRandomness));
                y = minHeight + (subtractOpMovingOutsideLoop) * y;
                vertices[i] = new Vector3(x, y, z);
                i++;
            }
        }

        triangles = new int[xSize * zSize * 6];
        var verts = 0;
        var tris = 0;

        for (var z = 0; z < zSize; z++)
        {
            for(var x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = verts;
                triangles[tris + 1] = verts + xSize + 1;
                triangles[tris + 2] = verts + 1;
                triangles[tris + 3] = verts + 1;
                triangles[tris + 4] = verts + xSize + 1;
                triangles[tris + 5] = verts + xSize + 2;
                tris += 6;
                verts++;
            }
            verts++;
        }
    }

    private void OnDrawGizmos()
    {
        if (vertices == null)
            return;
        for (var i = 0; i < vertices.Length ; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.05f);
        }
    }

    private void UpdateMesh()
    {
        mesh.Clear();
        
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        
        mesh.RecalculateNormals();
    }

}
