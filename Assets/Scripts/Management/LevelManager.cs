using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

[RequireComponent (typeof(AudioController))]
public class LevelManager : MonoBehaviour {
    
	
	private string currentSceneName;

	static public bool isSceneALevel { get { return (GetCurrentSceneByName().StartsWith("02")); } }
    
    /// <summary>
    /// singleton Process
    /// </summary>
    static private LevelManager instance;

	void Awake () {
		if (instance != null) {	
			Destroy (gameObject);
		} else {
			instance = this;
			DontDestroyOnLoad (gameObject);
		}
	}

	void Start(){
		PlayerPrefsManager.UnlockStartLevel();
	}

	void Update(){
		
	}

	static public string GetCurrentSceneByName()
	{
		return instance.currentSceneName;
	}

	private void SerCurrentSceneName(string value)
	{
		currentSceneName = value;
	}

	void SceneChanged (Scene scene, LoadSceneMode mode){
        SerCurrentSceneName(SceneManager.GetActiveScene().name);
		UnlockCurrentLevel ();
		AudioController.LoadVolumes ();
	}

	/// <summary>
	/// Load menu screens
	/// </summary>
	/// <param name="scene">use menuscene enum to tell which scene you wish to load</param>
	static public void LoadMenu(MenuScenes scene)
    {
		string sceneName;
        switch (scene)
        {
            case MenuScenes.Start:
				sceneName = "01 Start";
                break;
			case MenuScenes.LevelSelect:
				sceneName = "01 LevelSelect";
				break;
            case MenuScenes.Options:
				sceneName = "01 Options";
                break;
            case MenuScenes.Stats:
				sceneName = "01 Stats";
                break;
            default:
				sceneName = "";
                break;
        }

		if (sceneName != "")
        {
			SceneManager.LoadScene (sceneName, LoadSceneMode.Single);
		}
	}


	static public void ReloadCurrentLevel()
    {
		LoadLevel(GetCurrentSceneByName());
    }

	static public void LoadLevel (string levelName){
		SceneManager.LoadScene (levelName, LoadSceneMode.Single);
	}

	static public void LoadLevel (int levelIndex){
		SceneManager.LoadScene (levelIndex, LoadSceneMode.Single);
	}

	static public void LoadNextLevel (){
		instance.UnlockCurrentLevel ();
		int nextLevel = SceneManager.GetActiveScene ().buildIndex + 1;
		LoadLevel (nextLevel);
	}

	void UnlockCurrentLevel(){
		//unlock next level for playing
		if (isSceneALevel)
		{
			string levelID = SceneManager.GetActiveScene ().name.Substring (9);
			print ("Unlocking levelID " + levelID);
			PlayerPrefsManager.UnlockLevel(levelID);
		}
	}




	void OnEnable(){ SceneManager.sceneLoaded += SceneChanged; }

	void OnDisable(){ 
		SceneManager.sceneLoaded -= SceneChanged; 
		Time.timeScale = 1f;
	}


}

public enum MenuScenes { Start, LevelSelect, Options, Stats }