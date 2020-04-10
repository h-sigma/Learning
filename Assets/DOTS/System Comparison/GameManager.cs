using System;
using System.Linq;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = System.Random;

namespace DOTS.System_Comparison
{
    public class GameManager : MonoBehaviour
    {
        /*
        public struct MovementJob : IJobParallelForTransform
        {
            public float speed;
            public float topBound;
            public float bottomBound;
            public float time;

            public void Execute(int i, TransformAccess access)
            {
                var pos = access.position;
                pos += Vector3.down * (speed * time);
                if (pos.y < bottomBound)
                    pos.y = topBound;
                access.position = pos;
            }
        }
        */


        public static GameManager _instance;

        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    var mangs = FindObjectsOfType<GameManager>();
                    if (mangs.Length > 0) _instance = mangs[0];
                    for (int i = 1; i < mangs.Length; i++)
                    {
                        DestroyImmediate(mangs[i].gameObject);
                    }

                    if (_instance == null)
                    {
                        var go = new GameObject("Game Manager");
                        _instance = go.AddComponent<GameManager>();
                        DontDestroyOnLoad(go);
                    }
                }

                return _instance;
            }
        }

        public GameObject enemyPrefab;
        public int enemyShipIncrement;
        public float enemySpeed = 10f;
        public float topBound = 30f;
        public float bottomBound = -30f;
        public float leftBound = -30f;
        public float rightBound = 30f;

        private float[] frameTime = new float[300];
        private int currFrame;
        private int enemyCount = 0;

        /*
        private JobHandle handle;
        private MovementJob job;
        private TransformAccessArray array;
        */

        private EntityManager manager;
        private Entity shipEntity;

        public void Start()
        {
            /*
             * array = new TransformAccessArray(1);
             */
            manager = World.DefaultGameObjectInjectionWorld.EntityManager;
            shipEntity = manager.CreateEntityQuery(ComponentType.ReadWrite<MoveSpeed>()).GetSingletonEntity();
            //manager.AddComponent<MoveSpeed>(shipEntity);
        }

        private void OnGUI()
        {
            GUILayout.Label(enemyCount.ToString());
            GUILayout.Label((1 / frameTime.Average()).ToString() + "fps");
        }

        void Update()
        {
            frameTime[currFrame] = Time.deltaTime;
            currFrame++;
            if (currFrame == 300)
            {
                currFrame = 0;
            }

            /*
            handle.Complete();
            */

            if (Input.GetKeyDown("space"))
                AddShips(enemyShipIncrement);

            /*
            job = new MovementJob
            {
                speed = enemySpeed,
                time = Time.deltaTime,
                bottomBound = bottomBound,
                topBound =  topBound
            };

            handle = job.Schedule(array);
            
            JobHandle.ScheduleBatchedJobs();
            */
        }

        void AddShips(int amount)
        {
            enemyCount += amount;
            var entities = new NativeArray<Entity>(amount, Allocator.Temp);
            manager.Instantiate(shipEntity, entities);

            for (int i = 0; i < amount; i++)
            {
                var xVal = UnityEngine.Random.Range(leftBound, rightBound);
                var yVal = UnityEngine.Random.Range(bottomBound, topBound);
                //manager.SetComponentData<LocalToWorld>(entities[i], new LocalToWorld{Value = new float4x4(float3x3.identity, new float3(xVal, yVal, 0))});

                manager.SetComponentData<Translation>(entities[i], new Translation {Value = new float3(xVal, yVal, 0)});
                manager.SetComponentData<MoveSpeed>(entities[i], new MoveSpeed {Value = UnityEngine.Random.Range(enemySpeed/2, enemySpeed)});
            }

            entities.Dispose();
        }

        private void OnDestroy()
        {
        }

        /*
        void AddShips(int amount)
        {
            enemyCount += amount;
            
            handle.Complete();    //ensures no jobs are running
            
            array.capacity = array.length + amount;
            for (int i = 0; i < amount; i++)
            {
                float xVal = Random.Range(leftBound, rightBound);
                float yVal = Random.Range(bottomBound + 10f, topBound - 10f);

                Vector3 pos = new Vector3(xVal, yVal, 0);
                Quaternion rot = Quaternion.Euler(0f, 0f, 0f);

                var obj = Instantiate(enemyPrefab, pos, rot) as GameObject;
                array.Add(obj.transform);
            }
        }

        private void OnDestroy()
        {
            array.Dispose();
        }
        */
    }
}