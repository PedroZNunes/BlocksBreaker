using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GUIController : MonoBehaviour {

	public delegate void LaunchHandler (Vector2 direction);
	public static event LaunchHandler LaunchEvent;

	[SerializeField] private LaunchInterface firstLaunch;
	[SerializeField] private GameObject pauseMenu;
	[SerializeField] private GameObject winScreen;
	[SerializeField] private GameObject loseScreen;
	[SerializeField] private GameObject mainCanvas;
	[SerializeField] private Text levelNameText;

	private GameMaster actionMaster;


	void OnEnable(){
		actionMaster = FindObjectOfType<GameMaster> ();
		Debug.Assert (actionMaster != null, "Action Master not found");
		pauseMenu.SetActive (false);
		Subscribe ();
	}

	void Start(){
//		int highScore = PlayerPrefsManager.GetHighScore ();
//		highScoreText.text = string.Format ("{0:00000}", highScore);
		string sceneName = SceneManager.GetActiveScene ().name;
		string levelText = sceneName.Substring(sceneName.LastIndexOf('_')+2);
		levelNameText.text = "Level " + levelText;
	}


	void GameStateSwitched (GameMaster.States currentState){
		switch (currentState){
		case GameMaster.States.Start:
			//do nothing
			break;
		case GameMaster.States.OpeningAnimations:
			//do nothing
			break;
		case GameMaster.States.FirstLaunch:
			firstLaunch.gameObject.SetActive (true);
			firstLaunch.ActivateFirstLaunch ();
			break;
		case GameMaster.States.Play:
			firstLaunch.gameObject.SetActive (false);
			break;
		case GameMaster.States.NoBallsLeft:
			break;
		case GameMaster.States.Relaunch:
			firstLaunch.gameObject.SetActive (true);
			firstLaunch.ActivateLaunch ();
			break;
		case GameMaster.States.Victory:
			firstLaunch.gameObject.SetActive (false);
			ShowWinScreen ();
			break;
		case GameMaster.States.Defeat:
			firstLaunch.gameObject.SetActive (false);
			ShowLoseScreen ();
			break;
		case GameMaster.States.Pause:
			firstLaunch.gameObject.SetActive (false);
			break;
		}
	}

	public void ProcLaunchEvent(Vector2 direction){
		if (LaunchEvent != null)
			LaunchEvent (direction);
		else
			Debug.LogError ("No methods found in launch event");
		actionMaster.TriggerPlay();
	}

	void ShowHidePauseMenu (string currentState){
		if (currentState == "Pause") {
			pauseMenu.SetActive (true);
		} else {
			pauseMenu.SetActive (false);
		}
	}


	void ShowLoseScreen(){
		Instantiate (loseScreen, mainCanvas.transform.position, Quaternion.identity, mainCanvas.transform);
	}

	void ShowWinScreen(){
		Instantiate (winScreen, mainCanvas.transform.position, Quaternion.identity, mainCanvas.transform);

	}

	void OnDisable(){
		Unsubscribe ();
	}

	void Subscribe(){
		GameMaster.GUIPauseMenuEvent += ShowHidePauseMenu;
		GameMaster.GUIStateChangedEvent += GameStateSwitched;
//		ScoreManager.GUIUpdateScoreEvent += UpdateScore;
	}

	void Unsubscribe(){
		GameMaster.GUIPauseMenuEvent -= ShowHidePauseMenu;
		GameMaster.GUIStateChangedEvent -= GameStateSwitched;
//		ScoreManager.GUIUpdateScoreEvent -= UpdateScore;
	}

//	void UpdateScore(int score){
//		scoreText.text = string.Format("{0:00000}", score);
//	}

}
