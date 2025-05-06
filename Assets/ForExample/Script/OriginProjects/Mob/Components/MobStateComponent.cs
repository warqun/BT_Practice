using Unity.Entities;

namespace OriginProject.Mob
{
    public struct MobStateComponent : IComponentData
    {
        public MobState currentState;
        public float stateTimer;
        public float attackCooldownTimer;
        public float lastAttackTime;
        public float attackRange;
        public float stunDuration;
    }
} 