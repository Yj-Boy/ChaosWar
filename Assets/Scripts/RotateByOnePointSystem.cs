/*
 *  实体围绕某一点旋转系统 （弃用）
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Mathematics;

public class RotateByOnePointSystem : JobComponentSystem
{
    struct RoteteByOnePointJob : IJobForEach<Translation, Rotation, RotateByOnePointComponent>
    {
        public float deltaTime;

        public void Execute(ref Translation translation, ref Rotation rotation, ref RotateByOnePointComponent rotateByOnePoint)
        {
            /*
             * 求圆上点公式
             * x1 = x0 + r * cos(a * PI / 180)
             * y1 = y0 + r * sin(a * PI / 180)    
             */
            translation.Value.x = rotateByOnePoint.point.x + rotateByOnePoint.radius * math.cos((rotateByOnePoint.angle * math.PI / 90));
            translation.Value.y = rotateByOnePoint.point.y + rotateByOnePoint.radius * math.sin((rotateByOnePoint.angle * math.PI / 90));

            if (rotateByOnePoint.duration >= 0)
            {
                rotateByOnePoint.angle += rotateByOnePoint.speed * deltaTime;
                if (rotateByOnePoint.angle >= 360)
                    rotateByOnePoint.angle = 0f;               
            }
            else if (rotateByOnePoint.duration < 0)
            {
                rotateByOnePoint.angle -= rotateByOnePoint.speed * deltaTime;
                if (rotateByOnePoint.angle <= -360f)
                    rotateByOnePoint.angle = 0f;
            }
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        RoteteByOnePointJob roteteByOnePointJob = new RoteteByOnePointJob
        {
            deltaTime = Time.deltaTime
        };
        JobHandle roteteByOnePointHandle = roteteByOnePointJob.Schedule(this, inputDeps);
        return roteteByOnePointHandle;
    }
}
