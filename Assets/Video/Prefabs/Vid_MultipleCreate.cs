using UnityEditor.SceneManagement;
using UnityEngine;

public class Vid_MultipleCreate : MonoBehaviour
{
    public bool allowMultipleCreate = true; // Flag to indicate if multiple spawns are allowed

    [SerializeField]
    public GameObject prefabs; // Prefab to spawn
    public int count = 5; // Number of objects to spawn
    public float totalAngle = 60.0f; // Angle of rotation



    private void OnDestroy()
    {
        if (prefabs == null) return;

        if (allowMultipleCreate)
        {
            float startAngle = -totalAngle / 2f;
            float angleStep = (count == 1) ? 0f : totalAngle / (count - 1);

            for (int i = 0; i < count; i++)
            {

                float angle = startAngle + angleStep * i;

                Quaternion rotation = Quaternion.Euler(0, angle, 0) * transform.rotation;

                Instantiate(prefabs, transform.position, rotation);

            }
        }
    }
}
