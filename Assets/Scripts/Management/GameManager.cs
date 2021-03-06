﻿using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] private GameObject ballPrefab;

    private Player player;
    private GameMaster actionMaster;
    private GameObject ballParent;


    void Awake()
    {
        player = FindObjectOfType<Player>();
        Debug.Assert(player != null, "Player not found in the scene");

        ballParent = GameObject.FindGameObjectWithTag(MyTags.BallSpawn.ToString());
        Debug.Assert(ballParent != null, "Ball Spawn not found by tag. Should be on player.");

        actionMaster = GetComponent<GameMaster>();
        Debug.Assert(actionMaster != null, "ActionMaster not found in the scene");
    }
    void Start()
    {

    }

    public void RespawnBall()
    {
        GameObject ball = Instantiate(ballPrefab, ballParent.transform.position, ballParent.transform.rotation, ballParent.transform) as GameObject;
        ball.GetComponent<Rigidbody2D>().isKinematic = true;
    }

    //TODO fix. game master should not have a reference to the player.
    public bool DeathCheck()
    {
        if (player.isDead)
        {
            actionMaster.TriggerDefeat();
            return true;
        }
        else
        {
            return false;
        }
    }
}
