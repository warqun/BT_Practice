using UnityEngine;

namespace OriginProject.Mob
{
    /// <summary>
    /// 메인 Scene의 플레이어 카메라에 부착되는 컴포넌트로,
    /// 카메라 설정을 오소그래픽 모드로 유지하는 역할을 합니다.
    /// SubScene의 CameraViewAuthoring은 이 카메라를 런타임에 자동으로 찾습니다.
    /// </summary>
    public class CameraViewBridge : MonoBehaviour
    {
        // 오소그래픽 크기 설정
        [Tooltip("카메라의 오소그래픽 크기")]
        public float orthographicSize = 5f;
        
        // 카메라 컴포넌트 캐싱
        private Camera cameraComponent;

        private void Start()
        {
            // 카메라 컴포넌트 가져오기
            cameraComponent = GetComponent<Camera>();
            
            if (cameraComponent == null)
            {
                Debug.LogError("CameraViewBridge must be attached to a GameObject with a Camera component.");
                return;
            }
            
            // 이 오브젝트가 MainCamera 태그를 가지고 있는지 확인
            if (gameObject.tag != "MainCamera")
            {
                Debug.LogWarning("CameraViewBridge: 이 게임오브젝트는 'MainCamera' 태그를 가지고 있지 않습니다. " +
                               "CameraViewAuthoring이 이 카메라를 찾지 못할 수 있습니다.");
            }
            
            // 카메라를 오소그래픽 모드로 설정
            cameraComponent.orthographic = true;
            cameraComponent.orthographicSize = orthographicSize;
        }

        private void Update()
        {
            // 카메라 속성 업데이트
            if (cameraComponent != null)
            {
                // 항상 오소그래픽 모드 유지
                if (!cameraComponent.orthographic)
                {
                    cameraComponent.orthographic = true;
                }
                
                // orthographicSize 업데이트 (인스펙터에서 변경한 경우)
                if (cameraComponent.orthographicSize != orthographicSize)
                {
                    cameraComponent.orthographicSize = orthographicSize;
                }
            }
        }
    }
} 