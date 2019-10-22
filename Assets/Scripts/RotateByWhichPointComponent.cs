/*
 *  围绕某个点旋转组件
 */
using Unity.Entities;
using Unity.Mathematics;

public struct RotateByWhichPointComponent : IComponentData
{
    public float3 point;
    public float angle;
    public float radius;
}
