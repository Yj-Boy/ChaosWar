/*
 *  实体自身旋转系统
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Jobs;

public class RotateSystem : JobComponentSystem
{
    struct RotateJob : IJobForEach<Translation, Rotation, RotateComponent>
    {
        public float deltaTime;

        public void Execute(ref Translation translation, ref Rotation rotation, ref RotateComponent rotate)
        {
            rotation.Value = quaternion.Euler(math.up()*rotate.angle);

            if (rotate.duration >= 0)
            {
                rotate.angle += rotate.speed * deltaTime;
                if (rotate.angle >= rotate.initAngle + 360f)
                    rotate.angle = rotate.initAngle;
            }
            else if (rotate.duration < 0)
            {
                rotate.angle -= rotate.speed * deltaTime;
                if (rotate.angle <= rotate.initAngle - 360f)
                    rotate.angle = rotate.initAngle;
            }
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
