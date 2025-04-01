using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // ������ ������Ʈ
    public float smoothSpeed = 0.125f; // Lerp �ӵ�
    public Vector3 offset; // ī�޶� ������

    private Quaternion initialRotation; // �ʱ� ȸ���� ����

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

        // Ÿ���� �ٶ󺸵��� ����
        transform.LookAt(target);
    }
}
