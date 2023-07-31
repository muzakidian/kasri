using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class IntroVideo : MonoBehaviour
{
    public PlayableDirector director;
    public string nextSceneName = "rumah";
    // public FadeEffect fadeEffect; // Referensi ke FadeEffect

    void Start()
    {
        director.stopped += OnVideoFinished;
        director.Play();
    }
    void Update()
    {
        // Cek apakah pemain menekan tombol "Skip" atau input yang telah ditentukan untuk skip video.
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space))
        {
            // Panggil fungsi OnVideoFinished dengan mengirimkan direktor video yang sama.
            OnVideoFinished(director);
        }
    }

    void OnVideoFinished(PlayableDirector d)
    {
        if (d == director)
        {
            // Panggil fungsi FadeOutAndLoadScene dari FadeEffect
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
