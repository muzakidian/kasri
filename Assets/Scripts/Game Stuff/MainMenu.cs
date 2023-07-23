using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;


public class MainMenu : MonoBehaviour
{
    public GameObject optionMenuPanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewGame()
    {   
        // Reset data SavedGame saat memulai permainan baru
        ResetSavedGameData();
        // Aktifkan GameObject "IntroController"
        SceneManager.LoadScene("Intro");
    }
    public void LoadGame()
    {
        // Cek apakah ada data SavedGame yang tersimpan
        if (PlayerPrefs.HasKey("GameData"))
        {
            // Muat data GameDataSave dari PlayerPrefs
            string jsonData = PlayerPrefs.GetString("GameData");
            GameData gameDataSave = JsonUtility.FromJson<GameData>(jsonData);

            // Muat scene terakhir yang disimpan
            SceneManager.LoadScene(gameDataSave.lastScene);

            // Pindahkan player ke posisi terakhir
            // PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();
            // playerMovement.transform.position = gameDataSave.playerPosition;
        }
        else
        {
            // Tidak ada data SavedGame yang tersimpan
            Debug.Log("No saved game data found.");
        }
    }
    public void QuitToDesktop()
    {
        Application.Quit();
    }
    private void ResetSavedGameData()
    {
        // Menghapus data SavedGame
        PlayerPrefs.DeleteKey("GameData");
        PlayerPrefs.DeleteKey("NextDifficulty");
        PlayerPrefs.DeleteKey("LastScene");
        PlayerPrefs.Save();

        Debug.Log("SavedGame data has been reset.");
    }
    public void ShowMenuOptions()
    {
    // Cek apakah ada instance dari DungeonEnemyRoom
    DungeonEnemyRoom dungeonRoom = FindObjectOfType<DungeonEnemyRoom>();
    // Jika Options sudah aktif, nonaktifkan. Jika tidak, aktifkan.
    bool isActive = optionMenuPanel.activeSelf;
    optionMenuPanel.SetActive(!isActive);
    }
    public void BackToMenu()
    {
        optionMenuPanel.SetActive(false); // nonaktifkan optionsPanel
    }
}
