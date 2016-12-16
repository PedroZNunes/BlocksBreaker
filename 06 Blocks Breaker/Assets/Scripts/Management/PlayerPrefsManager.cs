using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerPrefsManager : MonoBehaviour {

	const string MUSIC_VOLUME_KEY = "music_volume";
	const string EFFECTS_VOLUME_KEY = "effects_volume";
	const string LEVEL_KEY = "levelunlocked_";
	const string LAST_LEVEL_PLAYED = "last_level";
	const string HIGH_SCORE = "high_score";

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



	public static void Unlocklevel (int level){
		if (level < SceneManager.sceneCountInBuildSettings) {
			PlayerPrefs.SetInt(LEVEL_KEY + level.ToString(), 1); //use 1 for true
		} else {
			Debug.LogError ("Trying to unlock level not in build settings");
		}
	}

	public static void SetLastLevelPlayed (int level){
		Unlocklevel (level);
		PlayerPrefs.SetInt (LAST_LEVEL_PLAYED, level);
	}

	public static int GetLastLevelPlayed (){
		int lastLevel = PlayerPrefs.GetInt (LAST_LEVEL_PLAYED);
		return lastLevel;
	}

		
	public static bool IsLevelUnlocked (int level){
		int levelValue = PlayerPrefs.GetInt(LEVEL_KEY+level.ToString());
		bool isLevelUnlocked = (levelValue == 1);
		if (level <= SceneManager.sceneCountInBuildSettings - 1) {
			return isLevelUnlocked;
		}else{
			Debug.LogError ("Trying to query level not in build settings");
			return false;
		}
	}

	public static void SetHighScore(int score){
		PlayerPrefs.SetInt (HIGH_SCORE, score);
	}

	public static int GetHighScore(){
		int score = PlayerPrefs.GetInt (HIGH_SCORE, 0);
		return score;
	}
		
}
