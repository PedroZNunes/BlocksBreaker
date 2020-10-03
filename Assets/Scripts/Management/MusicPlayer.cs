using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour
{

    private static MusicPlayer instance;

    //singleton Process
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Persists on options menu.
    void OnEnable() { SceneManager.sceneLoaded += SceneChanged; }

    void OnDisable() { SceneManager.sceneLoaded -= SceneChanged; }

    void SceneChanged(Scene scene, LoadSceneMode mode)
    {
        if (!scene.name.StartsWith("01"))
            Destroy(gameObject);
    }
    //EndOf

}
