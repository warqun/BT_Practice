using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

namespace OriginProject.Mob
{
    public class MobSpawnAuthoring : MonoBehaviour
    {
        public GameObject mobPrefab;
        public float spawnInterval = 5f;
        public float spawnRadius = 20f;
        public float minSpawnDistance = 10f;
        public int maxMobCount = 10;
        public uint randomSeed = 1;
        
        class Baker : Baker<MobSpawnAuthoring>
        {
            public override void Bake(MobSpawnAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                
                AddComponent(entity, new MobSpawnConfigComponent
                {
                    mobPrefab = GetEntity(authoring.mobPrefab, TransformUsageFlags.Dynamic),
                    spawnInterval = authoring.spawnInterval,
                    nextSpawnTime = 0f,
                    spawnRadius = authoring.spawnRadius,
                    minSpawnDistance = authoring.minSpawnDistance,
                    maxMobCount = authoring.maxMobCount,
                    random = new Unity.Mathematics.Random(authoring.randomSeed)
                });
            }
        }
    }
} 