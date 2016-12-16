using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSparksManager : MonoBehaviour {
	private static GameSparksManager instance = null;

	void Awake ()
	{
		if (instance == null) { // check to see if the instance has a refrence
			instance = this; // if not, give it a refrence to this class...
			DontDestroyOnLoad (this.gameObject); // and make this object persistant as we load new scenes
		} else { // if we already have a refrence then remove the extra manager from the scene
			Destroy (this.gameObject);
		}
	}

	void SceneChanged (Scene scene, LoadSceneMode mode){
		string sceneName = scene.name;
		if (sceneName.StartsWith ("02")) {
			
		}
	}

	void OnEnable(){ SceneManager.sceneLoaded += SceneChanged; }

	void OnDisable(){ SceneManager.sceneLoaded -= SceneChanged; }
}
