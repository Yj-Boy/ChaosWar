using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Jobs;

public class TimeToLiveSystem : JobComponentSystem
{
    struct TimeToLiveJob : IJobForEach<TimeToLiveComponent>
    {
        public float deltaTime;

        public void Execute(ref TimeToLiveComponent timeToLive)
        {
            if(timeToLive.value>=0)
            {
                timeToLive.value-=deltaTime;
            }
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        TimeToLiveJob timeToLiveJob = new TimeToLiveJob
        {
            deltaTime = Time.deltaTime
        };

        JobHandle timeToLiveHandle = timeToLiveJob.Schedule(this, inputDeps);
        return timeToLiveHandle;
    }
}
