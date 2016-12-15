using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class LevelManager : MonoBehaviour {

	[SerializeField] private AudioMixer audioMixer;

	const int PLAYER_BASE_HP = 3;

	private int currentScore;
	private int playerCurrentHP;

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
		Player player = FindObjectOfType<Player> ();
		if (player!= null)
			playerCurrentHP = player.hp;
		currentScore = ScoreManager.GetScore ();
		print (currentScore);
	}

	void Load(string sceneName){
		Player player = FindObjectOfType<Player> ();
		if (player != null){
			if (playerCurrentHP != 0)
				player.hp = playerCurrentHP;
			else
				playerCurrentHP = PLAYER_BASE_HP;
		}

		float musicvolume = PlayerPrefsManager.GetMusicVolume ();
		float effectsVolume = PlayerPrefsManager.GetEffectsVolume ();
		audioMixer.SetFloat ("musicVolume", musicvolume);
		audioMixer.SetFloat ("effectsVolume", effectsVolume);
		if (sceneName == "01a Start") {
			ResetProgress();
		} else {
			ScoreManager.SetScore (currentScore);
		}
	}

	public void ResetProgress(){
		ScoreManager.ResetScore ();
		currentScore = ScoreManager.GetScore ();
		playerCurrentHP = PLAYER_BASE_HP;
	}

	void OnEnable(){ SceneManager.sceneLoaded += SceneChanged; }

	void OnDisable(){ 
		SceneManager.sceneLoaded -= SceneChanged; 
		Time.timeScale = 1f;
	}


}
