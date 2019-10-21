using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Collections;

public class MovementSystem : JobComponentSystem
{
    struct MovementJob : IJobForEach<Translation, Rotation, MoveSpeedComponent,MoveForwardComponent>
    {
        public float deltaTime;

        public void Execute(ref Translation translation, [ReadOnly]ref Rotation rotation,[ReadOnly]ref MoveSpeedComponent moveSpeed,[ReadOnly]ref MoveForwardComponent moveForward)
        {
            //translation.Value.z += deltaTime*moveSpeed.value;
            translation.Value += moveSpeed.value * math.forward(rotation.Value) * deltaTime ;
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        MovementJob movementJob = new MovementJob
        {
            deltaTime = Time.deltaTime
        };

        JobHandle movementHandle = movementJob.Schedule(this, inputDeps);

        return movementHandle;
    }
}
