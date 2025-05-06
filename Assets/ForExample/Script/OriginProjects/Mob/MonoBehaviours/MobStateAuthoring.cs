using Unity.Entities;
using UnityEngine;

namespace OriginProject.Mob
{
    public class MobStateAuthoring : MonoBehaviour
    {
        public float attackRange = 2f;
        public float attackCooldown = 1f;
        public float stunDuration = 1f;

        class Baker : Baker<MobStateAuthoring>
        {
            public override void Bake(MobStateAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                
                AddComponent(entity, new MobStateComponent
                {
                    currentState = MobState.Idle,
                    stateTimer = 0f,
                    attackCooldownTimer = authoring.attackCooldown,
                    lastAttackTime = 0f,
                    attackRange = authoring.attackRange,
                    stunDuration = authoring.stunDuration
                });
            }
        }
    }
} 