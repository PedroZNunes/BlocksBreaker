using System;
using System.Collections;
using UnityEngine;

public class GameMaster : MonoBehaviour
{

    public delegate void LaunchHandler(Vector2 direction);
    public static event LaunchHandler LaunchEvent;
    public static event Action StartingEvent;
    public static event Action DefeatEvent;
    public static event Action VictoryEvent;
    public static event Action EndGameEvent;

    public delegate void StateChangedHandler(States newState);
    public static event StateChangedHandler GUIStateChangedEvent;

    public delegate void GUIPauseMenuHandler(string currentState);
    public static event GUIPauseMenuHandler GUIPauseMenuEvent;


    public enum States { Start, OpeningAnimations, FirstLaunch, Play, NoBallsLeft, Relaunch, Victory, Defeat, Pause };

    private States currentGameState;
    public States previousGameState;
    public States CurrentGameState
    {
        get { return currentGameState; }
        private set
        {
            currentGameState = value;
            if (GUIStateChangedEvent != null)
                GUIStateChangedEvent(currentGameState);
        }
    }
    [SerializeField] private GameObject ballPrefab;
    private readonly GameObject ballParent;

    [SerializeField] private GameObject playerPrefab;
    private GameObject playerRespawn;
    private Player player;

    [SerializeField] private PowerUpManager powerUpManager;
    [SerializeField] private AudioSource BackgroundAudioSource;

    [SerializeField] private BooleanVariable isMovementAllowed;
    [SerializeField] private BooleanVariable isShootingAllowed;
    [SerializeField] private BooleanVariable canPlayerGetHit;


    private bool isOpening = false;
    private bool isWinning = false;
    private bool isLosing = false;

    #region Initialization

    void Awake()
    {
        SpawnPlayer();
        Debug.Assert(player != null, "Player not found in the scene");
    }

    void Start()
    {
        CurrentGameState = States.Start;
    }

    void OnEnable() { Subscribe(); }

    void OnDisable() { Unsubscribe(); }

    void Subscribe()
    {
        Block.NoBlocksLeftEvent += HandleNoBlocksLeft;
        Ball.NoBallsLeftEvent += HandleNoBallsLeft;
        OptionsController.UnpauseEvent += HandleUnpause;
    }

    void Unsubscribe()
    {
        Block.NoBlocksLeftEvent -= HandleNoBlocksLeft;
        Ball.NoBallsLeftEvent -= HandleNoBallsLeft;
        OptionsController.UnpauseEvent -= HandleUnpause;

    }
    //todo: get a playermanager to do this. get a ball manager to spawn the ball as well.
    void SpawnPlayer()
    {
        player = FindObjectOfType<Player>();

        if (player != null)
        {
            Destroy(player.gameObject);
        }

        playerRespawn = GameObject.FindGameObjectWithTag(MyTags.PlayerSpawn);
        GameObject playerGO = Instantiate(playerPrefab, playerRespawn.transform);
        player = playerGO.GetComponent<Player>();
    }
    #endregion


    void Update() { GameStateSwitch(); }


    void GameStateSwitch()
    {
        switch (CurrentGameState)
        {
            case States.Start:
                HandleStart();
                break;
            case States.OpeningAnimations:
                HandleOpeningAnimations();
                break;
            case States.FirstLaunch:
                HandleLaunch();
                break;
            case States.Play:
                HandlePlayMode();
                break;
            case States.Relaunch:
                HandleLaunch();
                break;
            case States.Victory:
                if (isWinning == false)
                    StartCoroutine(HandleWin());
                break;
            case States.Defeat:
                if (isLosing == false)
                    HandleLose();
                break;
            case States.Pause:
                HandlePause();
                break;
        }
    }


    #region Game State Handlers
    void HandleStart()
    {
        TriggerOpeningAnimations();
    }

    void HandleOpeningAnimations()
    {
        if (isOpening == false)
        {
            //Block[] blocks = FindObjectsOfType<Block> ();
            //int totalHits = 0;
            //for (int i = 0; i<blocks.Length ; i++){
            //	totalHits += blocks[i].hp;	
            //}
            //print ("Total Hits needed: " + totalHits);
            isMovementAllowed.value = false;
            isShootingAllowed.value = false;
            canPlayerGetHit.value = false;

            StartCoroutine(OpeningAnimations());
        }
    }

    void HandleLaunch()
    {
        //isMovementAllowed.value = true;
        isShootingAllowed.value = false;
        canPlayerGetHit.value = false;

        //check if there is UI dealing with the first launch. if not, do it automatically
        if (FindObjectOfType<GUIController>() == null)
        {
            if (Input.anyKeyDown)
            {
                Debug.LogWarning("No GUI controller.");
                if (LaunchEvent != null)
                    LaunchEvent(new Vector2(1f, 2f).normalized);
                else
                    Debug.LogWarning("Launch Event has no methods");
            }
        }


        if (Input.GetButtonDown("Cancel"))
        {
            TriggerPause();
        }

        if (Ball.Count > 1)
        {
            Debug.Break();
            Debug.LogWarning("More than one ball detected in First Launch Mode");
        }
    }

