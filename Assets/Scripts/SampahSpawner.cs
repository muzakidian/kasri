using System.Collections;
using UnityEngine;

public class SampahSpawner : MonoBehaviour
{
    public GameObject[] objectsToSpawn; // The objects to spawn.
    public BoxCollider2D spawnArea;     // The area to spawn the objects in.
    public int maxObjects = 10;         // Maximum number of objects to spawn.
    public float spawnInterval = 1.0f;  // Time in seconds between each spawn.

    private int objectsSpawned = 0;     // Number of objects spawned so far.

    void Start()
    {
        // Start spawning objects.
        StartCoroutine(SpawnRandomObjects());
    }

    IEnumerator SpawnRandomObjects()
    {
        while (objectsSpawned < maxObjects)
        {
            SpawnRandomObject();

            // Wait for the spawn interval before spawning the next object.
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnRandomObject()
    {
        // Select a random object.
        GameObject randomObject = objectsToSpawn[Random.Range(0, objectsToSpawn.Length)];

        // Generate a random position within the BoxCollider2D bounds.
        Vector2 randomPosition = new Vector2(
            Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x),
            Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y)
        );

        // Instantiate the object at the random position.
        Instantiate(randomObject, randomPosition, Quaternion.identity);

        // Increment the count of objects spawned.
        objectsSpawned++;
    }
}
