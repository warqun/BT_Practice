using Unity.Entities;

public struct SeparationComponent : IComponentData
{
    public float separationRadius;
    public float separationStrength;
    public float nextUpdateTime;
    public float updateInterval;
}