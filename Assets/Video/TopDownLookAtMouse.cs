using UnityEngine;

public class TopDownLookAtMouse : MonoBehaviour
{
    public float rotationSpeed = 10f;

    void Update()
    {
        LookAtMouse();
    }

    Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        float rayDistance;
        if (groundPlane.Raycast(ray, out rayDistance))
        {
            return ray.GetPoint(rayDistance);
        }

        return transform.position;
    }

    void LookAtMouse()
    {
        Vector3 targetPos = GetMouseWorldPosition();
        Vector3 direction = targetPos - transform.position;
        direction.y = 0;

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
}
