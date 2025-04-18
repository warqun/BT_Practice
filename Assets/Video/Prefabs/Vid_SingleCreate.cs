using UnityEngine;

public class Vid_SingleCreate : MonoBehaviour
{
    [SerializeField]
    public GameObject prefabs; // Prefab to spawn

    private void OnDestroy()
    {
        if (prefabs == null) return;
        Instantiate(prefabs, transform.position, Quaternion.identity);
    }
}

