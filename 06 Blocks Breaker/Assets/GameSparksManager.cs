using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSparksManager : MonoBehaviour {
	
	private static GameSparksManager instance = null;

	static public bool isAuthenticated = false;

	void Awake ()
	{
		if (instance == null) { // check to see if the instance has a refrence
			instance = this; // if not, give it a refrence to this class...
			DontDestroyOnLoad (this.gameObject); // and make this object persistant as we load new scenes
		} else { // if we already have a refrence then remove the extra manager from the scene
			Destroy (this.gameObject);
		}
	}

	void Start(){
		StartCoroutine (AuthenticateDevice ());
	}

	IEnumerator AuthenticateDevice(){
		yield return (GameSparks.Core.GS.Available);
		Debug.Log ("Authenticating Device...");
		new GameSparks.Api.Requests.DeviceAuthenticationRequest ()
			.SetDisplayName ("Randy")
			.Send ((response) => {
				if (!response.HasErrors) {Debug.Log ("Device Authenticated..."); isAuthenticated = true;}
				else {Debug.LogError ("Error Authenticating Device..."); isAuthenticated = false;}
			});
	}

}
