using UnityEngine;
using System.Collections;

public class ActionMaster : MonoBehaviour {

	public delegate void LaunchHandler (Vector2 direction);
	public static event LaunchHandler LaunchEvent;
	public static event System.Action GUILoseEvent;
	public static event System.Action GUIWinEvent;
	public static event System.Action GUIFirstLaunchEvent;

	public delegate void StateChangedHandler (States newState);
	public static event StateChangedHandler GUIStateChangedEvent;

	public delegate void GUIPauseMenuHandler (string currentState);
	public static event GUIPauseMenuHandler GUIPauseMenuEvent;


	public enum States {Inactive, OpeningAnimations, FirstLaunch, PlayMode, NoBallsLeft, Relaunch, Win, Lose, Pause};

	private States currentGameState;
	public States previousGameState;
	public States CurrentGameState { 
		get{ return currentGameState; }
		private set{ 
			currentGameState = value;
			if (GUIStateChangedEvent != null)
				GUIStateChangedEvent (currentGameState);
		}
	}

	private PowerUpManager powerUpManager;
	private Player player;
	private GameManager gameManager;
	private AudioSource audioSource;

	private bool isOpening = false;
	private bool isWinning = false;


	void Awake (){

	}

	void Start (){
		player = FindObjectOfType<Player>();
		Debug.Assert (player != null, "Player not found in the scene");

		gameManager = FindObjectOfType<GameManager>();
		Debug.Assert (gameManager != null, "Game Manager not found in the scene");

		audioSource = GetComponent<AudioSource> ();
		Debug.Assert (audioSource != null, "Audio Source not found in the scene");

		powerUpManager = FindObjectOfType <PowerUpManager> ();
		Debug.Assert (powerUpManager != null, "Power Up Manager not found in the scene");

		CurrentGameState = States.Inactive;
		Subscribe ();
	}

	void OnDisable(){ Unsubscribe (); }


	void Update (){
		GameStateSwitch ();
	}


	void GameStateSwitch (){
		switch (CurrentGameState){
		case States.Inactive:
			HandleInactive ();
			break;
		case States.OpeningAnimations:
			HandleOpeningAnimations ();
			break;
		case States.FirstLaunch:
			HandleLaunch ();
			break;
		case States.PlayMode:
			HandlePlayMode ();
			break;
		case States.NoBallsLeft:
			HandleNoBallsLeft ();
			break;
		case States.Relaunch:
			HandleLaunch ();
			break;
		case States.Win:
			if (isWinning == false)
				StartCoroutine (HandleWin ());
			break;
		case States.Lose:
			HandleLose ();
			break;
		case States.Pause:
			HandlePause ();
			break;
		}
	}

	void HandleInactive (){
		TriggerOpeningAnimations ();
	}

	void HandleOpeningAnimations(){
		if (isOpening == false) {
			Block[] blocks = FindObjectsOfType<Block> ();
			int totalHits = 0;
			for (int i = 0; i<blocks.Length ; i++){
				totalHits += blocks[i].hp;	
			}
			print ("Total Hits needed: " + totalHits);
			StartCoroutine (OpeningAnimations ());
		}
	}

	IEnumerator OpeningAnimations ()
	{
		isOpening = true;
		player.canMove = false;
		Block[] blocks = FindObjectsOfType<Block> ();
		for (int i = 0; i < blocks.Length; i++) {
			blocks [i].StartAnimation ();
		}
		audioSource.Play ();
		//player animation is automatic.
		yield return new WaitForSeconds (2.5f);
		gameManager.RespawnBall ();
		player.canMove = true;
		TriggerFirstLaunch ();
	}

	void HandleLaunch(){

		//check if there is UI dealing with the first launch. if not, do it automatically
		if (FindObjectOfType<GUIController> () == null) {
			if (Input.anyKeyDown) {
				Debug.LogWarning ("No GUI controller.");
				if (LaunchEvent != null)
					LaunchEvent (new Vector2 (1f, 2f).normalized);
				else
					Debug.LogWarning ("Launch Event has no methods");
			}
		}


		if (Input.GetButtonDown ("Cancel")) {
			TriggerPause ();
		}

		if (Ball.Count > 1) {
			Debug.Break ();
			Debug.LogWarning ("More than one ball detected in First Launch Mode");
		}
	}

