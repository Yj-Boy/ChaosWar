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
            //translation.Value.x = rotateByWhichPoint.point.x + rotateByWhichPoint.radius * math.cos((rotateByWhichPoint.angle * math.PI / 90));
            //translation.Value.z = rotateByWhichPoint.point.z + rotateByWhichPoint.radius * math.sin((rotateByWhichPoint.angle * math.PI / 90));

            //rotation.Value = quaternion.Euler(math.up() * rotateByOnePoint.angleSelf);


            //if (rotateByWhichPoint.duration >= 0)
            //{
            //    rotateByWhichPoint.angle += rotateByWhichPoint.speed * deltaTime;
            //    if (rotateByWhichPoint.angle >= 360)
            //        rotateByWhichPoint.angle = 0f;
            //    rotateByWhichPoint.angleSelf += rotateByWhichPoint.speedSelf * deltaTime;
            //    if (rotateByWhichPoint.angleSelf >= 360)
            //        rotateByWhichPoint.angleSelf = 0f;
            //}
            //else if (rotateByWhichPoint.duration < 0)
            //{
            //    rotateByWhichPoint.angle -= rotateByWhichPoint.speed * deltaTime;
            //    if (rotateByWhichPoint.angle <= -360f)
            //        rotateByWhichPoint.angle = 0f;
            //    rotateByWhichPoint.angleSelf -= rotateByWhichPoint.speedSelf * deltaTime;
            //    if (rotateByWhichPoint.angleSelf <= -360f)
            //        rotateByWhichPoint.angleSelf = 0f;
            //}
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
