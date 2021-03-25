using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(AudioSource))]
public class Ball : MonoBehaviour
{

    public static event System.Action BallDestroyed;
    public static event System.Action NoBallsLeft;



    public static int Count = 0;
    [SerializeField] private AudioClip hitBlockClip, hitPlayerClip, hitUnbreakableClip, hitOtherClip, firstLaunchClip;
    [SerializeField] private  IntegerVariable speed;
    private Vector2 minAngles = new Vector2(0.1f, 0.1f);
    private Vector2 maxAngles = new Vector2(0.9f, 0.9f);

    private GameMaster actionMaster;
    private readonly Transform spawnPoint;
    private Rigidbody2D rigidBody;
    private AudioSource audioSource;

    [SerializeField] private BoundsVariable gameArea;

    private static List<Ball> balls;

    private void OnEnable() { Subscribe(); }
    private void OnDisable() { Unsubscribe(); }

    private void Awake()
    {
        if (balls == null)
        {
            balls = new List<Ball>();
        }
        balls.Add(this);
    }


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rigidBody = GetComponent<Rigidbody2D>();
        actionMaster = FindObjectOfType<GameMaster>();
        Debug.Assert(actionMaster != null, "Action Master not found by the ball");
    }


    void Update()
    {
        CheckBorders();
        Debug.DrawRay(transform.position, rigidBody.velocity.normalized);
    }


    void OnTriggerEnter2D(Collider2D trigger)
    {
        if (trigger.CompareTag(MyTags.Shredder.ToString()))
        {
            DestroyBall();
        }
    }


    public void DestroyBall()
    {
        balls.Remove(this);
        if (BallDestroyed != null)
            BallDestroyed();

        if (balls.Count <= 0)
        {
            if (NoBallsLeft != null)
                NoBallsLeft();
        }

        Destroy(this.gameObject);
    }


    void Launch(Vector2 direction)
    {
        rigidBody.isKinematic = false;
        transform.parent = GameObject.FindGameObjectWithTag(MyTags.Dynamic.ToString()).transform;
        actionMaster.TriggerPlay();
        rigidBody.velocity = direction * speed.value;
        audioSource.PlayOneShot(firstLaunchClip, 0.5f);
    }


    void CheckBorders()
    {
        //Vector2 screenExtents = new Vector2(Camera.main.orthographicSize * Camera.main.aspect, Camera.main.orthographicSize);
        Bounds ballBounds = GetComponent<CircleCollider2D>().bounds;
        Debug.Assert(ballBounds != null, "Collider bounds not found in the ball");

        Vector2 movementAreaExtents = new Vector2();

        //movementAreaExtents.x = (screenExtents.x - (ballBounds.extents.x * transform.localScale.x));
        movementAreaExtents.x = (gameArea.value.extents.x - (ballBounds.extents.x * transform.localScale.x));
        //movementAreaExtents.y = (screenExtents.y - (ballBounds.extents.y * transform.localScale.y));
        movementAreaExtents.y = (gameArea.value.extents.y - (ballBounds.extents.y * transform.localScale.y));

        if (Mathf.Abs(transform.position.x) > movementAreaExtents.x)
        {
            rigidBody.velocity = new Vector2(-Mathf.Sign(transform.position.x) * Mathf.Abs(rigidBody.velocity.x), rigidBody.velocity.y);
        }
        if (Mathf.Abs(transform.position.y) > movementAreaExtents.y)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, -Mathf.Sign(transform.position.y) * Mathf.Abs(rigidBody.velocity.y));
        }
    }


    void OnCollisionEnter2D(Collision2D col)
    {
        //Não funciona exatamente como esperado, mas funciona...
        Vector2 normalizedVelocity = new Vector2(
            Mathf.Clamp(Mathf.Abs(rigidBody.velocity.normalized.x), minAngles.x, maxAngles.x) * Mathf.Sign(rigidBody.velocity.x),
            Mathf.Clamp(Mathf.Abs(rigidBody.velocity.normalized.y), minAngles.y, maxAngles.y) * Mathf.Sign(rigidBody.velocity.y)).normalized;
        rigidBody.velocity = normalizedVelocity * speed.value;
        
        if (col.gameObject.CompareTag(MyTags.Block.ToString()))
        {
            audioSource.PlayOneShot(hitBlockClip, 0.3f);
        }
        else if (col.gameObject.CompareTag(MyTags.Unbreakable.ToString()))
        {
            audioSource.PlayOneShot(hitUnbreakableClip, 0.4f);
        }
        else if (col.gameObject.CompareTag(MyTags.Player.ToString()))
        {
            audioSource.PlayOneShot(hitPlayerClip, 0.5f);
        }
        else
        {
            audioSource.PlayOneShot(hitOtherClip, 0.6f);
        }

        //dealing with angles
        if (col.gameObject.CompareTag(MyTags.Wall.ToString()))
        {
            Debug.Log(col.relativeVelocity.ToString());

            //this.rigidBody.velocity = new Vector2 (Mathf.Sign( rigidBody.velocity.x ) * Mathf.Max( this.rigidBody.velocity.x, 1f ), rigidBody.velocity.y);
        }
    }


    private void Subscribe()
    {
        GameMaster.LaunchEvent += Launch;
        GameMaster.EndGameEvent += ResetBalls;
        LevelManager.LeavingLevelEvent += ResetBalls;
        GUIController.LaunchEvent += Launch;
    }

    private void Unsubscribe()
    {
        GameMaster.LaunchEvent -= Launch;
        GameMaster.EndGameEvent -= ResetBalls;
        LevelManager.LeavingLevelEvent -= ResetBalls;
        GUIController.LaunchEvent -= Launch;
    }

    public static void ResetBalls()
    {
        if (balls != null)
        {
            if (balls.Count > 0)
            {
                balls.Clear();
            }
        }
    }


}
