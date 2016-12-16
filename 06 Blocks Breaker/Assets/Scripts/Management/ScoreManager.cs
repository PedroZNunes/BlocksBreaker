using UnityEngine;
using System.Collections;

public class ScoreManager : MonoBehaviour {
	
	public delegate void UIHandler (int score);
	static public event UIHandler GUIUpdateScoreEvent;
	static private int score;


	static public void AddPoints(int points){
		score += points;
		GUIUpdateScoreEvent (score);
	}

	static public int GetScore (){
		return score;
	}

	static public void SetScore(int points){
		score = points;
		GUIController guiInstance = FindObjectOfType<GUIController> ();
		if (guiInstance != null)
			GUIUpdateScoreEvent (score);
	}

	static public void ResetScore(){
		SetScore (0);
	}

}
