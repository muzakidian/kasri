using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DungeonEnemyRoom : DungeonRoom
{
    // Enemy Spawn
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
    private float cEasy;
    private float cNormal;
    private float cHard;
    private float xEasy;
    private float xNormal;
    private float xHard;
    public Difficulty difficulty;
    private UCB1Algorithm ucb1Algorithm;
    // Dungeon Variable
    public Door[] doors;
    public GameManagerScript gameManager;
    private PlayerMovement playerMovement;
    private string currentScene; // Menyimpan nama scene saat ini
    private void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;
        playerMovement = FindObjectOfType<PlayerMovement>();
        // ResetUCB1Values();
        if (!PlayerPrefs.HasKey("GameData"))
        {
            ResetUCB1Values();
        }
        else
        {
        LoadUCB1Data();    
        }
        GenerateWave();
        ActivateEnemiesToSpawn();
        ucb1Algorithm = new UCB1Algorithm();
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
                DungeonCompleted(true);
            }
            else
            {
                ActivateEnemiesToSpawn();
            }
        }
    }
    public void DungeonCompleted(bool playerWon)
    {
        GameData gameData = new GameData();
        // Update nilai c dan x sesuai dengan rules dan hasil permainan
        if (playerWon)
        {
            if (difficulty == Difficulty.Easy)
            {
                if (cEasy == 1) // Jika player pertama kali memasuki tingkat kesulitan Easy
                    cNormal = 1; // Set nilai cNormal menjadi 1
                cEasy++;
                xEasy = 1;
                xNormal = 0;
                xHard = 0;
            }
            else if (difficulty == Difficulty.Normal)
            {
                if (cNormal == 2) // Jika player pertama kali memasuki tingkat kesulitan Normal
                    cHard = 1; // Set nilai cHard menjadi 1
                cNormal++;
                xNormal = 1;
                xEasy = 0;
                xHard = 0;
            }
            else if (difficulty == Difficulty.Hard)
            {
                cHard++;
                xHard = 2;
                xEasy = 0;
                xNormal = 0;
            }
        }
        else // Player kalah dari stage
        {
            if (difficulty == Difficulty.Easy)
            {
                cEasy++;
                xEasy = 0;
                xNormal = 0;
                xHard = 0;
            }
            else if (difficulty == Difficulty.Normal)
            {
                cNormal++;
                xNormal = 0;
                xEasy = 0;
                xHard = 0;
            }
            else if (difficulty == Difficulty.Hard)
            {
                cHard++;
                xHard = 1;
                xEasy = 0;
                xNormal = 1;
            }
        }

        // Menghitung skor UCB1 untuk tingkat kesulitan selanjutnya
        float ucb1ScoreEasy = ucb1Algorithm.CalculateUCB1Score(cEasy, xEasy, cNormal, xNormal, cHard, xHard, "Easy");
        float ucb1ScoreNormal = ucb1Algorithm.CalculateUCB1Score(cEasy, xEasy, cNormal, xNormal, cHard, xHard, "Normal");
        float ucb1ScoreHard = ucb1Algorithm.CalculateUCB1Score(cEasy, xEasy, cNormal, xNormal, cHard, xHard, "Hard");
        Debug.Log("cEasy: " + cEasy);
        Debug.Log("cNormal: " + cNormal);
        Debug.Log("cHard: " + cHard);
        Debug.Log("xEasy: " + xEasy);
        Debug.Log("xNormal: " + xNormal);
        Debug.Log("xHard: " + xHard);
        Debug.Log("Score Easy: " + ucb1ScoreEasy);
        Debug.Log("ScoreNormal: " + ucb1ScoreNormal);
        Debug.Log("ScoreHard: " + ucb1ScoreHard);

        // Menentukan tingkat kesulitan berdasarkan skor UCB1
        string currentDifficulty = difficulty.ToString();
        string nextDifficulty = ucb1Algorithm.DetermineNextDifficulty(ucb1ScoreEasy, ucb1ScoreNormal, ucb1ScoreHard, currentDifficulty);
        Debug.Log("Current Difficulty: " + currentDifficulty);

        // Menyimpan posisi player dan scene terakhir
        // PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();
        // gameData.playerPosition = playerMovement.transform.position;
        // gameData.lastScene = SceneManager.GetActiveScene().name;

        // Menyimpan data permainan
        SaveUCB1Data(nextDifficulty);
        Debug.Log("Otomatis ke save lur");

        // Simpan nilai nextDifficulty sebagai PlayerPrefs
        PlayerPrefs.SetString("NextDifficulty", nextDifficulty);
        PlayerPrefs.Save();

        // Pindah ke scene selanjutnya menggunakan SceneTransition
        SceneTransition sceneTransition = FindObjectOfType<SceneTransition>();
        sceneTransition.FadeCo();
    }

    public void SaveUCB1Data(string nextDifficulty)
    {
        // Membuat objek untuk menyimpan data permainan
        GameData gameData = new GameData();
        // Menyimpan posisi dan scene terakhir player
        // gameData.playerPosition = playerMovement.transform.position;
        gameData.lastScene = SceneManager.GetActiveScene().name;
        // Menyimpan nilai tingkat kesulitan selanjutnya
        gameData.nextDifficulty = nextDifficulty;

        // Menyimpan nilai c dan x
        gameData.cEasy = cEasy;
        gameData.xEasy = xEasy;
        gameData.cNormal = cNormal;
        gameData.xNormal = xNormal;
        gameData.cHard = cHard;
        gameData.xHard = xHard;


        // Mengubah objek gameData  menjadi JSON
        string jsonData = JsonUtility.ToJson(gameData);

        // Menyimpan data JSON ke penyimpanan (misalnya PlayerPrefs)
        PlayerPrefs.SetString("GameData", jsonData);
        PlayerPrefs.Save();
    }
    private void LoadUCB1Data()
    {
        string jsonData = PlayerPrefs.GetString("GameData");
        GameData gameData = JsonUtility.FromJson<GameData>(jsonData);

        // Mengisi nilai-nilai cEasy, cNormal, dan cHard dengan nilai yang disimpan sebelumnya
        cEasy = gameData.cEasy;
        cNormal = gameData.cNormal;
        cHard = gameData.cHard;

        string nextDifficulty = PlayerPrefs.GetString("NextDifficulty");
        if (nextDifficulty == "Easy")
            difficulty = Difficulty.Easy;
        else if (nextDifficulty == "Normal")
            difficulty = Difficulty.Normal;
        else if (nextDifficulty == "Hard")
            difficulty = Difficulty.Hard;
        Debug.Log("Next Difficulty: " + gameData.nextDifficulty);
        Debug.Log("cEasy: " + gameData.cEasy);
        Debug.Log("xEasy: " + gameData.xEasy);
        Debug.Log("cNormal: " + gameData.cNormal);
        Debug.Log("xNormal: " + gameData.xNormal);
        Debug.Log("cHard: " + gameData.cHard);
        Debug.Log("xHard: " + gameData.xHard);
    }
    // private void SaveGameData(string nextDifficulty)
    // {
    //     // Membuat objek untuk menyimpan data permainan
    //     GameData gameData = new GameData();

    //     // Menyimpan nilai tingkat kesulitan selanjutnya
    //     gameData.nextDifficulty = nextDifficulty;

    //     // Menyimpan nilai c dan x
    //     gameData.cEasy = cEasy;
    //     gameData.xEasy = xEasy;
    //     gameData.cNormal = cNormal;
    //     gameData.xNormal = xNormal;
    //     gameData.cHard = cHard;
    //     gameData.xHard = xHard;


    //     // Mengubah objek GameData menjadi JSON
    //     string jsonData = JsonUtility.ToJson(gameData);

    //     // Menyimpan data JSON ke penyimpanan (misalnya PlayerPrefs)
    //     PlayerPrefs.SetString("GameData", jsonData);
    //     PlayerPrefs.Save();
    // }
    // private void LoadGameData()
    // {
    //     string jsonData = PlayerPrefs.GetString("GameData");
    //     GameData gameData = JsonUtility.FromJson<GameData>(jsonData);

    //     // Mengisi nilai-nilai cEasy, cNormal, dan cHard dengan nilai yang disimpan sebelumnya
    //     cEasy = gameData.cEasy;
    //     cNormal = gameData.cNormal;
    //     cHard = gameData.cHard;

    //     string nextDifficulty = PlayerPrefs.GetString("NextDifficulty");
    //     if (nextDifficulty == "Easy")
    //         difficulty = Difficulty.Easy;
    //     else if (nextDifficulty == "Normal")
    //         difficulty = Difficulty.Normal;
    //     else if (nextDifficulty == "Hard")
    //         difficulty = Difficulty.Hard;
    //     Debug.Log("Next Difficulty: " + gameData.nextDifficulty);
    //     Debug.Log("cEasy: " + gameData.cEasy);
    //     Debug.Log("xEasy: " + gameData.xEasy);
    //     Debug.Log("cNormal: " + gameData.cNormal);
    //     Debug.Log("xNormal: " + gameData.xNormal);
    //     Debug.Log("cHard: " + gameData.cHard);
    //     Debug.Log("xHard: " + gameData.xHard);
    // }

    public class UCB1Algorithm
    {
        public float CalculateUCB1Score(float cEasy, float xEasy, float cNormal, float xNormal, float cHard, float xHard, string currentDifficulty)
        {
            // Menghitung total penggunaan (c) dari masing-masing tingkat kesulitan
            float totalCEasy = cEasy;
            float totalCNormal = cNormal;
            float totalCHard = cHard;

            // Menghitung total hasil (x) dari masing-masing tingkat kesulitan
            float totalXEasy = xEasy;
            float totalXNormal = xNormal;
            float totalXHard = xHard;

            // Menghitung total permainan yang telah dilakukan di setiap tingkat kesulitan
            float totalGamesEasy = totalCEasy;
            float totalGamesNormal = totalCEasy + totalCNormal;
            float totalGamesHard = totalCEasy + totalCNormal + totalCHard; // Total keseluruhan game yang dimainkan

            // Menghitung rata-rata hasil (x) di setiap tingkat kesulitan
            float averageXEasy = totalXEasy / totalGamesEasy;
            float averageXNormal = totalXNormal / totalGamesNormal;
            float averageXHard = totalXHard / totalGamesHard;

            float defaultUCB1Score = 0f; // Nilai default yang dapat disesuaikan sesuai kebutuhan Anda
            float ucb1ScoreEasy = (totalCEasy > 0f) ? (totalXEasy + Mathf.Sqrt(2f * Mathf.Log(totalGamesHard + Mathf.Epsilon) / totalCEasy)) : defaultUCB1Score;
            float ucb1ScoreNormal = (totalCNormal > 0f) ? (totalXNormal + Mathf.Sqrt(2f * Mathf.Log(totalGamesHard + Mathf.Epsilon) / totalCNormal)) : defaultUCB1Score;
            float ucb1ScoreHard = (totalCHard > 0f) ? (totalXHard + Mathf.Sqrt(2f * Mathf.Log(totalGamesHard + Mathf.Epsilon) / totalCHard)) : defaultUCB1Score;
            Debug.Log("UCB1 Score Easy = totalXEasy(" + totalXEasy + ") + (2 * (log " + totalGamesHard + ") / totalCEasy(" + totalCEasy + "))");
            Debug.Log("UCB1 Score Normal = totalXNormal(" + totalXNormal + ") + (2 * (log " + totalGamesHard + ") / totalCNormal(" + totalCNormal + "))");
            Debug.Log("UCB1 Score Hard = totalXHard(" + totalXHard + ") + (2 * (log " + totalGamesHard + ") / totalCHard(" + totalCHard + "))");


            // Mengembalikan skor UCB1 sesuai tingkat kesulitan saat ini
            if (currentDifficulty == "Easy")
                return ucb1ScoreEasy;
            else if (currentDifficulty == "Normal")
                return ucb1ScoreNormal;
            else if (currentDifficulty == "Hard")
                return ucb1ScoreHard;

            // Jika tingkat kesulitan tidak dikenali, mengembalikan nilai default
            return 0f;
        }


        public string DetermineNextDifficulty(float ucb1ScoreEasy, float ucb1ScoreNormal, float ucb1ScoreHard, string currentDifficulty)
        {

            // Logika baru untuk penentuan tingkat kesulitan
            if (ucb1ScoreEasy > ucb1ScoreNormal && ucb1ScoreEasy > ucb1ScoreHard)
            {
                return "Easy";
            }
            else if (ucb1ScoreNormal > ucb1ScoreEasy && ucb1ScoreNormal > ucb1ScoreHard)
            {
                return "Normal";
            }
            else if (ucb1ScoreHard > ucb1ScoreEasy && ucb1ScoreHard > ucb1ScoreNormal)
            {
                return "Hard";
            }
            else if (ucb1ScoreEasy == ucb1ScoreNormal && currentDifficulty != "Hard")
            {
                return currentDifficulty;
            }
            else if (ucb1ScoreEasy == ucb1ScoreHard && currentDifficulty != "Normal")
            {
                return currentDifficulty;
            }
            else if (ucb1ScoreNormal == ucb1ScoreHard && currentDifficulty != "Easy")
            {
                return currentDifficulty;
            }
            else
            {
                return currentDifficulty;
            }
        }
    }
    public void ResetUCB1Values()
    {
    if (PlayerPrefs.HasKey("GameData"))
    {
        // Menghapus data SavedGame
        PlayerPrefs.DeleteKey("GameData");
        PlayerPrefs.DeleteKey("NextDifficulty");
        PlayerPrefs.Save();

        Debug.Log("SavedGame data has been reset.");
    }
    else
    {
        // Tidak ada data SavedGame yang tersimpan, tidak perlu menghapusnya
        Debug.Log("No saved game data found.");
    }
        // Reset nilai cEasy, cNormal, cHard, xEasy, xNormal, dan xHard
        cEasy = 0;
        cNormal = 0;
        cHard = 0;
        difficulty = Difficulty.Easy;
        Debug.Log("SavedGame telah di-reset.");
    }

    // Mengatur pintu dan aktivasi objek ruangan
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
[System.Serializable]
public class GameData
{
    public string nextDifficulty;
    public float cEasy;
    public float xEasy;
    public float cNormal;
    public float xNormal;
    public float cHard;
    public float xHard;

    public Vector3 playerPosition;
    public string lastScene;
}
