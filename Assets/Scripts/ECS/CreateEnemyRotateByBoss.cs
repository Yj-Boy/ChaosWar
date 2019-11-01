/*
 *  创建围绕Boss旋转的大量实体
 *  
 *  由MoveSpeedComponent、RotateComponent、FloatingComponent组合实现
 */
using Unity.Collections;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Rendering;

public class CreateEnemyRotateByBoss : MonoBehaviour
{
    [SerializeField]
    private int amount;                 //实体总数
    [SerializeField]
    private float3 position;            //生成实体中心点位置
    private float3 pos;                 //生成实体的实际位置
    [SerializeField]
    private float radius;               //生成实体半径范围
    [SerializeField]
    private float rotateSpeed;          //旋转速度
    private float rotateAngle;          //累计旋转角度
    private int duration;               //旋转方向
    [SerializeField]
    private float moveSpeed;            //移动速度
    [SerializeField]
    private float floatingSpeed;        //浮动参数
    [SerializeField]
    private float floatingTopBound;     //浮动参数
    [SerializeField]
    private float floatingBottomBound;  //浮动参数 
    [SerializeField]
    private Mesh _mesh;                 //实体渲染网格
    [SerializeField]
    private Material _material;         //实体渲染材质

    EntityManager entityManager;       //实体管理对象，用于创建实体原型（Archetype）和实体（Entity）

    private void Start()
    {
        //初始化实体管理对象
        entityManager = World.Active.EntityManager;

        //创建并初始化实体原型
        EntityArchetype entityArchetype = entityManager.CreateArchetype(
            typeof(Translation),
            typeof(Rotation),
            typeof(MoveSpeedComponent),
            typeof(MoveForwardComponent),
            typeof(RotateComponent),
            typeof(FloatingComponent),
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
            /*
             * 求圆上点公式
             * x1 = x0 + r * cos(angle * PI / 180)
             * y1 = y0 + r * sin(angle * PI / 180)    
             */
            //int x = UnityEngine.Random.Range(0, 360);
            //int z = UnityEngine.Random.Range(0, 360);

            //计算已radius为半径的实体位置
            float r;
            do
            {
                r = UnityEngine.Random.Range(-radius, radius);
            } while (r > -2 && r < 2);
            

            pos.x= position.x + r * math.cos(UnityEngine.Random.Range(0, 360) * math.PI / 180);
            pos.y = position.y+UnityEngine.Random.Range(-radius, radius);
            pos.z = position.z + r * math.sin(UnityEngine.Random.Range(0, 360) * math.PI / 180);

            entityManager.SetComponentData(entityArr[i], new Translation
            {
                Value = pos
            });

            entityManager.SetComponentData(entityArr[i], new MoveSpeedComponent
            {
                value = moveSpeed
            });

            duration = UnityEngine.Random.Range(-1, 1);
            //计算实体朝向中心点的角度（通过两个向量求夹角再转化成角度）
            rotateAngle = math.abs(Vector2.Angle(new Vector2(pos.x, pos.z), new Vector2(1, 0)));
            //转化成已radiu为半径的圆的切线角度（这时实体的朝向就是切线的朝向）
            rotateAngle = 180 - rotateAngle;
            //rotateSpeed = rotateSpeed * r;//(bug:在循环里每次相乘，无限大)
            //float v = 360 / ((2 * math.PI * r) / 3000);
            entityManager.SetComponentData(entityArr[i], new RotateComponent
            {
                speed = rotateSpeed*r,
                angle = rotateAngle,
                initAngle = rotateAngle,
                duration = duration
            });

            entityManager.SetComponentData(entityArr[i], new FloatingComponent
            {
                speed = UnityEngine.Random.Range(0,floatingSpeed),
                topBound = floatingTopBound,
                bottomBound = floatingBottomBound,
                floatingStartPosY = pos.y
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
