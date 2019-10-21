/*
 *  浮动组件
 */
using Unity.Entities;

public struct FloatingComponent : IComponentData
{
    public float speed;     //浮动的速度
    public float topBound;      //浮动的上限
    public float bottomBound;   //浮动的下限
    public float floatingStartPosY;     //浮动的Y轴初始位置
}
