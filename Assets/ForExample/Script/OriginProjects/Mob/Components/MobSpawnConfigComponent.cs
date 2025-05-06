using Unity.Entities;
using Unity.Mathematics;

namespace OriginProject.Mob
{
    // 스폰 설정을 위한 싱글톤 컴포넌트
    public struct MobSpawnConfigComponent : IComponentData
    {
        public Entity mobPrefab;        // 스폰할 몬스터 프리팹
        public float spawnInterval;     // 스폰 간격
        public float nextSpawnTime;     // 다음 스폰 시간
        public float spawnRadius;       // 스폰 반경
        public float minSpawnDistance;  // 플레이어로부터의 최소 스폰 거리
        public int maxMobCount;         // 최대 몬스터 수
        public Random random;           // 랜덤 생성기
    }
} 