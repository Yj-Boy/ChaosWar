/*
 *  达到一定时间销毁实体系统 
 */
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;

//在MovementSystem后执行
[UpdateAfter(typeof(MovementSystem))]
public class TimeToDestorySystem : JobComponentSystem
{
    //创建buffer用来初始化commands
    EndSimulationEntityCommandBufferSystem buffer;
    //重写OnCreate函数
    protected override void OnCreate()
    {
        //初始化buffer
        buffer = World.Active.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }
    //定义IJobForEachWithEntity类型的结构体，该类型结构体可以对每一实体进行具体操作
    struct TimeJob : IJobForEachWithEntity<TimeComponent>
    {
        public EntityCommandBuffer.Concurrent commands;
        public float deltaTime;

        public void Execute(Entity entity,int index, ref TimeComponent time)
        {
            time.value -= deltaTime;
            if (time.value <= 0f)
                commands.DestroyEntity(index, entity);  //销毁实体
        }
    }
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        TimeJob timeJob = new TimeJob
        {
            //用buffer初始化cammands
            commands=buffer.CreateCommandBuffer().ToConcurrent(),
            deltaTime = Time.deltaTime
        };

        JobHandle timeHandle = timeJob.Schedule(this, inputDeps);
        //将handle添加到buffer的生产序列中
        buffer.AddJobHandleForProducer(timeHandle);

        return timeHandle;
    }
}
