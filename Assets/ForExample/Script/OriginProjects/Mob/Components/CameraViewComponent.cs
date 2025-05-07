using Unity.Entities;
using Unity.Mathematics;

namespace OriginProject.Mob
{
    // 카메라 시야 정보를 저장하는 싱글톤 컴포넌트
    public struct CameraViewComponent : IComponentData
    {
        public float3 position;         // 카메라 위치
        public float3 forward;          // 카메라 전방 벡터
        public float viewDistance;      // 시야 거리
        public float aspectRatio;       // 화면 비율
        public float orthographicSize;  // 오소그래픽 카메라 크기
    }
} 