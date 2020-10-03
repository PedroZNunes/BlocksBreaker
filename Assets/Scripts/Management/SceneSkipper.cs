using UnityEngine;

public class SceneSkipper : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        Invoke("SkipScene", 60f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            SkipScene();
        }
    }

    public void SkipScene()
    {
        PlayerPrefsManager.ResetProgress();

        LevelManager.LoadMenu(MenuScenes.Start);
    }
}
