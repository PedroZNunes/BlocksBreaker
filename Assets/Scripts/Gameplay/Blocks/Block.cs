using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Block : MonoBehaviour
{

    public delegate void PowerUpDropHandler(Vector3 blockPosition, float dropChancePercent);
    public static event PowerUpDropHandler PowerUpDropEvent;
    public static event Action BlockDestroyedEvent;
    public static event Action NoBlocksLeftEvent;

    public delegate void GotHitEventHandler(Collider2D blockCol, Collider2D ballCol);
    public static event GotHitEventHandler GotHit;
    

    public delegate void GettingHitEventHandler(Block block, ref int damageAmount);

    public event GettingHitEventHandler ThisGettingHit;


    public static int Count;

    [SerializeField] private Health health;

    //public int hp;
    [SerializeField] private Sprite[] sprites;
    private Color color;

    //[SerializeField] private int scorePerHit;

    private static readonly float powerUpDropPerHP = 1f;
    private SpriteRenderer spriteRenderer;
    private Vector3 desiredPosition;
    /// <summary>
    /// Offset that affects all blocks so that they show above mid-screen
    /// </summary>
    private Vector3 blockDisplacement = new Vector3(0f, 10f, 0f);

    static private List<Block> blocks;

    void OnValidate()
    {
        if (sprites.Length != health.GetMax())
        {
            sprites = new Sprite[health.GetMax()];
        }


    }

    private void OnEnable()
    {
        GameMaster.EndGameEvent += ResetBlocks;
        LevelManager.LeavingLevelEvent += ResetBlocks;
        health.Initialize();
    }

    private void OnDisable()
    {
        GameMaster.EndGameEvent -= ResetBlocks;
        LevelManager.LeavingLevelEvent -= ResetBlocks;
    }

    void Awake()
    {
        //initialize blocks list
        if (blocks == null)
        {
            blocks = new List<Block>();
        }
        //test if blocks are stacking and if not, aff them to the blocks list
        foreach (Block anotherBlock in blocks)
        {
            if (this.transform.position == anotherBlock.transform.position)
            {
                Debug.Log("Stacking blocks detected at position: " + this.transform.position.ToString());
                Destroy(gameObject);
                return;
            }
        }
        blocks.Add(this);

        //send the block out of the screen for animation purposes
        //desiredPosition = transform.position;
        //transform.position += blockDisplacement;

    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    //public void StartAnimation()
    //{
    //    StartCoroutine(StartAnimationCR());
    //}

    //IEnumerator StartAnimationCR()
    //{
    //    float delayInSeconds = (transform.position.y * 15 / 100) + (transform.position.x * 5 / 100);
    //    Vector3 startPosition = transform.position;
    //    yield return new WaitForSeconds(delayInSeconds);
    //    for (float i = 0f; i <= 1f; i += 0.1f)
    //    {
    //        if (i > 0.9f)
    //            i = 1f;
    //        transform.position = Vector3.Lerp(startPosition, desiredPosition, i);
    //        yield return null;
    //    }
    //    GetComponent<AudioSource>().Play();
    //}

    void OnCollisionEnter2D(Collision2D col)
    {
        //TODO fix this. change to compare layers
        if (col.gameObject.CompareTag(MyTags.Ball.ToString()))
        {
            GetHit();

            if (GotHit != null)
            {
                GotHit(col.otherCollider, col.collider);
            }
        }
    }

    public void GetHit()
    {
        int damageAmount = 1;

        if(ThisGettingHit != null)
        {
            ThisGettingHit (this, ref damageAmount);
        }
        GetHit(damageAmount);
    }

    public void GetHit(int amount)
    {
        health.GetHit(amount);
        bool isDead = health.isDead;

        //		ScoreManager.AddPoints (scorePerHit);
        if (isDead)
        {
            float dropChance = powerUpDropPerHP * health.GetMax();
            if (PowerUpDropEvent != null)
                PowerUpDropEvent(transform.position, dropChance);
            DestroyBlock();
        }
        else
        {
            spriteRenderer.sprite = sprites[health.current - 1];
        }
    }

    void DestroyBlock()
    {
        blocks.Remove(this);
        if (BlockDestroyedEvent != null)
            BlockDestroyedEvent();
        if (blocks.Count <= 0)
        {
            if (NoBlocksLeftEvent != null)
            {
                NoBlocksLeftEvent();
            }
        }
        Destroy(gameObject);
    }

    private void ResetBlocks()
    {
        if (blocks != null)
        {
            if (blocks.Count > 0)
            {
                blocks.Clear();
            }
        }
    }

}
