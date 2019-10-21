using Unity.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Rendering;
using System;

public class CreateEnemyBesideBoss : MonoBehaviour
{
    [SerializeField]
    private int amount;
    [SerializeField]
    private float floatingSpeed;
    [SerializeField]
    private float floatingTopBound;
    [SerializeField]
    private float floatingBottomBound;
    [SerializeField]
    private float xNum;

    private float3 position;

    [SerializeField]
    private Mesh _mesh;
    [SerializeField]
    private Material _material;

    EntityManager entityManager;

    private void Start()
    {
        entityManager = World.Active.EntityManager;

        EntityArchetype entityArchetype = entityManager.CreateArchetype(
            typeof(FloatingComponent),
            typeof(Translation),
            typeof(MeshRenderer),
            typeof(LocalToWorld)
            );

        NativeArray<Entity> entityArr = new NativeArray<Entity>(amount, Allocator.Persistent);

        entityManager.CreateEntity(entityArchetype, entityArr);

        for (int i=0;i<entityArr.Length; i++)
        {
            float3 pos = new float3(
                position.x - xNum / 2+UnityEngine.Random.Range(-1f,1f),
                position.y + noise.cnoise(new float2(position.x, 1f) * UnityEngine.Random.Range(0f, 1f)),
                position.z + UnityEngine.Random.Range(-2f, 2f));
            entityManager.SetComponentData(entityArr[i], new Translation
            {
                Value = transform.TransformPoint(pos)
            });
            entityManager.SetComponentData(entityArr[i], new FloatingComponent
            {               
                speed = UnityEngine.Random.Range(0f,2f),
                topBound = floatingTopBound,
                bottomBound = floatingBottomBound,
                floatingStartPosY = pos.y
            });
            entityManager.AddSharedComponentData(entityArr[i], new RenderMesh { mesh = _mesh, material = _material });
            position.x++;
            if (position.x % 40 == 0)
            {
                position.x = 0f;
                position.z += 3f;
                if (position.z >= 29f)
                {
                    position.y -= 2f;
                    position.z = 20f;
                }
            }
            
        }
        entityArr.Dispose();
    }
}
