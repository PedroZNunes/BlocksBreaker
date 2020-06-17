using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerPrefsManager : MonoBehaviour {

	const string MUSIC_VOLUME_KEY = "music_volume";
	const string EFFECTS_VOLUME_KEY = "effects_volume";
	const string LEVEL_KEY = "isunlocked_";
	const string LAST_LEVEL_PLAYED = "last_level";
	const string HIGH_SCORE = "high_score";
	const string BALLS_USED = "balls_used";
	const string DEATHS = "deaths";
	const string POWER_UPS = "power_ups";
	const string BLOCKS_DESTROYED = "blocks_destroyed";


	public static void SetMusicVolume (float volume){
		float minVolume = -20f;
		float maxVolume = 20f;

		if (volume > maxVolume || volume < minVolume)
			Debug.LogWarning ("Trying to set music volume out of bounds");

		PlayerPrefs.SetFloat (MUSIC_VOLUME_KEY, Mathf.Clamp(volume, minVolume, maxVolume));
	}

	public static float GetMusicVolume (){
		float volume = PlayerPrefs.GetFloat (MUSIC_VOLUME_KEY);
		return volume;
	}


	public static void SetEffectsVolume (float volume){
		float minVolume = -20f;
		float maxVolume = 20f;

		if (volume > maxVolume || volume < minVolume)
			Debug.LogWarning ("Trying to set effects volume out of bounds");

		PlayerPrefs.SetFloat (EFFECTS_VOLUME_KEY, Mathf.Clamp(volume, minVolume, maxVolume));
	}

	public static float GetEffectsVolume (){
		float volume = PlayerPrefs.GetFloat (EFFECTS_VOLUME_KEY);
		return volume;
	}


	public static bool IsLevelUnlocked (string levelID){ //3-digit ID
		int levelValue = PlayerPrefs.GetInt(LEVEL_KEY + levelID);
		bool isLevelUnlocked = (levelValue == 1);
//		Debug.Log ("LevelID " + levelID + " is Unlocked? " + isLevelUnlocked);
		return isLevelUnlocked;
	}

	public static void UnlockLevel (string levelID){
		PlayerPrefs.SetInt(LEVEL_KEY + levelID, 1); //use 1 for true
	}

	public static void ResetProgress(){
		PlayerPrefs.DeleteAll ();
		UnlockStartLevel ();
	}

	public static void UnlockStartLevel (){
		UnlockLevel ("101");
	}


	public static void Set_Stats(int ballsUsed, int deaths, int blocksDestroyed, int powerUps){
		PlayerPrefs.SetInt (BALLS_USED, ballsUsed);
		PlayerPrefs.SetInt (DEATHS, deaths);
		PlayerPrefs.SetInt (BLOCKS_DESTROYED, blocksDestroyed);
		PlayerPrefs.SetInt (POWER_UPS, powerUps);
	}

	public static int Get_BallsUsed (){
		int ballsUsed = PlayerPrefs.GetInt (BALLS_USED, 0);
		return ballsUsed;
	}

	public static int Get_Deaths (){
		int deaths = PlayerPrefs.GetInt (DEATHS, 0);
		return deaths;
	}

	public static int Get_PowerUps (){
		int powerUps = PlayerPrefs.GetInt (POWER_UPS, 0);
		return powerUps;
	}

	public static int Get_BlocksDestroyed (){
		int blocksDestroyed = PlayerPrefs.GetInt (BLOCKS_DESTROYED, 0);
		return blocksDestroyed;
	}



}
