using Unity.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Rendering;

public class CreateEnemyRotateByBoss : MonoBehaviour
{
    [SerializeField]
    private int amount;     //实体总数
    [SerializeField]
    private float3 position;    //生成实体位置
    [SerializeField]
    private float radius;   //旋转半径
    //[SerializeField]
    //private float floatingSpeed, floatingTopBound, floatingBottomBound;   //浮动参数
    [SerializeField]
    private float rotateByOnePointSpeed;
    [SerializeField]
    private float rotateSelfSpeed;
    private float rotateSelfAngle;
    
    [SerializeField]
    private Mesh _mesh;     //实体渲染网格
    [SerializeField]
    private Material _material;     //实体渲染材质

    EntityManager entityManager;       //实体管理对象，用于创建实体原型（Archetype）和实体（Entity）

    private void Start()
    {
        //初始化实体管理对象
        entityManager = World.Active.EntityManager;

        //创建并初始化实体原型
        EntityArchetype entityArchetype = entityManager.CreateArchetype(
            typeof(Translation),
            typeof(Rotation),
            typeof(RotateByWhichPointComponent),
            typeof(MoveSpeedComponent),
            typeof(MoveForwardComponent),
            typeof(RenderMesh),
            typeof(LocalToWorld)
            );

        //创建实体数组
        NativeArray<Entity> entityArr = new NativeArray<Entity>(amount, Allocator.Persistent);

        //将实体原型和实体数组结合
        entityManager.CreateEntity(entityArchetype, entityArr);

        //初始化每个实体对象
        for (int i = 0; i < entityArr.Length; i++)
        {
            float3 pos;
            int x = UnityEngine.Random.Range(0, 360);
            int z = UnityEngine.Random.Range(0, 360);
            pos.x= position.x + UnityEngine.Random.Range(-radius,radius) * math.cos(x * math.PI / 180);
            pos.y = UnityEngine.Random.Range(-radius, radius);
            pos.z = position.z + UnityEngine.Random.Range(-radius, radius) * math.sin(z * math.PI / 180);

            float a = math.abs(Vector2.Angle(new Vector2(pos.x, pos.z), new Vector2(1, 0)));
            rotateSelfAngle = 180 - a;

            entityManager.SetComponentData(entityArr[i], new Translation
            {
                Value = pos
            });

            entityManager.SetComponentData(entityArr[i], new MoveSpeedComponent
            {
                value = 0
            });
            entityManager.SetComponentData(entityArr[i], new RotateByWhichPointComponent
            {
                point = position,
                speed = rotateByOnePointSpeed,
                angle = rotateSelfAngle,
                radius = UnityEngine.Random.Range(-radius, radius)
            });

            entityManager.SetSharedComponentData(entityArr[i], new RenderMesh
            {
                mesh = _mesh,
                material = _material
            });
        }
        //释放实体数组
        entityArr.Dispose();
    }
}
