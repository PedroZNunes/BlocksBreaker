﻿using System.Collections;
using UnityEngine;

public class StatsTracker : MonoBehaviour
{

    private int ballsUsed = 0;
    private int deaths = 0;
    private int powerUps = 0;
    private int blocksDestroyed = 0;

    private int oldBallsUsed = 0;
    private int oldDeaths = 0;
    private int oldPowerUps = 0;
    private int oldBlocksDestroyed = 0;


    void CountRemainingBalls()
    {
        Ball[] balls = FindObjectsOfType<Ball>();
        Debug.Log("Balls Remaining In Game: " + balls.Length);
        ballsUsed += balls.Length;
    }

    void IncrementBallsUsed() { ballsUsed++; }
    void IncrementDeaths() { deaths++; }
    void IncrementPowerUps(PowerUpsEnum name) { powerUps++; }
    void IncrementBlocksDestroyed() { blocksDestroyed++; }
    void UpdateAllStatsWrapper() { StartCoroutine(UpdateAllStats()); }

    IEnumerator UpdateAllStats()
    {
        CountRemainingBalls();
        GetData();
        AssignVariables();
        Debug.Log("Starting to Save...");
        SaveAllData();
        yield return null;
    }

    void GetData()
    {
        oldBallsUsed = PlayerPrefsManager.Get_BallsUsed();
        oldDeaths = PlayerPrefsManager.Get_Deaths();
        oldBlocksDestroyed = PlayerPrefsManager.Get_BlocksDestroyed();
        oldPowerUps = PlayerPrefsManager.Get_PowerUps();
    }

    void SaveAllData()
    {
        PlayerPrefsManager.Set_Stats(ballsUsed, deaths, blocksDestroyed, powerUps);
    }

    void AssignVariables()
    {
        ballsUsed += oldBallsUsed;
        deaths += oldDeaths;
        blocksDestroyed += oldBlocksDestroyed;
        powerUps += oldPowerUps;
    }


    void OnEnable()
    {
        Ball.BallDestroyed += IncrementBallsUsed;
        Block.BlockDestroyedEvent += IncrementBlocksDestroyed;
        PickUpItem.PowerUpPickedUpEvent += IncrementPowerUps;
        GameMaster.DefeatEvent += IncrementDeaths;
        GameMaster.DefeatEvent += UpdateAllStatsWrapper;
        GameMaster.VictoryEvent += UpdateAllStatsWrapper;
    }

    void OnDisable()
    {
        Ball.BallDestroyed -= IncrementBallsUsed;
        Block.BlockDestroyedEvent -= IncrementBlocksDestroyed;
        PickUpItem.PowerUpPickedUpEvent -= IncrementPowerUps;
        GameMaster.DefeatEvent -= IncrementDeaths;
        GameMaster.DefeatEvent -= UpdateAllStatsWrapper;
        GameMaster.VictoryEvent -= UpdateAllStatsWrapper;
    }
}
