using Unity.Entities;
using UnityEngine;

namespace OriginProject.Mob
{
    /// <summary>
    /// 몬스터 스폰 영역을 제어하기 위한 카메라 시야 설정
    /// SubScene에 배치되며 카메라 설정을 직접 변경하지 않습니다.
    /// </summary>
    public class CameraViewAuthoring : MonoBehaviour
    {
        [Tooltip("시야 거리 - 이 거리 밖에서 몬스터가 스폰됩니다")]
        public float viewDistance = 30f;
        
        [Tooltip("런타임에 찾을 카메라 태그")]
        public string cameraTag = "MainCamera";
        
        // 런타임에 찾은 카메라 캐싱
        private Camera runtimeCamera;

        class Baker : Baker<CameraViewAuthoring>
        {
            public override void Bake(CameraViewAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                
                // 베이킹 시점에는 카메라가 없으므로 기본값 사용
                AddComponent(entity, new CameraViewComponent
                {
                    position = authoring.transform.position,
                    forward = authoring.transform.forward,
                    viewDistance = authoring.viewDistance,
                    aspectRatio = 1.7777f, // 16:9 기본값 (런타임에 업데이트됨)
                    orthographicSize = 5f   // 기본값 (런타임에 업데이트됨)
                });
            }
        }

        private void Update()
        {
            // 런타임에 카메라 찾기
            if (runtimeCamera == null)
            {
                GameObject cameraObj = GameObject.FindWithTag(cameraTag);
                if (cameraObj != null)
                {
                    runtimeCamera = cameraObj.GetComponent<Camera>();
                    
                    // 카메라의 브릿지 컴포넌트 확인
                    var bridge = cameraObj.GetComponent<CameraViewBridge>();
                    if (bridge == null)
                    {
                        Debug.LogWarning($"카메라 '{cameraObj.name}'에 CameraViewBridge 컴포넌트가 없습니다. 몬스터 스폰 시스템이 정상적으로 작동하지 않을 수 있습니다.");
                    }
                }
                
                if (runtimeCamera == null)
                {
                    // 태그로 찾지 못했으면 메인 카메라 사용
                    runtimeCamera = Camera.main;
                    if (runtimeCamera == null)
                    {
                        return; // 카메라를 찾지 못함
                    }
                }
            }
            
            // ECS 컴포넌트 업데이트
            if (World.DefaultGameObjectInjectionWorld != null)
            {
                var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
                var query = entityManager.CreateEntityQuery(typeof(CameraViewComponent));
                
                if (query.CalculateEntityCount() > 0)
                {
                    var entity = query.GetSingletonEntity();
                    entityManager.SetComponentData(entity, new CameraViewComponent
                    {
                        position = transform.position,
                        forward = transform.forward,
                        viewDistance = viewDistance,
                        aspectRatio = runtimeCamera.aspect,
                        orthographicSize = runtimeCamera.orthographicSize
                    });
                }
            }
        }
    }
} 