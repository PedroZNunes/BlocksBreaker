using UnityEngine;
using System;
using System.Collections;

public class ActionMaster : MonoBehaviour {

	public delegate void LaunchHandler (Vector2 direction);
	public static event LaunchHandler LaunchEvent;
	public static event Action StartingEvent;
	public static event Action DefeatEvent;
	public static event Action VictoryEvent;
	public static event Action EndGameEvent;

	public delegate void StateChangedHandler (States newState);
	public static event StateChangedHandler GUIStateChangedEvent;

	public delegate void GUIPauseMenuHandler (string currentState);
	public static event GUIPauseMenuHandler GUIPauseMenuEvent;


	public enum States {Start, OpeningAnimations, FirstLaunch, Play, NoBallsLeft, Relaunch, Victory, Defeat, Pause};

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
	[SerializeField] private GameObject ballPrefab;
	private GameObject ballParent;

	[SerializeField] private GameObject playerPrefab;
	private GameObject playerRespawn;

	private PowerUpManager powerUpManager;
	private Player player;
	private AudioSource audioSource;


	private bool isOpening = false;
	private bool isWinning = false;
	private bool isLosing = false;

    #region Initialization

    void Awake (){

		SpawnPlayer();
		Debug.Assert( player != null, "Player not found in the scene" );

		ballParent = GameObject.FindGameObjectWithTag (MyTags.BallSpawn.ToString());
		Debug.Assert (ballParent != null, "Ball Spawn not found by tag. Should be on player.");

		audioSource = GetComponent<AudioSource> ();
		Debug.Assert (audioSource != null, "Audio Source not found in the scene");

		powerUpManager = FindObjectOfType <PowerUpManager> ();
		Debug.Assert (powerUpManager != null, "Power Up Manager not found in the scene");
	}

	void Start (){
		CurrentGameState = States.Start;
	}

    void OnEnable() { Subscribe (); }

    void OnDisable(){ Unsubscribe (); }

	void Subscribe() {
		Block.NoBlocksLeftEvent += HandleNoBlocksLeft;
		Ball.NoBallsLeftEvent += HandleNoBallsLeft;
	}

	void Unsubscribe() {
		Block.NoBlocksLeftEvent -= HandleNoBlocksLeft;
		Ball.NoBallsLeftEvent -= HandleNoBallsLeft;
	}

	void SpawnPlayer() {
		player = FindObjectOfType<Player>();

		if (player != null) {
			Destroy( player.gameObject );
		}
		else {
			playerRespawn = GameObject.FindGameObjectWithTag( MyTags.PlayerSpawn );
			GameObject playerGO = Instantiate( playerPrefab, playerRespawn.transform );
			player = playerGO.GetComponent<Player>();
		}
	}
	#endregion


    void Update (){ GameStateSwitch (); }


	void GameStateSwitch (){
		switch (CurrentGameState){
		case States.Start:
			HandleStart ();
			break;
		case States.OpeningAnimations:
			HandleOpeningAnimations ();
			break;
		case States.FirstLaunch:
			HandleLaunch ();
			break;
		case States.Play:
			HandlePlayMode ();
			break;
		case States.Relaunch:
			HandleLaunch ();
			break;
		case States.Victory:
			if (isWinning == false)
				StartCoroutine (HandleWin ());
			break;
		case States.Defeat:
			if (isLosing == false)
				HandleLose ();
			break;
		case States.Pause:
			HandlePause ();
			break;
		}
	}


    #region Game State Handlers
    void HandleStart (){
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

		if (Input.GetKeyDown (KeyCode.W)) { //HACK
			TriggerVictory ();
		}
	}

	IEnumerator HandleWin(){
		isWinning = true;
		player.canMove = false;
		yield return new WaitForSeconds (4f);
//		int score = ScoreManager.GetScore ();
//		if (score > PlayerPrefsManager.GetHighScore ()) {
//			PlayerPrefsManager.SetHighScore (score);
//		}
		if (EndGameEvent != null) { EndGameEvent(); }

		LevelManager levelManager = FindObjectOfType<LevelManager> ();
		levelManager.LoadNextLevel ();
	}

	void HandleLose(){
		isLosing = true;
//		int score = ScoreManager.GetScore ();
//		if (score > PlayerPrefsManager.GetHighScore ()) {
//			PlayerPrefsManager.SetHighScore (score);
//		}
		player.canMove = false;
		if (EndGameEvent != null) { EndGameEvent(); }
	}

	void HandlePause (){
		Time.timeScale = 0f;
		if (GUIPauseMenuEvent != null)
			GUIPauseMenuEvent (CurrentGameState.ToString ());
		if (Input.GetButtonDown ("Cancel")) {
			HandleUnpause ();
		}
	}

	void HandleUnpause() {
		if (GUIPauseMenuEvent != null)
			GUIPauseMenuEvent( previousGameState.ToString() );
		Time.timeScale = 1f;
		if (previousGameState == States.Play)
			TriggerPlay();
		else if (previousGameState == States.FirstLaunch)
			TriggerFirstLaunch();
	}
	#endregion



	#region Event Handlers
	private void HandleNoBallsLeft() {
		Debug.Log( "Handling No Balls Left" );
		player.TakeHit();

		if (CheckForDefeat()) {
			TriggerDefeat();
        }
        else {
			RespawnBall();
			player.canMove = true;
			TriggerLaunch();
		}
	}

	private void HandleNoBlocksLeft() {
		if (CurrentGameState == States.Play || CurrentGameState == States.Relaunch) {
			Debug.Log( "Triggered Win by lack of blocks" );
			TriggerVictory();
		}
	}
    #endregion


    #region Auxiliar Functions
    IEnumerator OpeningAnimations() {
		isOpening = true;
		player.canMove = false;
		Block[] blocks = FindObjectsOfType<Block>();
		for (int i = 0; i < blocks.Length; i++) {
			blocks[i].StartAnimation();
		}
		audioSource.Play();
		//player animation is automatic.
		yield return new WaitForSeconds( 2.5f );
		RespawnBall();
		player.canMove = true;
		TriggerFirstLaunch();
	}

	private void RespawnBall(){
		GameObject ball = Instantiate (ballPrefab, ballParent.transform.position, ballParent.transform.rotation, ballParent.transform) as GameObject;
		ball.GetComponent<Rigidbody2D> ().isKinematic = true;
	}

	private bool CheckForDefeat(){
		return (player.hp <= 0);
	}

	#endregion


	#region Triggers
	public void TriggerOpeningAnimations (){
		CurrentGameState = States.OpeningAnimations;
		Debug.Log ("Handling Opening Animations");
	}

	public void TriggerFirstLaunch(){
		CurrentGameState = States.FirstLaunch;
		Debug.Log ("Handling First Launch");
	}

	public void TriggerPlay (){
		CurrentGameState = States.Play;
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

	public void TriggerVictory(){
		CurrentGameState = States.Victory;
		if (VictoryEvent != null)
			VictoryEvent();
		Debug.Log ("Handling Win");
	}

	public void TriggerDefeat(){
		CurrentGameState = States.Defeat;
		if (DefeatEvent != null)
			DefeatEvent ();
		Debug.Log ("Handling Lose");
	}

	public void TriggerPause(){
		previousGameState = CurrentGameState;
		CurrentGameState = States.Pause;
		Debug.Log ("Handling Pause");
	}
    #endregion
}
