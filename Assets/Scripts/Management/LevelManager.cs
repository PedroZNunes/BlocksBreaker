using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

[RequireComponent (typeof(AudioController))]
public class LevelManager : MonoBehaviour {

	private AudioController audioController;

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

		audioController = GetComponent<AudioController>();
	}

	void Start(){
		PlayerPrefsManager.UnlockStartLevel();
	}

	void Update(){
		
	}

	void SceneChanged (Scene scene, LoadSceneMode mode){
		UnlockCurrentLevel ();
		audioController.LoadVolumes ();
	}


	public void LoadLevel (string levelName){
		SceneManager.LoadScene (levelName, LoadSceneMode.Single);
	}

	public void LoadLevel (int levelIndex){
		SceneManager.LoadScene (levelIndex, LoadSceneMode.Single);
	}

	public void LoadNextLevel (){
		UnlockCurrentLevel ();
		int nextLevel = SceneManager.GetActiveScene ().buildIndex + 1;
		LoadLevel (nextLevel);
	}

	void UnlockCurrentLevel(){
		//unlock next level for playing
		if (SceneManager.GetActiveScene().name.StartsWith ("02")){
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
