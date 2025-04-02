using Unity.Entities;
using Unity.Mathematics;

public struct SpawnerComponent : IComponentData
{
    public Entity prefab;
    public float3 areaCenter;
    public float3 areaSize;
    public float interval;
    public float timer;
    public float fixedY;
}