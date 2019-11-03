/*
 *  创建空气墙，主要用于销毁千军万马的实体 
 */
using Unity.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Rendering;

public class CreateWallToDestroy : MonoBehaviour
{
    /*
     * 
     *  原采用PureECS来生成空气墙，发现无法对Scale的单个属性进行调整
     *  
     *  所以改用HyBridECS来生成空气墙
     *
     */

    /*
     *  HybridECS 
     */
    public GameObject goPrefab;     //要转化为实体的预制体
    public float3 position;         //生成实体的位置

    Entity entity;                  //实体对象
    EntityManager entityManager;    //实体管理对象

    private void Start()
    {
        //参数检查
        if(goPrefab==null)
        {
            Debug.Log(GetType() + "/Start()/空气墙预制体没有指定，请检查!");
        }
        //将对象转化为Entity
        entity = GameObjectConversionUtility.ConvertGameObjectHierarchy(
            goPrefab,
            World.Active);
        //得到实体管理器
        entityManager = World.Active.EntityManager;
        //从实体预设，大量克隆实体
        Entity entityClone = entityManager.Instantiate(entity); 
        //实体管理器设置其中的组件参数
        entityManager.SetComponentData(entityClone, new Translation
        {
            Value = position
        });
        //把定义的组件加入到实体管理器中
        entityManager.AddComponentData(entityClone, new WallTagComponent { });
        entityManager.AddComponentData(entityClone, new IsDestroyComponent
        {
            value = false
        });
    }

    /*
     *  PureECS 
     */

    //[SerializeField]
    //private float3 position;    //生成实体位置

    //[SerializeField]
    //private Mesh _mesh;     //实体渲染网格
    //[SerializeField]
    //private Material _material;     //实体渲染材质

    //EntityManager entityManager;       //实体管理对象，用于创建实体原型（Archetype）和实体（Entity）

    //private void Start()
    //{
    //    //初始化实体管理对象
    //    entityManager = World.Active.EntityManager;

    //    //创建并初始化实体原型
    //    EntityArchetype entityArchetype = entityManager.CreateArchetype(
    //        typeof(Translation),
    //        typeof(Scale),
    //        typeof(WallTagComponent),
    //        typeof(IsDestroyComponent),
    //        typeof(RenderMesh),
    //        typeof(LocalToWorld)
    //        );

    //    //创建实体数组
    //    NativeArray<Entity> entityArr = new NativeArray<Entity>(1, Allocator.Persistent);

    //    //将实体原型和实体数组结合
    //    entityManager.CreateEntity(entityArchetype, entityArr);

    //    //初始化每个实体对象
    //    for (int i = 0; i < entityArr.Length; i++)
    //    {
    //        entityManager.SetComponentData(entityArr[i], new Translation
    //        {
    //            Value = transform.TransformPoint(position)
    //        });

    //        entityManager.SetComponentData(entityArr[i], new Scale
    //        {
    //            Value=30
    //        });

    //        entityManager.SetComponentData(entityArr[i], new IsDestroyComponent
    //        {
    //            value = false
    //        });
    //        entityManager.SetSharedComponentData(entityArr[i], new RenderMesh
    //        {
    //            mesh = _mesh,
    //            material = _material
    //        });
    //    }
    //    //释放实体数组
    //    entityArr.Dispose();
    //}
}
