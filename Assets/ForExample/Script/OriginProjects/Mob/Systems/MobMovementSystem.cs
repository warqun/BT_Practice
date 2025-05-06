using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace OriginProject.Mob
{
    [BurstCompile]
    [UpdateInGroup(typeof(MobSystemGroup))]
    [UpdateAfter(typeof(MobSpawnSystem))]
    public partial class MobMovementSystem : SystemBase
    {
        protected override void OnCreate()
        {
            RequireForUpdate<MobStatsComponent>();
            RequireForUpdate<MobStateComponent>();
            RequireForUpdate<PlayerTagComponent>();
        }

        protected override void OnUpdate()
        {
            var playerPosition = SystemAPI.GetSingleton<PlayerTagComponent>().Position;
            var deltaTime = SystemAPI.Time.DeltaTime;

            foreach (var (transform, stats, mobState) in 
                SystemAPI.Query<RefRW<LocalTransform>, RefRO<MobStatsComponent>, RefRO<MobStateComponent>>()
                .WithAll<MobStatsComponent>())
            {
                if (!stats.ValueRO.isAlive || mobState.ValueRO.currentState == MobState.Stun)
                    continue;

                // 플레이어 방향으로 이동
                float3 toPlayer = playerPosition - transform.ValueRO.Position;
                float3 direction = math.normalize(toPlayer);
                float moveSpeed = stats.ValueRO.moveSpeed;

                // 이동
                transform.ValueRW.Position += direction * moveSpeed * deltaTime;

                // 회전
                if (math.lengthsq(toPlayer) > 0.01f)
                {
                    quaternion targetRotation = quaternion.LookRotation(direction, math.up());
                    transform.ValueRW.Rotation = math.slerp(transform.ValueRO.Rotation, targetRotation, deltaTime * 5f);
                }
            }
        }
    }
} 