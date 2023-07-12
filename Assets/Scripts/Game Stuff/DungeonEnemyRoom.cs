using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonEnemyRoom : DungeonRoom
{
    [Header("Enemy Spawn")]
    public List<EnemySpawn> enemyspawn = new List<EnemySpawn>();
    public int currWave;
    private int waveValue;
    public List<GameObject> enemiesToSpawn = new List<GameObject>();

    public Transform[] spawnLocation;
    public int spawnIndex;

    public int waveDuration;
    private float waveTimer;
    private float spawnInterval;
    private float spawnTimer;
    public List<GameObject> spawnedEnemies = new List<GameObject>();
    public enum Difficulty
    {
        Easy,
        Normal,
        Hard
    }

    public Difficulty difficulty;
    [Header("Dungeon Variable")]
    public Door[] doors;
    private void Start()
    {
        OpenDoors();
        GenerateWave();
        ActivateEnemiesToSpawn();
    }
    void FixedUpdate()
    {
        if (spawnTimer <= 0)
        {
            // Spawn musuh
            if (enemiesToSpawn.Count > 0)
            {
                GameObject enemy = Instantiate(enemiesToSpawn[0], spawnLocation[spawnIndex].position, Quaternion.identity); // Spawn first enemy in our list
                enemiesToSpawn.RemoveAt(0); // And remove it
                spawnedEnemies.Add(enemy);
                spawnTimer = spawnInterval;

                if (spawnIndex + 1 <= spawnLocation.Length - 1)
                {
                    spawnIndex++;
                }
                else
                {
                    spawnIndex = 0;
                }
            }
            else
            {
                waveTimer = 0; // If no enemies remain, end wave
            }
        }
        else
        {
            spawnTimer -= Time.fixedDeltaTime;
            waveTimer -= Time.fixedDeltaTime;
        }
    }
    public void ActivateEnemiesToSpawn()
{
    for (int i = 0; i < enemiesToSpawn.Count; i++)
    {
        enemiesToSpawn[i].SetActive(true);
    }
}
    public void GenerateWave()
    {
        waveValue = currWave * 10;
        GenerateEnemies();

        spawnInterval = waveDuration / enemiesToSpawn.Count; // Gives a fixed time between each enemies
        waveTimer = waveDuration; // Wave duration is read only
        ActivateEnemiesToSpawn();
    }

    public void GenerateEnemies()
    {
        // Create a temporary list of enemies to generate
        List<GameObject> generatedEnemies = new List<GameObject>();

        // Determine jumlah musuh berdasarkan tingkat kesulitan
        int enemyCount = 0;
        switch (difficulty)
        {
            case Difficulty.Easy:
                enemyCount = 10;
                break;
            case Difficulty.Normal:
                enemyCount = 15;
                break;
            case Difficulty.Hard:
                enemyCount = 20;
                break;
        }

        // Generate musuh secara acak
        while (waveValue > 0 && generatedEnemies.Count < enemyCount)
        {
            int randEnemyId = Random.Range(0, enemyspawn.Count);
            int randEnemyCost = enemyspawn[randEnemyId].cost;

            if (waveValue - randEnemyCost >= 0)
            {
                generatedEnemies.Add(enemyspawn[randEnemyId].enemyPrefab);
                waveValue -= randEnemyCost;
                // Tambahkan musuh ke dalam enemies
                Enemy enemyComponent = enemyspawn[randEnemyId].enemyPrefab.GetComponent<Enemy>();
                enemies.Add(enemyComponent);
            }
            else if (waveValue <= 0)
            {
                break;
            }
        }

        enemiesToSpawn.Clear();
        enemiesToSpawn = generatedEnemies;
    }
    public int EnemiesActive()
    {
        int activeEnemies = 0;
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].gameObject.activeInHierarchy)
            {
                activeEnemies++;
            }
        }
        return activeEnemies;
    }
    public void DestroyEnemy(GameObject enemy)
{
    if (spawnedEnemies.Contains(enemy))
    {
        spawnedEnemies.Remove(enemy);
        Destroy(enemy);
        CheckEnemies(); // Panggil fungsi CheckEnemies() setelah menghapus musuh
    }
}

public void CheckEnemies()
{
    if (EnemiesActive() == 0 && spawnedEnemies.Count == 0)
    {
        if (enemiesToSpawn.Count == 0)
        {
            OpenDoors();
        }
        else
        {
            ActivateEnemiesToSpawn();
        }
    }
}
    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            //Activate all enemies and pots
            for (int i = 0; i < enemies.Count; i++)
            {
                ChangeActivation(enemies[i], true);
            }
            for (int i = 0; i < pots.Length; i++)
            {
                ChangeActivation(pots[i], true);
            }
            CloseDoors();
            virtualCamera.SetActive(true);

        }

    }

    public override void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            //Deactivate all enemies and pots
            for (int i = 0; i < enemies.Count; i++)
            {
                ChangeActivation(enemies[i], false);
            }
            for (int i = 0; i < pots.Length; i++)
            {
                ChangeActivation(pots[i], false);
            }
            virtualCamera.SetActive(false);

        }
    }

    public void CloseDoors()
    {
        for (int i = 0; i < doors.Length; i++)
        {
            doors[i].Close();
        }
        Debug.Log("Close Doors");
    }

    public void OpenDoors()
    {
        for (int i = 0; i < doors.Length; i++)
        {
            doors[i].Open();
        }
        Debug.Log("Open Doors");
    }
}
[System.Serializable]
public class EnemySpawn
{
    public GameObject enemyPrefab;
    public int cost;
}