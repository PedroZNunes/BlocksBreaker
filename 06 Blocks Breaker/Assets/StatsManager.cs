using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsManager : MonoBehaviour {

	[SerializeField] Text BallsUsedText;
	[SerializeField] Text DeathsText;
	[SerializeField] Text LevelsPlayedText;
	[SerializeField] Text LevelsCompletedText;
	[SerializeField] Text PowerUpsText;
	[SerializeField] Text QuickestLevelText;
	[SerializeField] Text LongestLevelText;

	void Update () {
		if (Input.GetKeyDown (KeyCode.A)) {
			AuthenticateDevice ();
		} if (Input.GetKeyDown (KeyCode.B)) {
			SaveData ();
		}
		if (Input.GetKeyDown (KeyCode.C)) {
			GetData ();
		}
	}

	void AuthenticateDevice(){
		Debug.Log ("Authenticating Device...");
		new GameSparks.Api.Requests.DeviceAuthenticationRequest ()
				.SetDisplayName ("Randy")
				.Send ((response) => {
			if (!response.HasErrors) {Debug.Log ("Device Authenticated...");}
				else {Debug.LogError ("Error Authenticating Device...");}
		});
	}

	void SaveData(){
		Debug.Log ("Saving player's data...");
		new GameSparks.Api.Requests.LogEventRequest ().SetEventKey ("Set_Stats")
			.SetEventAttribute ("BallsUsed", 123)
			.SetEventAttribute ("Deaths", 321)
			.SetEventAttribute ("LevelsPlayed", 12)
			.SetEventAttribute ("LevelsComp", 5)
			.SetEventAttribute ("PowerUps", 54)
			.SetEventAttribute ("QuickLevel", "Level 06")
			.SetEventAttribute ("LongLevel", "Level 09")
			.Send ((response) => {
				if (!response.HasErrors) {Debug.Log ("Player's data saved...");}
				else {Debug.LogError ("Error Saving player's data...");}
		});
	}

	void GetData(){
		new GameSparks.Api.Requests.LogEventRequest ().SetEventKey ("Get_Stats")
			.Send ((response) => {
			if (!response.HasErrors) {
				Debug.Log ("Player' stats loaded...");
				GameSparks.Core.GSData stats = response.ScriptData.GetGSData ("playerStats");
					BallsUsedText.text = stats.GetInt ("playerBalls").ToString();
					DeathsText.text = stats.GetInt ("playerDeaths").ToString();
					LevelsPlayedText.text = stats.GetInt ("playerLevelsPlayed").ToString();
					LevelsCompletedText.text = stats.GetInt ("playerLevelsComp").ToString();
					PowerUpsText.text = stats.GetInt ("playerPowerUps").ToString();
				QuickestLevelText.text = stats.GetString ("playerQuickLevel");
				LongestLevelText.text = stats.GetString ("playerLongLevel");
						
			} else {
				Debug.LogError ("Error Loading Player's stats...");
			}
		});
	}
}
