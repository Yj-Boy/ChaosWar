/*
 *  创建BOSS旁边上下浮动的怪物 
 */
using Unity.Collections;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Rendering;

public class CreateEnemyBesideBoss : MonoBehaviour
{
    [SerializeField]
    private int amount;     //创建的实体总数
    [SerializeField]
    private float floatingSpeed;    //上下浮动的速度
    [SerializeField]
    private float floatingTopBound;     //浮动上限
    [SerializeField]
    private float floatingBottomBound;  //浮动下限
    [SerializeField]
    private float xDistance;    //每个实体x轴上的间距
    [SerializeField]
    private float yDistance;    //每个实体y轴上的间距
    [SerializeField]
    private float zDistance;    //每个实体z轴上的间距
    [SerializeField]
    private int eachXAmount;    //每一行的实体数量
    [SerializeField]
    private float3 position;    //生成实体的位置
    private float positionZtmp;     //生成实体位置的z轴初始值
    [SerializeField]
    private Mesh _mesh;     //渲染实体的网格
    [SerializeField]
    private Material _material;     //渲染实体的材质

    EntityManager entityManager;    //实体管理对象，用于创建实体原型（Archetype）和实体（Entity）

    private void Start()
    {
        //相关变量初始化
        positionZtmp = position.z;
        xDistance = position.x;

        //初始化实体管理对象
        entityManager = World.Active.EntityManager;

        //创建并定义实体原型
        EntityArchetype entityArchetype = entityManager.CreateArchetype(
            typeof(FloatingComponent),
            typeof(Translation),
            typeof(MeshRenderer),
            typeof(LocalToWorld)
            );
        
        //创建实体数组
        NativeArray<Entity> entityArr = new NativeArray<Entity>(amount, Allocator.Persistent);

        //将实体原型和实体数组结合
        entityManager.CreateEntity(entityArchetype, entityArr);

        //对每一个实体进行初始化
        for (int i=0;i<entityArr.Length; i++)
        {
            float3 pos = new float3(
                position.x - eachXAmount / 2+UnityEngine.Random.Range(-1f,1f),
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
            if (position.x % eachXAmount == 0)
            {
                position.x = xDistance;
                position.z += zDistance;
                if (position.z >= positionZtmp+(zDistance*3))
                {
                    position.y -= yDistance;
                    position.z = positionZtmp;
                }
            }         
        }
        //释放实体数组
        entityArr.Dispose();
    }
}
