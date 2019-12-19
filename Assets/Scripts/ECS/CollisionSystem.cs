/*
 *  碰撞检测系统
 *  
 *  用于简单的检测实体之间是否发生碰撞
 */
using UnityEngine;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

//在移动系统后、销毁系统之前去执行碰撞检测
[UpdateAfter(typeof(MovementSystem))]
[UpdateBefore(typeof(DestorySystem))]
public class CollisionSystem : JobComponentSystem
{
    //定义组去承接要检测碰撞的entity
    EntityQuery troopsGroup;
    EntityQuery wallGroup;

    protected override void OnCreate()
    {
        //通过GetEntityQuery寻找确定的entity，并赋值给定义好的组
        troopsGroup = GetEntityQuery(typeof(IsDestroyComponent),ComponentType.ReadOnly<Translation>(), ComponentType.ReadOnly<TroopsTagComponent>());
        wallGroup = GetEntityQuery(typeof(IsDestroyComponent),ComponentType.ReadOnly<Translation>(), ComponentType.ReadOnly<WallTagComponent>());
    }

    //定义IJobChunk结构体，碰撞检测的逻辑将在这里面完成
    [BurstCompile]
    struct CollisionJob : IJobChunk
    {
        //检测碰撞范围的半径
        //public float radius;

        //定义相关的的ArchetypeChunkComponentType
        public ArchetypeChunkComponentType<IsDestroyComponent> isDestroyType;
        [ReadOnly] public ArchetypeChunkComponentType<Translation> translationType;

        //定义一个translation数组
        [DeallocateOnJobCompletion]
        [ReadOnly] public NativeArray<Translation> transToTestAgainst;


        public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
        {
            //定义与之前ArchetypeChunkComponentType相关的变量
            var chunkIsDestroy = chunk.GetNativeArray(isDestroyType);
            var chunkTranslations = chunk.GetNativeArray(translationType);

            //遍历每一个chunk，进行碰撞检测
            for (int i = 0; i < chunk.Count; i++)
            {
                IsDestroyComponent isDestroy = chunkIsDestroy[i];
                Translation pos = chunkTranslations[i];

                for (int j = 0; j < transToTestAgainst.Length; j++)
                {
                    Translation pos2 = transToTestAgainst[j];

                    //判断是否碰撞
                    if (CheckCollision2(pos.Value, pos2.Value, 2))
                    {
                        isDestroy.value = true;
                    }
                }
                chunkIsDestroy[i] = isDestroy;
            }
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        var isDestroyType = GetArchetypeChunkComponentType<IsDestroyComponent>(false);
        var translationType = GetArchetypeChunkComponentType<Translation>(true);

        //float enemyRadius = 20;
        //float playerRadius = 1;

        var jobEvB = new CollisionJob()
        {
            //radius = enemyRadius * enemyRadius,
            isDestroyType = isDestroyType,
            translationType = translationType,
            transToTestAgainst = wallGroup.ToComponentDataArray<Translation>(Allocator.TempJob)
        };

        JobHandle jobHandle;
        return jobHandle = jobEvB.Schedule(troopsGroup, inputDependencies);

        //注释检测销毁碰撞墙的代码
        //var jobPvE = new CollisionJob()
        //{
        //    //radius = playerRadius * playerRadius,
        //    isDestroyType = isDestroyType,
        //    translationType = translationType,
        //    transToTestAgainst = troopsGroup.ToComponentDataArray<Translation>(Allocator.TempJob)
        //};

        //return jobPvE.Schedule(wallGroup, jobHandle);
    }

    //检测是否碰撞方法（圆形相交检测）
    static bool CheckCollision(float3 posA, float3 posB, float radiusSqr)
    {
        float3 delta = posA - posB;
        float distanceSquare = delta.x * delta.x + +delta.y*delta.y+delta.z * delta.z;

        return distanceSquare <= radiusSqr;
    }

    //检测是否碰撞方法（方形距离检测）
    static bool CheckCollision2(float3 posA,float3 posB,float zLenth)
    {
        float3 delta = posA - posB;

        return math.abs(delta.z) < zLenth;
    }
}
