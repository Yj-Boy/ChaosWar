/*
 *  创建百万陨石
 */
using Unity.Collections;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Rendering;

public class CreateAsteroid : MonoBehaviour
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
    //private float xDistance;    //每个实体x轴上的间距
    //[SerializeField]
    //private float yDistance;    //每个实体y轴上的间距
    //[SerializeField]
    //private float zDistance;    //每个实体z轴上的间距
    //[SerializeField]
    //private int eachXAmount;    //每一行的实体数量
    private float3 position;    
    //private float positionZtmp;     //生成实体位置的z轴初始值
    [SerializeField]
    private Mesh _mesh_01;     //渲染实体的网格
    [SerializeField]
    private Material _material_01;     //渲染实体的材质
    [SerializeField]
    private Mesh _mesh_02;     //渲染实体的网格
    [SerializeField]
    private Material _material_02;     //渲染实体的材质
    [SerializeField]
    private Mesh _mesh_03;     //渲染实体的网格
    [SerializeField]
    private Material _material_03;     //渲染实体的材质

    EntityManager entityManager;    //实体管理对象，用于创建实体原型（Archetype）和实体（Entity）

    public Transform spawnTransform;   //生成实体的位置

    private void Start()
    {
        //相关变量初始化
        position = spawnTransform.position;
        //positionZtmp = position.z;
        //xDistance = position.x;
        Debug.Log("position:" + position);

        //初始化实体管理对象
        entityManager = World.Active.EntityManager;

        //创建并定义实体原型
        EntityArchetype entityArchetype = entityManager.CreateArchetype(
            typeof(FloatingComponent),
            typeof(RotateComponent),
            typeof(Translation),
            typeof(Rotation),
            typeof(Scale),
            typeof(MeshRenderer),
            typeof(LocalToWorld),
            typeof(IsDestroyComponent)
            );
        
        //创建实体数组
        NativeArray<Entity> entityArr = new NativeArray<Entity>(amount, Allocator.Persistent);

        //将实体原型和实体数组结合
        entityManager.CreateEntity(entityArchetype, entityArr);

        //对每一个实体进行初始化
        for (int i=0;i<entityArr.Length; i++)
        {
            float3 pos = new float3(
            //position.x - eachXAmount / 2+UnityEngine.Random.Range(-1f,1f),
            //position.y + noise.cnoise(new float2(position.x, 1f) * UnityEngine.Random.Range(0f, 1f)),
            //position.z + UnityEngine.Random.Range(-2f, 2f));
                position.x + UnityEngine.Random.Range(-140f, 140f),
                position.y + UnityEngine.Random.Range(-20f, 20f),
                position.z + UnityEngine.Random.Range(-20f, 60f));
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
            entityManager.SetComponentData(entityArr[i], new RotateComponent
            {
                speed = UnityEngine.Random.Range(0f, 5f),
                duration = UnityEngine.Random.Range(-1, 1),
                angle=0,
                initAngle=0
            });
            entityManager.SetComponentData(entityArr[i], new Scale
            {
                Value = 0.65f
            });
            entityManager.SetComponentData(entityArr[i], new IsDestroyComponent
            {
                value = false
            });
            int renderMeshNum = UnityEngine.Random.Range(0, 3);
            switch(renderMeshNum)
            {
                case 0:
                    entityManager.AddSharedComponentData(entityArr[i], new RenderMesh
                    {
                        mesh = _mesh_01,
                        material = _material_01
                    });
                    break;
                case 1:
                    entityManager.AddSharedComponentData(entityArr[i], new RenderMesh
                    {
                        mesh = _mesh_02,
                        material = _material_02
                    });
                    break;
                case 2:
                    entityManager.AddSharedComponentData(entityArr[i], new RenderMesh
                    {
                        mesh = _mesh_03,
                        material = _material_03
                    });
                    break;
            }
            
            //position.x++;
            //if (position.x % eachXAmount == 0)
            //{
            //    position.x = xDistance;
            //    position.z += zDistance;
            //    if (position.z >= positionZtmp+(zDistance*3))
            //    {
            //        position.y -= yDistance;
            //        position.z = positionZtmp;
            //    }
            //}         
        }
        //释放实体数组
        entityArr.Dispose();
    }
}
