using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;


public class MainMenu : MonoBehaviour
{
    public GameObject optionMenuPanel;
    public GameObject noSavedDataImage;
    public float warningDisplayDuration = 1f;
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
        }
    else
    {
        // Tampilkan gambar yang menunjukkan player belum bisa meload game
        if (noSavedDataImage != null)
        {
            noSavedDataImage.gameObject.SetActive(true);
            StartCoroutine(HideWarningAfterDelay());
        }
        else
        {
            Debug.LogWarning("NoSavedDataImage not assigned in the inspector.");
        }
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
    private IEnumerator HideWarningAfterDelay()
    {
        yield return new WaitForSeconds(warningDisplayDuration);
        if (noSavedDataImage != null)
        {
            noSavedDataImage.gameObject.SetActive(false);
        }
    }
}
