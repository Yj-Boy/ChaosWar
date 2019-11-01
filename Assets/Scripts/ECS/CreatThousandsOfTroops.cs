/*
 *  创建主角大招（千军万马）
 */
using Unity.Collections;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Rendering;


public class CreatThousandsOfTroops : MonoBehaviour
{
    [SerializeField]
    private int amount;     //实体总数
    [SerializeField]
    private float3 position;    //生成实体的位置
    [SerializeField]
    private float moveSpeed;    //移动速度
    [SerializeField]
    private float floatingSpeed,floatingTopBound,floatingBottomBound;   //浮动参数
    [SerializeField]
    private float xNum;     //x轴的实体数量
    [SerializeField]
    private Vector3 forwardVector;      //移动朝向（xyz）
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
            typeof(MoveSpeedComponent),
            typeof(FloatingComponent),
            typeof(Translation),
            typeof(Rotation),
            typeof(MoveForwardComponent),
            typeof(RenderMesh),
            typeof(LocalToWorld)
            );

        //创建实体数组
        NativeArray<Entity> entityArr = new NativeArray<Entity>(amount,Allocator.Persistent);

        //将实体原型和实体数组结合
        entityManager.CreateEntity(entityArchetype, entityArr);

        //创建实体的位置
        float positionX = 0f;
        float positionZ = 0f;
        //初始化每个实体对象
        for(int i=0;i<entityArr.Length;i++)
        {
            entityManager.SetComponentData(entityArr[i], new MoveSpeedComponent { value = moveSpeed });
            float3 pos = new float3(
                position.x+positionX - xNum / 2, 
                position.y+noise.cnoise(new float2(positionX, 1f) * UnityEngine.Random.Range(0f, 1f)),
                position.z+positionZ);
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
            entityManager.SetSharedComponentData(entityArr[i], new RenderMesh
            {
                mesh = _mesh, material = _material
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
    }
}
