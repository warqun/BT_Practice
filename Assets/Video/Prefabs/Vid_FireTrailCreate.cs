using UnityEngine;

public class Vid_ProjectileMoveFireTrail : MonoBehaviour
{
    float starttime = 0.0f; // 타이머 변수
    int fireTrailCount = 0; // Number of fire trails spawned

    [SerializeField]
    public GameObject fireTrailPrefab; // Prefab for the fire trail
    public float SpawnInterval = 0.1f; // Time interval between fire trail spawns


    private void Awake()
    {
        starttime = Time.time; // Initialize the start time
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > starttime + SpawnInterval * fireTrailCount) // Check if it's time to spawn a new fire trail
        {
            Vector3 spawnPosition = transform.position; // Calculate the spawn position
            spawnPosition.y = 0; // Set the y-coordinate to 0

            Instantiate(fireTrailPrefab, spawnPosition, Quaternion.identity); // Spawn a new fire trail
            fireTrailCount++; // Increment the count of spawned fire trails
        }
    }
}
