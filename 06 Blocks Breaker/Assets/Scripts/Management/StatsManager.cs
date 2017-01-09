using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StatsManager : MonoBehaviour {

	[SerializeField] private Text BallsUsedText;
	[SerializeField] private Text DeathsText;
	[SerializeField] private Text PowerUpsText;
	[SerializeField] private Text BlocksDestroyedText;

	private int ballsUsed;
	private int deaths;
	private int powerUps;
	private int blocksDestroyed;


	void Start () {
		GetData ();
		FillTextFields ();
	}

	void Update(){
		if (Input.GetButtonDown ("Cancel")) {
			FindObjectOfType<LevelManager> ().LoadLevel ("01a Start");
		}
	}

	void GetData(){
		ballsUsed = PlayerPrefsManager.Get_BallsUsed ();
		deaths = PlayerPrefsManager.Get_Deaths ();
		powerUps = PlayerPrefsManager.Get_PowerUps ();
		blocksDestroyed = PlayerPrefsManager.Get_BlocksDestroyed ();
	}

	void FillTextFields(){
		if (SceneManager.GetActiveScene().name.StartsWith ("01c")){
			BallsUsedText.text = ballsUsed.ToString ();
			DeathsText.text = deaths.ToString ();
			PowerUpsText.text = powerUps.ToString ();
			BlocksDestroyedText.text = blocksDestroyed.ToString ();
		}
	}

}
