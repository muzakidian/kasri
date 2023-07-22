using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class IntroVideo : MonoBehaviour
{
    public PlayableDirector director;
    public string nextSceneName = "rumah";
    public FadeEffect fadeEffect; // Referensi ke FadeEffect

    void Start()
    {
        director.stopped += OnVideoFinished;
        director.Play();
    }

    void OnVideoFinished(PlayableDirector d)
    {
        if (d == director)
        {
            // Panggil fungsi FadeOutAndLoadScene dari FadeEffect
            fadeEffect.FadeOutAndLoadScene(nextSceneName);
        }
    }
}
