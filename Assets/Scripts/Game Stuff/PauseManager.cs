using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class PauseManager : MonoBehaviour
{
    public TextMeshProUGUI cEasyText;
    public TextMeshProUGUI cNormalText;
    public TextMeshProUGUI cHardText;
    public TextMeshProUGUI scoreEasyText;
    public TextMeshProUGUI scoreNormalText;
    public TextMeshProUGUI scoreHardText;
    public TextMeshProUGUI nextDifficultyText;
    private bool isPaused;
    public GameObject pausePanel;
    public GameObject optionsPanel;
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
    public void ShowOptions()
    {
    // Cek apakah ada instance dari DungeonEnemyRoom
    DungeonEnemyRoom dungeonRoom = FindObjectOfType<DungeonEnemyRoom>();

    // Jika Options sudah aktif, nonaktifkan. Jika tidak, aktifkan.
    bool isActive = optionsPanel.activeSelf;
    optionsPanel.SetActive(!isActive);

    // Jika Options baru saja diaktifkan, update teks.
    if (!isActive)
    {
        cEasyText.text = "" + dungeonRoom.cEasy;
        cNormalText.text = "" + dungeonRoom.cNormal;
        cHardText.text = "" + dungeonRoom.cHard;
        scoreEasyText.text = "" + dungeonRoom.Ucb1ScoreEasy;
        scoreNormalText.text = "" + dungeonRoom.Ucb1ScoreNormal;
        scoreHardText.text = "" + dungeonRoom.Ucb1ScoreHard;
        nextDifficultyText.text = "" + dungeonRoom.NextDifficulty;
    }
    }
    public void BackToPause()
    {
        optionsPanel.SetActive(false); // nonaktifkan optionsPanel
        pausePanel.SetActive(true); // aktifkan pausePanel
    }

}
