using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Jobs;

public class FloatingSystem : JobComponentSystem
{
    struct FloatingJob : IJobForEach<Translation,FloatingComponent>
    {
        public float deltaTime;

        public void Execute(ref Translation translation,ref FloatingComponent floating)
        {
            translation.Value.y += floating.speed * deltaTime;
            if(translation.Value.y>0.5)
            {
                floating.speed = -Mathf.Abs(floating.speed);
            }
            if (translation.Value.y < 0)
            {
                floating.speed = Mathf.Abs(floating.speed);
            }
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        FloatingJob floatingJob = new FloatingJob
        {
            deltaTime = Time.deltaTime
        };
        JobHandle floatingHandle = floatingJob.Schedule(this, inputDeps);
        return floatingHandle;
    }
}
