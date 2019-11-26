/*
 *  实体自身旋转组件
 */
using Unity.Entities;

public struct RotateComponent : IComponentData
{
    public float speed;     //旋转速度
    public float angle;     //累计旋转角度
    public float initAngle;     //初始旋转角度
    public int duration;    //旋转方向(小于0为逆时针旋转，大于等于0为顺时针旋转)
}
