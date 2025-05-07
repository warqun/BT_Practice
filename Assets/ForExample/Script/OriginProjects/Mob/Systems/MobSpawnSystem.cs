using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace OriginProject.Mob
{
    [BurstCompile]
    [UpdateInGroup(typeof(MobSystemGroup), OrderFirst = true)]
    public partial class MobSpawnSystem : SystemBase
    {
        protected override void OnCreate()
        {
            RequireForUpdate<MobSpawnConfigComponent>();
            RequireForUpdate<CameraViewComponent>();
            RequireForUpdate<PlayerTagComponent>();
            Debug.Log("[MobSpawnSystem] 시스템이 생성되었습니다.");
        }

        protected override void OnUpdate()
        {
            var spawnConfig = SystemAPI.GetSingleton<MobSpawnConfigComponent>();
            var cameraView = SystemAPI.GetSingleton<CameraViewComponent>();
            var playerPosition = SystemAPI.GetSingleton<PlayerTagComponent>().Position;
            
            // 현재 시간 체크
            float currentTime = (float)SystemAPI.Time.ElapsedTime;
            if (currentTime < spawnConfig.nextSpawnTime)
            {
                Debug.Log($"[MobSpawnSystem] 다음 스폰 시간까지 대기 중입니다. 현재: {currentTime}, 다음 스폰: {spawnConfig.nextSpawnTime}");
                return;
            }

            // 현재 몬스터 수 체크
            int currentMobCount = SystemAPI.QueryBuilder().WithAll<MobStatsComponent>().Build().CalculateEntityCount();
            if (currentMobCount >= spawnConfig.maxMobCount)
            {
                Debug.Log($"[MobSpawnSystem] 최대 몬스터 수에 도달했습니다. 현재: {currentMobCount}, 최대: {spawnConfig.maxMobCount}");
                return;
            }

            // 스폰 위치 결정
            float3 spawnPosition;
            bool validPosition = false;
            int maxAttempts = 10;
            int attempts = 0;

            do
            {
                // 랜덤한 각도와 거리로 위치 생성
                float angle = spawnConfig.random.NextFloat(0, math.PI * 2);
                float distance = spawnConfig.random.NextFloat(spawnConfig.minSpawnDistance, spawnConfig.spawnRadius);
                
                float2 offset = new float2(math.cos(angle), math.sin(angle)) * distance;
                spawnPosition = new float3(playerPosition.x + offset.x, 0, playerPosition.z + offset.y);

                // 카메라 시야 밖인지 확인
                validPosition = IsOutsideCameraView(spawnPosition, cameraView);
                attempts++;

            } while (!validPosition && attempts < maxAttempts);

            if (validPosition)
            {
                Debug.Log($"[MobSpawnSystem] 몬스터를 생성합니다. 위치: {spawnPosition}");
                
                // 몬스터 생성
                Entity newMob = EntityManager.Instantiate(spawnConfig.mobPrefab);
                
                // 위치 설정
                var transform = EntityManager.GetComponentData<LocalTransform>(newMob);
                transform.Position = spawnPosition;
                EntityManager.SetComponentData(newMob, transform);

                // 다음 스폰 시간 설정
                var configEntity = SystemAPI.GetSingletonEntity<MobSpawnConfigComponent>();
                var config = EntityManager.GetComponentData<MobSpawnConfigComponent>(configEntity);
                config.nextSpawnTime = currentTime + spawnConfig.spawnInterval;
                EntityManager.SetComponentData(configEntity, config);
            }
            else
            {
                Debug.LogWarning("[MobSpawnSystem] 유효한 스폰 위치를 찾을 수 없습니다.");
            }
        }

        private bool IsOutsideCameraView(float3 position, CameraViewComponent camera)
        {
            // 카메라와의 거리 체크
            float3 dirToPosition = position - camera.position;
            float distance = math.length(dirToPosition);
            
            // 시야 거리 밖이면 true
            if (distance > camera.viewDistance)
                return true;

            // 오소그래픽 카메라의 경우 2D 평면상의 범위 계산
            // 카메라 forward 벡터를 기준으로 플레이어가 보는 방향의 평면 좌표계로 변환
            float3 right = math.cross(camera.forward, new float3(0, 1, 0));
            float3 up = math.cross(right, camera.forward);
            
            right = math.normalize(right);
            up = math.normalize(up);
            
            // 위치를 카메라 좌표계로 변환
            float rightDot = math.dot(dirToPosition, right);
            float upDot = math.dot(dirToPosition, up);
            
            // 오소그래픽 사이즈와 화면 비율로 카메라의 가시 범위 계산
            float horizontalSize = camera.orthographicSize * camera.aspectRatio;
            float verticalSize = camera.orthographicSize;
            
            // 가시 범위 밖이면 true
            return math.abs(rightDot) > horizontalSize || math.abs(upDot) > verticalSize;
        }
    }
} 