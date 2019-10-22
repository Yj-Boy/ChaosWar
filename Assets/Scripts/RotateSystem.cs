using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Jobs;

public class RotateSystem : JobComponentSystem
{
    struct RotateJob : IJobForEach<Translation, Rotation, RotateSpeedComponent>
    {
        public float deltaTime;

        public void Execute(ref Translation translation, ref Rotation rotation, ref RotateSpeedComponent rotateSpeed)
        {
            //rotation.Value = math.mul(math.normalize(rotation.Value),
            //    quaternion.AxisAngle(math.up(), rotateSpeed.value * deltaTime));
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        RotateJob rotateJob = new RotateJob
        {
            deltaTime = Time.deltaTime
        };

        JobHandle rotateHandle = rotateJob.Schedule(this, inputDeps);
        return rotateHandle;
    }
}
