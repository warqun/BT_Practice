using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Burst;

[BurstCompile]
public partial struct NormalMobMoveSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        float3 playerPos = PlayerPositionBroadcaster.Position;

        float deltaTime = SystemAPI.Time.DeltaTime;
        

        foreach (var (move, transform) in
                 SystemAPI.Query<RefRO<NormalMobMoveComponent>, RefRW<LocalTransform>>())
        {
            float3 mobPos = transform.ValueRW.Position;
            float3 dir = math.normalizesafe(playerPos - mobPos);

            mobPos += dir * move.ValueRO.moveSpeed * deltaTime;
            transform.ValueRW.Position = mobPos;
        }
    }
}