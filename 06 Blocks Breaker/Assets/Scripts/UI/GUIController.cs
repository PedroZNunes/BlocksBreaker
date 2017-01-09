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
	[SerializeField] private GameObject slideInterface;
	[SerializeField] private Text levelNameText;

	private ActionMaster actionMaster;


	void OnEnable(){
		actionMaster = FindObjectOfType<ActionMaster> ();
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


	void GameStateSwitched (ActionMaster.States currentState){
		switch (currentState){
		case ActionMaster.States.Inactive:
			//do nothing
			break;
		case ActionMaster.States.OpeningAnimations:
			//do nothing
			break;
		case ActionMaster.States.FirstLaunch:
			firstLaunch.gameObject.SetActive (true);
			firstLaunch.ActivateFirstLaunch ();
			slideInterface.SetActive (true);
			break;
		case ActionMaster.States.PlayMode:
			firstLaunch.gameObject.SetActive (false);
			slideInterface.SetActive (true);
			break;
		case ActionMaster.States.NoBallsLeft:
			slideInterface.SetActive (true);
			break;
		case ActionMaster.States.Relaunch:
			firstLaunch.gameObject.SetActive (true);
			firstLaunch.ActivateLaunch ();
			slideInterface.SetActive (true);
			break;
		case ActionMaster.States.Win:
			firstLaunch.gameObject.SetActive (false);
			slideInterface.SetActive (false);
			ShowWinScreen ();
			break;
		case ActionMaster.States.Lose:
			firstLaunch.gameObject.SetActive (false);
			slideInterface.SetActive (false);
			ShowLoseScreen ();
			break;
		case ActionMaster.States.Pause:
			slideInterface.SetActive (false);
			firstLaunch.gameObject.SetActive (false);
			break;
		}
	}

	public void ProcLaunchEvent(Vector2 direction){
		if (LaunchEvent != null)
			LaunchEvent (direction);
		else
			Debug.LogError ("No methods found in launch event");
		actionMaster.TriggerPlayMode();
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
		ActionMaster.GUIPauseMenuEvent += ShowHidePauseMenu;
		ActionMaster.GUIStateChangedEvent += GameStateSwitched;
//		ScoreManager.GUIUpdateScoreEvent += UpdateScore;
	}

	void Unsubscribe(){
		ActionMaster.GUIPauseMenuEvent -= ShowHidePauseMenu;
		ActionMaster.GUIStateChangedEvent -= GameStateSwitched;
//		ScoreManager.GUIUpdateScoreEvent -= UpdateScore;
	}

//	void UpdateScore(int score){
//		scoreText.text = string.Format("{0:00000}", score);
//	}

}
