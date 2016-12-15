using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour {

	[SerializeField] private AudioMixer audioMixer;
	private static MusicPlayer instance;

	//singleton Process
	void Awake () {
		if (instance != null) {	
			Destroy (gameObject);
		} else {
			instance = this;
			DontDestroyOnLoad (gameObject);
		}
	}

	void Start (){
		audioMixer.SetFloat ("effectsVolume", PlayerPrefsManager.GetEffectsVolume ());
		audioMixer.SetFloat ("musicVolume", PlayerPrefsManager.GetMusicVolume ());
	}


	// Persists on options menu.
	void OnEnable(){ SceneManager.sceneLoaded += SceneChanged; }

	void OnDisable(){ SceneManager.sceneLoaded -= SceneChanged; }

	void SceneChanged (Scene scene, LoadSceneMode mode){
		if (!scene.name.StartsWith ("01"))
			Destroy (gameObject);
	}
	//EndOf

}
