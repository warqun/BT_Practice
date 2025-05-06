using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace OriginProject.Mob
{
    [BurstCompile]
    [UpdateInGroup(typeof(MobSystemGroup))]
    [UpdateAfter(typeof(MobMovementSystem))]
    public partial class MobAttackSystem : SystemBase
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
                SystemAPI.Query<RefRO<LocalTransform>, RefRO<MobStatsComponent>, RefRW<MobStateComponent>>()
                .WithAll<MobStatsComponent>())
            {
                if (!stats.ValueRO.isAlive)
                    continue;

                // 플레이어와의 거리 계산
                float3 toPlayer = playerPosition - transform.ValueRO.Position;
                float distanceToPlayer = math.length(toPlayer);

                // 공격 범위 내에 있고 공격 쿨다운이 끝났으면 공격
                if (distanceToPlayer <= mobState.ValueRO.attackRange)
                {
                    if (mobState.ValueRO.attackCooldownTimer <= 0)
                    {
                        // 공격 실행
                        mobState.ValueRW.currentState = MobState.Attack;
                        mobState.ValueRW.attackCooldownTimer = mobState.ValueRO.lastAttackTime;
                        mobState.ValueRW.lastAttackTime = (float)SystemAPI.Time.ElapsedTime;
                    }
                }
                else
                {
                    // 공격 범위 밖이면 추적 상태로 변경
                    mobState.ValueRW.currentState = MobState.Chase;
                }

                // 공격 쿨다운 감소
                if (mobState.ValueRO.attackCooldownTimer > 0)
                {
                    mobState.ValueRW.attackCooldownTimer -= deltaTime;
                }
            }
        }
    }

    // 데미지 이벤트를 처리하기 위한 컴포넌트
    public struct DamageEventComponent : IComponentData
    {
        public Entity Target;
        public float Damage;
        public Entity Source;
    }
} 