using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;
using UnityEngine;

[BurstCompile]
public partial struct SpawnerSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);
        foreach (var (spawner, entity) in SystemAPI.Query<RefRW<SpawnerComponent>>().WithEntityAccess())
        {
            spawner.ValueRW.timer += SystemAPI.Time.DeltaTime;
            if (spawner.ValueRW.timer >= spawner.ValueRW.interval)
            {
                spawner.ValueRW.timer = 0f;
                var position = GetRandomPosition(spawner.ValueRW.areaCenter, spawner.ValueRW.areaSize, spawner.ValueRW.fixedY);
                var spawned = ecb.Instantiate(spawner.ValueRW.prefab);
                ecb.SetComponent(spawned, LocalTransform.FromPosition(position));
            }
        }
        ecb.Playback(state.EntityManager);
    }

    private float3 GetRandomPosition(float3 center, float3 size, float fixedY)
    {
        var random = Unity.Mathematics.Random.CreateFromIndex((uint)UnityEngine.Random.Range(1, 9999999));
        float3 halfSize = size * 0.5f;
        float3 pos = center + random.NextFloat3(-halfSize, halfSize);
        pos.y = fixedY; // Y√‡ ∞Ì¡§
        return pos;
    }
}