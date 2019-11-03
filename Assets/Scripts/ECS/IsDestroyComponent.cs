/*
 *  是否销毁组件
 *  
 *  用于判断是否销毁实体
 */
using Unity.Entities;

public struct IsDestroyComponent : IComponentData
{
    public bool value;
}
