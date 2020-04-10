using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace DOTS.System_Comparison
{
    public class MoveSpeedSystem : SystemBase
    {
        private EntityQuery query;
        protected override void OnCreate()
        {
            query = GetEntityQuery(ComponentType.ReadOnly<MoveSpeed>());
        }

        protected override void OnUpdate()
        {
            float time = Time.DeltaTime;
            float bottomBound = GameManager.Instance.bottomBound;
            float topBound = GameManager.Instance.topBound;
            /*Entities.ForEach((ref Translation t, in MoveSpeed speed) =>
            {
                var val = t.Value;
                //val.y -= time * speed.Value;
                val.y = val.y - math.mul(time , speed.Value);
                //if (val.y < bottomBound)
                //    val.y = topBound;
                val.y = math.@select(val.y, topBound, val.y < bottomBound);
                t.Value = val;
            }).ScheduleParallel();*/
            var job = new DescendJob
            {
                bottomBound =  bottomBound,
                topBound =  topBound,
                time = time,
                movespeed =  GetArchetypeChunkComponentType<MoveSpeed>(true),
                translation = GetArchetypeChunkComponentType<Translation>()
            };

            Dependency = job.Schedule(query, Dependency);
        }

        [BurstCompile]
        public struct DescendJob : IJobChunk
        {
            public float time;
            public float bottomBound;
            public float topBound; 
            [Unity.Collections.ReadOnly] public ArchetypeChunkComponentType<MoveSpeed> movespeed;
            public ArchetypeChunkComponentType<Translation> translation;
            
            public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
            {
                var speedArray = chunk.GetNativeArray(movespeed);
                var translationArray = chunk.GetNativeArray(translation);
                for (int i = 0; i < chunk.Count; i++)
                {
                    var val = translationArray[i].Value;
                    //val.y -= time * speed.Value;
                    val.y = val.y - math.mul(time , speedArray[i].Value);
                    //if (val.y < bottomBound)
                    //    val.y = topBound;
                    val.y = math.@select(val.y, topBound, val.y < bottomBound);
                    translationArray[i] = new Translation{Value =  val};
                }
            }
        }
    }
}