	void HandlePlayMode () {
		player.canMove = true;
		if (Input.GetButtonDown ("Cancel")) {
			TriggerPause ();
		}

		powerUpManager.IncrementTimer ();

//		if (Input.GetKeyDown (KeyCode.W)) { //HACK
//			TriggerWin ();
//		}
	}

	void HandleNoBallsLeft (){
		Debug.Log ("Handling No Balls Left");
		player.TakeHit ();
		if (gameManager.DeathCheck () == false) {
			Debug.Log (CurrentGameState);
			gameManager.RespawnBall ();
			player.canMove = true;
			TriggerLaunch ();
		} else {
			Debug.Log ("Player dead, current state: " + CurrentGameState);
		}
	}


	IEnumerator HandleWin(){
		isWinning = true;
		player.canMove = false;
		PlayerPrefsManager.SetLastLevelPlayed (UnityEngine.SceneManagement.SceneManager.GetActiveScene ().buildIndex);
		MusicPlayer musicPlayer = FindObjectOfType<MusicPlayer> ();
		yield return new WaitForSeconds (4f);
		int score = ScoreManager.GetScore ();
		if (score > PlayerPrefsManager.GetHighScore ()) {
			PlayerPrefsManager.SetHighScore (score);
		}
		LevelManager levelManager = FindObjectOfType<LevelManager> ();
		levelManager.LoadNextLevel ();
	}


	void HandleLose(){
		int score = ScoreManager.GetScore ();
		if (score > PlayerPrefsManager.GetHighScore ()) {
			PlayerPrefsManager.SetHighScore (score);
		}
		player.canMove = false;
	}

	void HandlePause (){
		Time.timeScale = 0f;
		if (GUIPauseMenuEvent != null)
			GUIPauseMenuEvent (CurrentGameState.ToString ());
		if (Input.GetButtonDown ("Cancel")) {
			Unpause ();
		}
	}



	void CountBalls(){
		if (Ball.Count <= 0 && CurrentGameState == States.PlayMode) {
			TriggerNoBallsLeft ();
		}
	}

	void CountBlocks(){
		Debug.Log ("Number of Blocks by Block.Count: " + Block.Count);
		if (Block.Count == 5) {
			int realNumberOfBlocks = FindObjectsOfType<Block> ().Length;
			Debug.Log ("Number of Blocks by counting objects: " + realNumberOfBlocks);
		}
		if (Block.Count <= 0f && CurrentGameState == States.PlayMode) {
			Debug.Log ("Triggered Win by lack of blocks");
			TriggerWin ();
		}
	}


	public void Unpause ()
	{
		if (GUIPauseMenuEvent != null)
			GUIPauseMenuEvent (previousGameState.ToString ());
		Time.timeScale = 1f;
		if (previousGameState == States.PlayMode)
			TriggerPlayMode ();
		else if (previousGameState == States.FirstLaunch)
			TriggerFirstLaunch ();
	}

	void Subscribe(){
		Block.BlockDestroyedEvent += CountBlocks;
		Ball.BallDestroyedEvent += CountBalls;
	}

	void Unsubscribe (){
		Block.BlockDestroyedEvent -= CountBlocks;
		Ball.BallDestroyedEvent -= CountBalls;
	}
		


	public void TriggerOpeningAnimations (){
		CurrentGameState = States.OpeningAnimations;
		Debug.Log ("Handling Opening Animations");
	}

	public void TriggerFirstLaunch(){
		CurrentGameState = States.FirstLaunch;
		Debug.Log ("Handling First Launch");
	}

	public void TriggerPlayMode (){
		CurrentGameState = States.PlayMode;
		Debug.Log ("Handling Play Mode");
	}

	public void TriggerNoBallsLeft(){
		CurrentGameState = States.NoBallsLeft;
		Debug.Log ("Handling No Balls Left");
	}


	public void TriggerLaunch (){
		CurrentGameState = States.Relaunch;
		Debug.Log ("Handling Relaunch");
	}

	public void TriggerWin(){
		CurrentGameState = States.Win;
		Debug.Log ("Handling Win");
	}

	public void TriggerLose(){
		CurrentGameState = States.Lose;
		Debug.Log ("Handling Lose");
	}

	public void TriggerPause(){
		previousGameState = CurrentGameState;
		CurrentGameState = States.Pause;
		Debug.Log ("Handling Pause");
	}
}
