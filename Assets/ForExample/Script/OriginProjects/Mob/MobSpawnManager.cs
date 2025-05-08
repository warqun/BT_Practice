using System.Collections;
using UnityEngine;

public class MobSpawnManager : MonoBehaviour
{
    [SerializeField]
    public GameObject[] mobPrefab; // array of mob prefabs
    public float spawnInterval = 0.5f; // interval between spawns

    Transform[] spawnPoints; // spawn points for mobs
    MobSpawner[] mobSpawners; // array of mob spawners
    GameObject objManager;
    float spawnTimer = 0;

    [System.Obsolete]
    private void Awake()
    {
        objManager = FindObjectOfType<ObjectManager>().gameObject;
        // Initialize spawn points
        spawnPoints = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            spawnPoints[i] = transform.GetChild(i);
        }
        // Initialize mob spawners
        mobSpawners = new MobSpawner[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            mobSpawners[i] = transform.GetChild(i).GetComponent<MobSpawner>();
        }
    }

    void Update()
    {
        spawnTimer += Time.deltaTime; // increment spawn timer

        if (spawnTimer >= spawnInterval)
        {
            StartCoroutine(SpawnMob()); // Start the SpawnMob coroutine
            spawnTimer = 0; // Reset the spawn timer
        }
    }

    IEnumerator SpawnMob()
    {
        // Loop through each spawn point
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            // Get the MobSpawner component for the current spawn point
            MobSpawner spawner = mobSpawners[i];

            // Calculate a random position within the spawner's range
            Vector3 randomPosition = new Vector3(
                Random.Range(-spawner.sizeX / 2f, spawner.sizeX / 2f),
                0, // Assuming mobs spawn on the ground (Y-axis is 0)
                Random.Range(-spawner.sizeZ / 2f, spawner.sizeZ / 2f)
            );

            // Offset the random position by the spawn point's position
            Vector3 spawnPosition = spawnPoints[i].position + randomPosition;

            // Instantiate a random mob prefab at the spawn point
            GameObject mob = Instantiate(mobPrefab[Random.Range(0, mobPrefab.Length)], spawnPosition, Quaternion.identity);

            // Set the parent of the mob to the spawner
            mob.transform.SetParent(objManager.transform);

            // Wait for the specified interval before spawning the next mob
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
