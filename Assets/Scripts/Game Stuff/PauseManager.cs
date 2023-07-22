using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{

    private bool isPaused;
    public GameObject pausePanel;
    public GameManagerScript gameManager;
    public bool usingPausePanel;
    public string mainMenu;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManagerScript>();
        isPaused = false;
        pausePanel.SetActive(false);
        usingPausePanel = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            ChangePause();
        }

    }

    public void ChangePause()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0f;
            usingPausePanel = true; ;
        }
        else
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void QuitToMain()
    {
        SceneManager.LoadScene(mainMenu);
        Time.timeScale = 1f;
    }

    public void SwitchPanels()
    {
        usingPausePanel = !usingPausePanel;
        if (usingPausePanel)
        {
            pausePanel.SetActive(true);
        }
        else
        {
            pausePanel.SetActive(false);
        }
    }
    public void RestartGame()
    {
        // Kembalikan Time.timeScale ke 1 sebelum me-restart
        Time.timeScale = 1f;
        // Panggil fungsi Restart() dari GameManagerScript
        gameManager.Restart();
    }
    public void SaveGame()
    {
        // Cek apakah ada instance dari DungeonEnemyRoom
        DungeonEnemyRoom dungeonRoom = FindObjectOfType<DungeonEnemyRoom>();
        if (dungeonRoom != null)
        {
            // Panggil fungsi SaveGameData dari DungeonEnemyRoom
            dungeonRoom.SaveUCB1Data(PlayerPrefs.GetString("NextDifficulty"));
        }
    }
}
