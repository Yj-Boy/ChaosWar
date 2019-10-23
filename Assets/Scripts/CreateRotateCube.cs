/*
 *  创建自身旋转实体（测试） 
 */
using Unity.Collections;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Rendering;

public class CreateRotateCube : MonoBehaviour
{
    [SerializeField]
    private int amount;     //实体总数
    [SerializeField]
    private float3 position;    //生成实体位置
    [SerializeField]
    private float speed;
    private float angle;

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
            typeof(RotateComponent),
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
            entityManager.SetComponentData(entityArr[i], new Translation
            {
                Value = transform.TransformPoint(position)     
            });

            angle = UnityEngine.Random.Range(0, 360);
            entityManager.SetComponentData(entityArr[i], new RotateComponent
            {
                speed = speed,
                angle = angle,
                initAngle = angle
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
