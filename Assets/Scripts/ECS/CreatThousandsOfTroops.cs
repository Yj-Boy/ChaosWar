﻿/*
 *  创建主角大招（千军万马）
 */
using Unity.Collections;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Rendering;
using System;

public class CreatThousandsOfTroops : MonoBehaviour
{
    [SerializeField]
    private int amount;                     //实体总数
    [SerializeField]
    private float3 position;                //生成实体的位置
    [SerializeField]
    private float moveSpeed;                //移动速度
    [SerializeField]
    private float floatingSpeed;            //浮动参数
    [SerializeField]
    private float floatingTopBound;         //浮动参数
    [SerializeField]
    private float floatingBottomBound;      //浮动参数
    [SerializeField]
    private int xNum;                       //x轴的实体数量
    //private int yNum;                     //生成多少列的实体
    [SerializeField]
    private Vector3 forwardVector;          //移动朝向（xyz）                                    
    [SerializeField]
    private Mesh _mesh;                     //实体渲染网格
    [SerializeField]
    private Material _material;             //实体渲染材质

    EntityManager entityManager;            //实体管理对象，用于创建实体原型（Archetype）和实体（Entity）
    EntityArchetype entityArchetype;        //实体原型对象

    private void Start()
    {
        //初始化实体管理对象
        entityManager = World.Active.EntityManager;

        //创建并初始化实体原型
        entityArchetype = entityManager.CreateArchetype(
            typeof(MoveSpeedComponent),
            typeof(FloatingComponent),
            typeof(Translation),
            typeof(Rotation),
            typeof(MoveForwardComponent),
            typeof(TimeToLiveComponent),
            typeof(IsDestroyComponent),
            typeof(TroopsTagComponent),
            typeof(RenderMesh),
            typeof(LocalToWorld)
            );

        ////行数大于0，执行InvokeRepeating函数重复调用SpawnEntity生成实体
        //yNum = amount / xNum;
        ////Debug.Log("yNum:"+yNum);
        //if(yNum>0)
        //{
        //    InvokeRepeating("SpawnEntity", 0.3f, 0.3f);
        //}

        //SpawnEntity();
    }

    public void SpawnEntity()
    {
        //创建实体数组
        NativeArray<Entity> entityArr = new NativeArray<Entity>(amount, Allocator.Persistent);

        //将实体原型和实体数组结合
        entityManager.CreateEntity(entityArchetype, entityArr);

        //创建实体的位置
        float positionX = 0f;
        float positionZ = 0f;
        //初始化每个实体对象
        for (int i = 0; i < entityArr.Length; i++)
        {
            entityManager.SetComponentData(entityArr[i], new MoveSpeedComponent { value = moveSpeed });
            float3 pos = new float3(
                position.x + positionX - xNum / 2,
                position.y + noise.cnoise(new float2(positionX, 1f) * UnityEngine.Random.Range(0f, 1f)),
                position.z + positionZ);
            entityManager.SetComponentData(entityArr[i], new Translation
            {
                Value = transform.TransformPoint(pos)
            });
            entityManager.SetComponentData(entityArr[i], new FloatingComponent
            {
                speed = floatingSpeed,
                topBound = floatingTopBound,
                bottomBound = floatingBottomBound,
                floatingStartPosY = position.y
            });
            entityManager.SetComponentData(entityArr[i], new Rotation
            {
                Value = Quaternion.Euler(forwardVector)
            });
            entityManager.SetComponentData(entityArr[i], new TimeToLiveComponent
            {
                value = 30f
            });
            entityManager.SetComponentData(entityArr[i], new IsDestroyComponent
            {
                value = false,
            });
            entityManager.SetSharedComponentData(entityArr[i], new RenderMesh
            {
                mesh = _mesh,
                material = _material
            });
            positionX++;
            if (positionX % xNum == 0)
            {
                positionX = 0f;
                positionZ -= 1f;
            }
        }
        //释放实体数组
        entityArr.Dispose();

        ////每生产一行实体，行数减一
        ////当行数小于0时，取消执行该脚本下的所有Invoke函数
        //yNum--;
        //if(yNum<=0)
        //{
        //    CancelInvoke();
        //}

        ShakeCamera.SetCameraShake(10f,0.2f,0.5f);
        //GameObject.Find("_script").GetComponent<ShakeCamera>().SetCameraShakeTime(10f);
    }
}
