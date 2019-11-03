/*
 *  Boss的大招
 *  
 *  主要由以下组件组成
 *  RotateByOnePointComponent
 *  MoveSpeedComponent
 *  TimeComponent
 *  
 */
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
    private int amount;             //实体数量
    [SerializeField]
    private float radius;           //技能圆柱体半径
    [SerializeField]
    private float3 position;        //技能位置
    private float3 pos;             //每个实体的具体位置
    [SerializeField]
    private float moveSpeed;        //技能速度
    private float angle;            //技能初始角度
    [SerializeField]
    private float speed;            //技能圆柱体旋转速度
    [SerializeField]
    private float spawnSpeed;       //技能生成速度
    [SerializeField]
    private float spawnTime;        //技能生成的时间间隔
    private float spawnTmpTime;     //距离上一段技能生成的时间
    
    [SerializeField]
    private int skillTime;          //生成实体的次数
    private int times;              //累计生成实体的次数

    [SerializeField]
    private Mesh _mesh;             //渲染实体的网格
    [SerializeField]
    private Material _material;     //渲染实体的材质

    EntityManager entityManager;    //实体管理对象
    private void Start()
    {
        entityManager = World.Active.EntityManager;
        times = 0;
    }

    private void Update()
    {
        spawnTmpTime += spawnSpeed * Time.deltaTime;
        if(times<skillTime&&spawnTmpTime>=spawnTime)
        {
            Spawn();
            spawnTmpTime = 0;
            times++;
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
            typeof(TimeToLiveComponent),
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
            entityManager.SetComponentData(entityArr[i], new TimeToLiveComponent {
                value = 2f
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
