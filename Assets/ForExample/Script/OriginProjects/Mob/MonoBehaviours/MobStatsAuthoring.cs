using Unity.Entities;
using UnityEngine;

namespace OriginProject.Mob
{
    public class MobStatsAuthoring : MonoBehaviour
    {
        public float maxHealth = 100f;
        public float defense = 10f;
        public float attackDamage = 20f;
        public float moveSpeed = 5f;

        class Baker : Baker<MobStatsAuthoring>
        {
            public override void Bake(MobStatsAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                
                AddComponent(entity, new MobStatsComponent
                {
                    maxHealth = authoring.maxHealth,
                    currentHealth = authoring.maxHealth,
                    defense = authoring.defense,
                    attackDamage = authoring.attackDamage,
                    moveSpeed = authoring.moveSpeed,
                    isAlive = true
                });
            }
        }
    }
} 