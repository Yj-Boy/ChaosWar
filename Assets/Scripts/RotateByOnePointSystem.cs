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

        public void Execute(ref Translation translation, ref Rotation rotation, ref RotateByWhichPointComponent rotateByWhichPoint)
        {
            /*
             * 求圆上点公式
             * x1 = x0 + r * cos(a * PI / 180)
             * y1 = y0 + r * sin(a * PI / 180)    
             */
            translation.Value.x = rotateByWhichPoint.point.x + rotateByWhichPoint.radius * math.cos((rotateByWhichPoint.angle * math.PI / 90));
            translation.Value.z = rotateByWhichPoint.point.z + rotateByWhichPoint.radius * math.sin((rotateByWhichPoint.angle * math.PI / 90));
            Vector3 pos = translation.Value - rotateByWhichPoint.point;
            Quaternion.LookRotation(pos);

            float a = math.abs(Vector2.Angle(new Vector2(pos.x, pos.z), new Vector2(1, 0)));

            //rotateByWhichPoint.angle += rotateByWhichPoint.speed*deltaTime;
            //if (rotateByWhichPoint.angle >= 360)
            //{
            //    rotateByWhichPoint.angle = 0;
            //}

            rotation.Value = quaternion.Euler(math.up() * rotateByWhichPoint.angle);

            if (rotateByWhichPoint.duration >= 0)
            {
                rotateByWhichPoint.angle += rotateByWhichPoint.speed * deltaTime;
                if (rotateByWhichPoint.angle >= 360)
                    rotateByWhichPoint.angle = 0f;
            }
            else if (rotateByWhichPoint.duration < 0)
            {
                rotateByWhichPoint.angle -= rotateByWhichPoint.speed * deltaTime;
                if (rotateByWhichPoint.angle <=  0f)
                    rotateByWhichPoint.angle = 0f;
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
