/*
 *  围绕某个点旋转组件
 */
using Unity.Entities;
using Unity.Mathematics;

public struct RotateByOnePointComponent : IComponentData
{
    public float3 point;    //围绕旋转的中心点
    public float speed;     //旋转速度
    public float angle;     //旋转累计角度
    public float radius;    //旋转半径
    public float duration;
}
