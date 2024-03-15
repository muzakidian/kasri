using System.Collections;
using UnityEngine;

public class SampahSpawner : MonoBehaviour
{
    public GameObject[] objectsToSpawn; 
    public BoxCollider2D spawnArea;    
    public int maxObjects = 10;  
    public float spawnInterval = 1.0f;

    private int objectsSpawned = 0;   

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
