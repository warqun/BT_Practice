using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Collections;
using Unity.Burst;

[BurstCompile]
public partial struct SeparationSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var entityQuery = SystemAPI.QueryBuilder().WithAll<LocalTransform, SeparationComponent>().Build();
        var entities = entityQuery.ToEntityArray(Allocator.Temp);
        var transforms = entityQuery.ToComponentDataArray<LocalTransform>(Allocator.Temp);
        var separations = entityQuery.ToComponentDataArray<SeparationComponent>(Allocator.Temp);

        for (int i = 0; i < entities.Length; i++)
        {
            float3 push = float3.zero;
            int count = 0;
            float3 myPos = transforms[i].Position;
            float radius = separations[i].separationRadius;
            float strength = separations[i].separationStrength;

            for (int j = 0; j < entities.Length; j++)
            {
                if (i == j) continue;
                float3 otherPos = transforms[j].Position;
                float dist = math.distance(myPos, otherPos);
                if (dist < radius && dist > 0.01f)
                {
                    push += (myPos - otherPos) / dist;
                    count++;
                }
            }

            if (count > 0)
            {
                float3 offset = push / count * strength;
                transforms[i] = LocalTransform.FromPosition(myPos + offset);
            }
        }

        for (int i = 0; i < entities.Length; i++)
        {
            SystemAPI.SetComponent(entities[i], transforms[i]);
        }

        entities.Dispose();
        transforms.Dispose();
        separations.Dispose();
    }
}
