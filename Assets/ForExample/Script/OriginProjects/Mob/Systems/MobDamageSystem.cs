using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace OriginProject.Mob
{
    [BurstCompile]
    [UpdateInGroup(typeof(MobSystemGroup))]
    [UpdateAfter(typeof(MobAttackSystem))]
    public partial class MobDamageSystem : SystemBase
    {
        protected override void OnCreate()
        {
            RequireForUpdate<MobStatsComponent>();
            RequireForUpdate<MobStateComponent>();
        }

        protected override void OnUpdate()
        {
            var deltaTime = SystemAPI.Time.DeltaTime;

            foreach (var (stats, stateComp) in 
                SystemAPI.Query<RefRW<MobStatsComponent>, RefRW<MobStateComponent>>())
            {
                // 체력이 0 이하면 사망 상태로 변경
                if (stats.ValueRO.currentHealth <= 0)
                {
                    stats.ValueRW.isAlive = false;
                    stateComp.ValueRW.currentState = MobState.Dead;
                    continue;
                }

                // 스턴 상태 처리
                if (stateComp.ValueRO.currentState == MobState.Stun)
                {
                    stateComp.ValueRW.stateTimer -= deltaTime;
                    if (stateComp.ValueRO.stateTimer <= 0)
                    {
                        stateComp.ValueRW.currentState = MobState.Idle;
                    }
                }
            }
        }
    }
} 