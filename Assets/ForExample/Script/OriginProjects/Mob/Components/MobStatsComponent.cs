using Unity.Entities;

namespace OriginProject.Mob
{
    public struct MobStatsComponent : IComponentData
    {
        public float maxHealth;
        public float currentHealth;
        public float defense;
        public float attackDamage;
        public float moveSpeed;
        public bool isAlive;
    }
} 