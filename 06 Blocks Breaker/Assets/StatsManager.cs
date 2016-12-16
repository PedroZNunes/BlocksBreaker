using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StatsManager : MonoBehaviour {

	[SerializeField] Text BallsUsedText;
	[SerializeField] Text DeathsText;
	[SerializeField] Text PowerUpsText;
	[SerializeField] Text BlocksDestroyedText;

	private int ballsUsed;
	private int deaths;
	private int powerUps;
	private int blocksDestroyed;

	private bool dataLoaded = false;


	void Start () {
		StartCoroutine (GetData ());

	}


	IEnumerator GetData(){
		yield return (GameSparksManager.isAuthenticated);
		new GameSparks.Api.Requests.LogEventRequest ().SetEventKey ("Get_Stats")
			.Send ((response) => {
			if (!response.HasErrors) {
				Debug.Log ("Player' stats loaded...");
				GameSparks.Core.GSData stats = response.ScriptData.GetGSData ("playerStats");
				ballsUsed = stats.GetInt ("playerBalls").GetValueOrDefault ();
				deaths = stats.GetInt ("playerDeaths").GetValueOrDefault ();
				powerUps = stats.GetInt ("playerPowerUps").GetValueOrDefault ();
				blocksDestroyed = stats.GetInt ("playerBlocksDestroyed").GetValueOrDefault ();
				dataLoaded = true;
				FillTextFields ();
			} else {
				Debug.LogError ("Error Loading Player's stats...");
			}
		});
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
