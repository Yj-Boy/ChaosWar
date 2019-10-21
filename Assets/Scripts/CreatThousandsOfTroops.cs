using Unity.Collections;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Rendering;


public class CreatThousandsOfTroops : MonoBehaviour
{
    [SerializeField]
    private int amount;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float floatingSpeed,floatingTopBound,floatingBottomBound;
    [SerializeField]
    private float xNum;
    [SerializeField]
    private Vector3 forwardVector;
    [SerializeField]
    private Mesh _mesh;
    [SerializeField]
    private Material _material;

    EntityManager entityManager;

    private void Start()
    {
        entityManager = World.Active.EntityManager;

        EntityArchetype entityArchetype = entityManager.CreateArchetype(
            typeof(MoveSpeedComponent),
            typeof(FloatingComponent),
            typeof(Translation),
            typeof(Rotation),
            typeof(MoveForwardComponent),
            typeof(RenderMesh),
            typeof(LocalToWorld)
            );

        NativeArray<Entity> entityArr = new NativeArray<Entity>(amount,Allocator.Persistent);

        entityManager.CreateEntity(entityArchetype, entityArr);

        float positionX = 0f;
        float positionZ = 0f;
        for(int i=0;i<entityArr.Length;i++)
        {
            entityManager.SetComponentData(entityArr[i], new MoveSpeedComponent { value = moveSpeed });
            float3 pos = new float3(
                positionX - xNum / 2, 
                noise.cnoise(new float2(positionX, 1f) * UnityEngine.Random.Range(0f, 1f)),
                positionZ);
            entityManager.SetComponentData(entityArr[i], new Translation
            {
                Value = transform.TransformPoint(pos)
            });
            entityManager.SetComponentData(entityArr[i], new FloatingComponent
            {
                speed = floatingSpeed,
                topBound = floatingTopBound,
                bottomBound = floatingBottomBound,
                floatingStartPosY = 0
            });    
            entityManager.SetComponentData(entityArr[i], new Rotation { Value = Quaternion.Euler(forwardVector)});
            entityManager.SetSharedComponentData(entityArr[i], new RenderMesh { mesh = _mesh, material = _material });
            positionX++;
            if (positionX % 10 == 0)
            {
                positionX = 0f;
                positionZ -= 1f;
            }             
        }
        entityArr.Dispose();
    }
}
