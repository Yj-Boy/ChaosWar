using Unity.Entities;

public struct FloatingComponent : IComponentData
{
    public float speed;
    public float topBound;
    public float bottomBound;
    public float floatingStartPosY;
}
