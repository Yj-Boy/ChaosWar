using Unity.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Rendering;

public class CreateBossMainSkill : MonoBehaviour
{
    [SerializeField]
    private int amount;
    [SerializeField]
    private float radius;
    [SerializeField]
    private float3 position;
    private float3 pos;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float angle;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float spawnSpeed;
    [SerializeField]
    private float spawnTime;
    private float spawnTmpTime;

    [SerializeField]
    private Mesh _mesh;
    [SerializeField]
    private Material _material;

    EntityManager entityManager;
    private void Start()
    {
        entityManager = World.Active.EntityManager;
    }

    private void Update()
    {
        spawnTmpTime += spawnSpeed * Time.deltaTime;
        if(spawnTmpTime>=spawnTime)
        {
            Spawn();
            spawnTmpTime = 0;
        }
    }

    private void Spawn()
    {
        EntityArchetype entityArchetype = entityManager.CreateArchetype(
            typeof(Translation),
            typeof(Rotation),
            typeof(MoveSpeedComponent),
            typeof(MoveForwardComponent),
            typeof(RotateByOnePointComponent),
            typeof(RenderMesh),
            typeof(LocalToWorld)
            );

        NativeArray<Entity> entityArr = new NativeArray<Entity>(amount, Allocator.Persistent);

        entityManager.CreateEntity(entityArchetype, entityArr);

        for (int i = 0; i < entityArr.Length; i++)
        {
            float r = UnityEngine.Random.Range(-radius, radius);
            pos.x = position.x + r * math.cos(UnityEngine.Random.Range(0, 360) * math.PI / 180);
            pos.y = position.z + r * math.sin(UnityEngine.Random.Range(0, 360) * math.PI / 180);
            pos.z = UnityEngine.Random.Range(-radius, radius);
            entityManager.SetComponentData(entityArr[i], new Translation
            {
                Value = pos
            });

            entityManager.SetComponentData(entityArr[i], new MoveSpeedComponent
            {
                value = moveSpeed
            });

            entityManager.SetComponentData(entityArr[i], new Rotation
            {
                Value = quaternion.Euler(math.up() * 135)
            });
            angle = math.abs(Vector2.Angle(new Vector2(pos.x, pos.y), new Vector2(1, 0)));
            entityManager.SetComponentData(entityArr[i], new RotateByOnePointComponent
            {
                point = position,
                angle = angle,
                radius = radius,
                speed = speed,
                duration = UnityEngine.Random.Range(-1, 0)
            });
            entityManager.SetSharedComponentData(entityArr[i], new RenderMesh
            {
                mesh = _mesh,
                material = _material
            });
        }
        entityArr.Dispose();
    }
}
