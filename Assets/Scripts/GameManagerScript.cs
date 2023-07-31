using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public GameObject gameOverUI;
    private PlayerMovement playerMovement;
    private DungeonEnemyRoom dungeonEnemyRoom;
    // Start is called before the first frame update
    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>(); // Mendapatkan referensi PlayerMovement
        playerMovement.gameManager = this;
        dungeonEnemyRoom = FindObjectOfType<DungeonEnemyRoom>(); // Mendapatkan referensi DungeonEnemyRoom
        dungeonEnemyRoom.gameManager = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void gameOver()
    {
        gameOverUI.SetActive(true);
    }
    public void Restart()
    {
        dungeonEnemyRoom.DungeonCompleted(false);

        Debug.Log("Score Easy: " + dungeonEnemyRoom.Ucb1ScoreEasy);
        Debug.Log("ScoreNormal: " + dungeonEnemyRoom.Ucb1ScoreNormal);
        Debug.Log("ScoreHard: " + dungeonEnemyRoom.Ucb1ScoreHard);
        playerMovement.currentHealth.RuntimeValue = playerMovement.currentHealth.initialValue;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