    void HandlePlayMode()
    {
        isMovementAllowed.value = true;
        isShootingAllowed.value = true;
        canPlayerGetHit.value = true;

        if (Input.GetButtonDown("Cancel"))
        {
            TriggerPause();
        }

        powerUpManager.IncrementTimer();

        if (Input.GetKeyDown(KeyCode.W))
        { //HACK
            TriggerVictory();
        }
    }

    IEnumerator HandleWin()
    {
        isWinning = true;
        isMovementAllowed.value = false;
        isShootingAllowed.value = false;
        canPlayerGetHit.value = false;

        yield return new WaitForSeconds(4f);
        //		int score = ScoreManager.GetScore ();
        //		if (score > PlayerPrefsManager.GetHighScore ()) {
        //			PlayerPrefsManager.SetHighScore (score);
        //		}
        if (EndGameEvent != null) { EndGameEvent(); }

        LevelManager.LoadNextLevel();
    }

    void HandleLose()
    {
        isLosing = true;
        isMovementAllowed.value = false;
        isShootingAllowed.value = false;
        canPlayerGetHit.value = false;

        //		int score = ScoreManager.GetScore ();
        //		if (score > PlayerPrefsManager.GetHighScore ()) {
        //			PlayerPrefsManager.SetHighScore (score);
        //		}
        if (EndGameEvent != null) { EndGameEvent(); }
    }

    void HandlePause()
    {
        isMovementAllowed.value = false;
        Time.timeScale = 0f;
        if (GUIPauseMenuEvent != null)
            GUIPauseMenuEvent(CurrentGameState.ToString());
        if (Input.GetButtonDown("Cancel"))
        {
            HandleUnpause();
        }
    }

    void HandleUnpause()
    {
        isMovementAllowed.value = true;
        if (GUIPauseMenuEvent != null)
            GUIPauseMenuEvent(previousGameState.ToString());
        Time.timeScale = 1f;
        if (previousGameState == States.Play)
            TriggerPlay();
        else if (previousGameState == States.FirstLaunch)
            TriggerFirstLaunch();
    }
    #endregion



    #region Event Handlers
    private void HandleNoBallsLeft()
    {
        Debug.Log("Handling No Balls Left");
        if (!canPlayerGetHit.value)
            return;

        player.TakeHit();

        if (CheckForDefeat())
        {
            TriggerDefeat();
        }
        else
        {
            RespawnBall();
            isMovementAllowed.value = true;
            TriggerLaunch();
        }
    }

    private void HandleNoBlocksLeft()
    {
        if (CurrentGameState == States.Play || CurrentGameState == States.Relaunch)
        {
            Debug.Log("Triggered Win by lack of blocks");
            TriggerVictory();
        }
    }

    #endregion


    #region Auxiliar Functions
    IEnumerator OpeningAnimations()
    {
        isOpening = true;
        isMovementAllowed.value = false;
        BackgroundAudioSource.Play();
        //player animation is automatic.
        yield return new WaitForSeconds(2.5f);
        RespawnBall();
        isMovementAllowed.value = true;
        TriggerFirstLaunch();
    }

    private void RespawnBall()
    {
        GameObject ball = Instantiate(ballPrefab, player.ballRespawn.position, player.ballRespawn.rotation, player.ballRespawn) as GameObject;
        ball.GetComponent<Rigidbody2D>().isKinematic = true;
    }

    private bool CheckForDefeat()
    {
        return (player.isDead);
    }

    #endregion


    #region Triggers
    public void TriggerOpeningAnimations()
    {
        CurrentGameState = States.OpeningAnimations;
        Debug.Log("Handling Opening Animations");
    }

    public void TriggerFirstLaunch()
    {
        CurrentGameState = States.FirstLaunch;
        Debug.Log("Handling First Launch");
    }

    public void TriggerPlay()
    {
        CurrentGameState = States.Play;
        Debug.Log("Handling Play Mode");
    }

    public void TriggerNoBallsLeft()
    {
        CurrentGameState = States.NoBallsLeft;
        Debug.Log("Handling No Balls Left");
    }


    public void TriggerLaunch()
    {
        CurrentGameState = States.Relaunch;
        Debug.Log("Handling Relaunch");
    }

    public void TriggerVictory()
    {
        CurrentGameState = States.Victory;
        if (VictoryEvent != null)
            VictoryEvent();
        Debug.Log("Handling Win");
    }

    public void TriggerDefeat()
    {
        CurrentGameState = States.Defeat;
        if (DefeatEvent != null)
            DefeatEvent();
        Debug.Log("Handling Lose");
    }

    public void TriggerPause()
    {
        previousGameState = CurrentGameState;
        CurrentGameState = States.Pause;
        Debug.Log("Handling Pause");
    }
    #endregion
}
