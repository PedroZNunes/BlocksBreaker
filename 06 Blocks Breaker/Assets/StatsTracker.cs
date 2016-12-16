using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StatsTracker : MonoBehaviour {

	private int ballsUsed = 0;
	private int deaths = 0;
	private int powerUps = 0;
	private int blocksDestroyed = 0;

	private int oldBallsUsed = 0;
	private int oldDeaths = 0;
	private int oldPowerUps = 0;
	private int oldBlocksDestroyed = 0;

	private bool dataLoaded = false;


	void CountRemainingBalls(){
		Ball[] balls = FindObjectsOfType<Ball> ();
		Debug.Log ("Balls Remaining In Game: " + balls.Length);
		ballsUsed += balls.Length;
	}

	void IncrementBallsUsed(){ballsUsed++;}
	void IncrementDeaths(){deaths++;}
	void IncrementPowerUps(){powerUps++;}
	void IncrementBlocksDestroyed(){blocksDestroyed++;}
	void UpdateAllStatsWrapper(){StartCoroutine (UpdateAllStats ());}

	IEnumerator UpdateAllStats(){
		CountRemainingBalls ();
		if (GameSparksManager.isAuthenticated) {
			GetData ();
		}
		yield return (dataLoaded);
		Debug.Log ("Starting to Save...");

	}

	void GetData(){
		new GameSparks.Api.Requests.LogEventRequest ().SetEventKey ("Get_Stats")
			.Send ((response) => {
				if (!response.HasErrors) {
					Debug.Log ("Player' stats loaded...");
					GameSparks.Core.GSData stats = response.ScriptData.GetGSData ("playerStats");
					oldBallsUsed = stats.GetInt ("playerBalls").GetValueOrDefault();
					oldDeaths = stats.GetInt ("playerDeaths").GetValueOrDefault();
					oldPowerUps = stats.GetInt ("playerPowerUps").GetValueOrDefault();
					oldBlocksDestroyed = stats.GetInt ("playerBlocksDestroyed").GetValueOrDefault();
					print ("Old data: " + oldBallsUsed + " " + oldDeaths + " " + oldPowerUps + " " + oldBlocksDestroyed);
					dataLoaded = true;
					AssignVariables ();
					SaveAllData ();
				} else {
					Debug.LogError ("Error Loading Player's stats...");
				}
			});
	}

	void SaveAllData(){
		if (GameSparksManager.isAuthenticated) {
			Debug.Log ("Saving player's data...");
			new GameSparks.Api.Requests.LogEventRequest ().SetEventKey ("Set_Stats")
				.SetEventAttribute ("BallsUsed", ballsUsed)
				.SetEventAttribute ("Deaths", deaths)
				.SetEventAttribute ("PowerUps", powerUps)
				.SetEventAttribute ("BlocksDestroyed", blocksDestroyed)
				.Send ((response) => {
				if (!response.HasErrors) {
					Debug.Log ("Player's data saved...");
					string stats = string.Format("Balls Used: {0} // Deaths: {1} // PowerUps: {2} // BlocksDestroyed: {3}", ballsUsed, deaths, powerUps, blocksDestroyed);
					print (stats);
				} else {
					Debug.LogError ("Error Saving player's data...");
				}
			});
		}
	}

	void AssignVariables(){
		ballsUsed += oldBallsUsed;
		deaths += oldDeaths;
		blocksDestroyed += oldBlocksDestroyed;
		powerUps += oldPowerUps;
	}


	void OnEnable(){ 
		Ball.BallDestroyedEvent += IncrementBallsUsed;
		Block.BlockDestroyedEvent += IncrementBlocksDestroyed;
		PickUpItem.PowerUpPickedUpEvent += IncrementPowerUps;
		ActionMaster.LoseEvent += IncrementDeaths;
		ActionMaster.UpdateAllStatsEvent += UpdateAllStatsWrapper;
	}

	void OnDisable(){
		Ball.BallDestroyedEvent -= IncrementBallsUsed;
		Block.BlockDestroyedEvent -= IncrementBlocksDestroyed;
		PickUpItem.PowerUpPickedUpEvent -= IncrementPowerUps;
		ActionMaster.LoseEvent -= IncrementDeaths;
		ActionMaster.UpdateAllStatsEvent -= UpdateAllStatsWrapper;
	}
}
