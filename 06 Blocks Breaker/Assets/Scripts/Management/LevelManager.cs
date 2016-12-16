using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class LevelManager : MonoBehaviour {



	[SerializeField] private AudioMixer audioMixer;

	private int currentScore;

	static private LevelManager instance;
	//singleton Process
	void Awake () {
		if (instance != null) {	
			Destroy (gameObject);
		} else {
			instance = this;
			DontDestroyOnLoad (gameObject);
		}
	}



	void SceneChanged (Scene scene, LoadSceneMode mode){
		string sceneName = scene.name;
		Load (sceneName);
	}

	public void LoadLevel (string levelName){
		SceneManager.LoadScene (levelName, LoadSceneMode.Single);
	}

	public void LoadLevel (int levelIndex){
		SceneManager.LoadScene (levelIndex, LoadSceneMode.Single);
	}

	public void LoadNextLevel (){
		Save ();
		int nextLevel = SceneManager.GetActiveScene ().buildIndex + 1;
		LoadLevel (nextLevel);
	}

	void Save(){
		//TODO save high score from previous level

		//unlock next level for playing
		PlayerPrefsManager.Unlocklevel(SceneManager.GetActiveScene().buildIndex+1);
	}

	void Load(string sceneName){
		//TODO load high score


		float musicvolume = PlayerPrefsManager.GetMusicVolume ();
		float effectsVolume = PlayerPrefsManager.GetEffectsVolume ();
		audioMixer.SetFloat ("musicVolume", musicvolume);
		audioMixer.SetFloat ("effectsVolume", effectsVolume);
	}


	void OnEnable(){ SceneManager.sceneLoaded += SceneChanged; }

	void OnDisable(){ 
		SceneManager.sceneLoaded -= SceneChanged; 
		Time.timeScale = 1f;
	}


}
