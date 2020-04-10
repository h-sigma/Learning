using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace DOTS
{
    public class OceanSimulation : MonoBehaviour
    {
        [Serializable]
        public class PerlinNoiseLayer
        {
            public float scale;
            public float speed;
            public float height;
        }

        public struct FlattenMeshJob : IJobParallelFor
        {
            public NativeArray<Vector3> vertices;
            
            public void Execute(int i)
            {
                var vertex = vertices[i];
                vertex.y = 0;
                vertices[i] = vertex;
            }
        }
        public struct PerlinNoiseLayerJob : IJobParallelFor
        {
            public float scale;
            public float speed;
            public float height;
            public float time;

            public NativeArray<Vector3> vertices;
            
            public void Execute(int i)
            {
                var vertex = vertices[i];
                var x = vertex.x * scale + time * speed;
                var z = vertex.z * scale + time * speed;

                vertex.y += (Mathf.PerlinNoise(x, z) - 0.5f) * height;

                vertices[i] = vertex;
            }
        }

        public bool useJobSystem = true;
        public int batchSize;
        
        public List<PerlinNoiseLayer> layers;
        
        private Mesh mesh;
        private Vector3[] vertices;
        private NativeArray<Vector3> verts;
        private int width = 500;
        private int length = 500;
        public void Awake()
        {
            mesh = new Mesh();

            vertices = new Vector3[width * length];

            var triangles = new List<int>();

            for (int j = 0; j < width; j++)
            {
                for (int i = 0; i < length; i++)
                {
                    vertices[j * length + i] = new Vector3(i, 0, j);
                }
            }

            for (int j = 0; j < width - 1; j++)
            {
                for (int i = 0; i < length - 1; i++)
                {
                    triangles.Add(j * length + i);
                    triangles.Add((j + 1) * length + i);
                    triangles.Add(j * length + i + 1);
                    triangles.Add(j * length + i + 1);
                    triangles.Add((j + 1) * length + i);
                    triangles.Add((j + 1) * length + i + 1);
                }
            }
            
            mesh.vertices = vertices;
            mesh.SetTriangles(triangles, 0);
            mesh.RecalculateNormals();
            GetComponent<MeshFilter>().sharedMesh = mesh;
            
            verts = new NativeArray<Vector3>(length * width, Allocator.Persistent);
        }

        public void Update()
        {
            vertices = mesh.vertices;
            
            if(useJobSystem)
            {
                verts.CopyFrom(vertices);

                var flattenHandle = FlattenMesh(verts);
                JobHandle n = default;
                for (int i = 0; i < layers.Count; i++)
                {
                    if (i == 0)
                        n = ApplyPerlinNoise(verts, layers[i], Time.timeSinceLevelLoad, flattenHandle);
                    else
                        n = ApplyPerlinNoise(verts, layers[i], Time.timeSinceLevelLoad, n);
                }

                n.Complete();

                verts.CopyTo(vertices);
            }
            else
            {
                FlattenMesh();

                foreach (var layer in layers)
                {
                    ApplyPerlinNoise(layer, Time.timeSinceLevelLoad);
                }
            }
            
            mesh.SetVertices(vertices);
            mesh.RecalculateNormals();
        }

        private JobHandle ApplyPerlinNoise(NativeArray<Vector3> v, PerlinNoiseLayer layer, float time, JobHandle n)
        {
            var job = new PerlinNoiseLayerJob();
            job.vertices = v;
            job.height = layer.height;
            job.scale = layer.scale;
            job.speed = layer.speed;
            job.time = time;
            
            return job.Schedule(v.Length, batchSize, n);
        }

        private JobHandle FlattenMesh(NativeArray<Vector3> v)
        {
            var job = new FlattenMeshJob();
            job.vertices = v;
            return job.Schedule(v.Length, batchSize);
        }

        private void FlattenMesh()
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].y = 0;
            }
        }

        private void ApplyPerlinNoise(PerlinNoiseLayer layer, float time)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                var vertex = vertices[i];
                var x = vertex.x * layer.scale + time * layer.speed;
                var z = vertex.z * layer.scale + time * layer.speed;

                vertices[i].y += (Mathf.PerlinNoise(x, z) - 0.5f) * layer.height;
            }
        }
        
        public void OnDestroy()
        {
            verts.Dispose();
        }
    }
}