/*
 *  实体围绕某一点旋转系统 
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
    struct RoteteByOnePointJob : IJobForEach<Translation, Rotation, RotateByWhichPointComponent>
    {
        public float deltaTime;
        //private float r;       

        public void Execute(ref Translation translation, ref Rotation rotation, ref RotateByWhichPointComponent rotateByWhichPoint)
        {
            //r = math.sqrt((translation.Value.x - rotateByWhichPoint.point.x) * (translation.Value.x - rotateByWhichPoint.point.x))
            //   + math.sqrt((translation.Value.z - rotateByWhichPoint.point.z) * (translation.Value.z - rotateByWhichPoint.point.z));
            //x1 = x0 + r * cos(a * PI / 180)
            //y1 = y0 + r * sin(a * PI / 180)
            translation.Value.x = rotateByWhichPoint.point.x + rotateByWhichPoint.radius * math.cos((rotateByWhichPoint.angle * math.PI / 180));
            translation.Value.z = rotateByWhichPoint.point.z + rotateByWhichPoint.radius * math.sin((rotateByWhichPoint.angle * math.PI / 180));

            //rotation.Value.value = new float4(0, rotateByWhichPoint.angle, 0, 0);
            //rotation.Value.value.x += deltaTime;
            //rotation.Value = math.mul(math.normalize(rotation.Value),
                //quaternion.AxisAngle(math.up(), 1 * deltaTime));

            rotateByWhichPoint.angle += 60*deltaTime;
            if (rotateByWhichPoint.angle >= 360)
            {
                rotateByWhichPoint.angle = 0;
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
