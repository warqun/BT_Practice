using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // 추적할 오브젝트
    public float smoothSpeed = 0.125f; // Lerp 속도
    public Vector3 offset; // 카메라 오프셋

    private Quaternion initialRotation; // 초기 회전값 저장

    void Start()
    {

    }

    void LateUpdate()
    {
        if (target == null)
            return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // 타겟을 바라보도록 설정
        transform.LookAt(target);
    }
}
