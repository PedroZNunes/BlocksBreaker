using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D), typeof(AudioSource))]
public class Ball : MonoBehaviour {

	public static event System.Action BallDestroyedEvent;
	public static event System.Action NoBallsLeftEvent;

	public delegate void PowerUpsHandler (Collider2D col, GameObject ball);
	public static event PowerUpsHandler BallCollidedEvent;

	public static int Count = 0;
	[SerializeField] private AudioClip hitBlockClip, hitPlayerClip, hitUnbreakableClip, hitOtherClip, firstLaunchClip;
	private float maxSpeed = 10f;
	private Vector2 minAngles = new Vector2 (0.1f, 0.1f);
	private Vector2 maxAngles = new Vector2 (0.9f, 0.9f);

	private ActionMaster actionMaster;
	private Transform spawnPoint;
	private Rigidbody2D rigidBody;
	private AudioSource audioSource;

	private static List<Ball> balls;

    private void OnEnable() { Subscribe(); }
	private void OnDisable(){ Unsubscribe(); }
	
    private void Awake() { 
		if (balls == null) {
			balls = new List<Ball>();
        }
		balls.Add( this ); 
	}

    void Start () {
		audioSource = GetComponent<AudioSource> ();
		rigidBody = GetComponent<Rigidbody2D> ();
		actionMaster = FindObjectOfType<ActionMaster> ();
		Debug.Assert (actionMaster != null, "Action Master not found by the ball");
	}

	void Update(){
		Debug.DrawRay (transform.position, rigidBody.velocity.normalized);
	}
	
	void OnTriggerEnter2D (Collider2D trigger){
		if (trigger.CompareTag (MyTags.Shredder.ToString())){
			DestroyBall();
		}
	}

	private void DestroyBall() {
		balls.Remove( this );
		if (BallDestroyedEvent != null)
			BallDestroyedEvent();

		if (balls.Count <= 0) {
			if (NoBallsLeftEvent != null)
				NoBallsLeftEvent ();
        }

		Destroy( this.gameObject );
    }


	void Launch(Vector2 direction){
		rigidBody.isKinematic = false;
		transform.parent = GameObject.FindGameObjectWithTag(MyTags.Dynamic.ToString()).transform;
		actionMaster.TriggerPlay ();
		rigidBody.velocity = direction * maxSpeed;
		audioSource.PlayOneShot (firstLaunchClip, 0.5f);
	}


	void OnCollisionEnter2D( Collision2D col ) {
		//Não funciona exatamente como esperado, mas funciona...
		Vector2 normalizedVelocity = new Vector2( 
			Mathf.Clamp( Mathf.Abs( rigidBody.velocity.normalized.x ), minAngles.x, maxAngles.x ) * Mathf.Sign( rigidBody.velocity.x ),
			Mathf.Clamp( Mathf.Abs( rigidBody.velocity.normalized.y ), minAngles.y, maxAngles.y ) * Mathf.Sign( rigidBody.velocity.y )).normalized;
		rigidBody.velocity = normalizedVelocity * maxSpeed;
		if (col.gameObject.CompareTag( MyTags.Block.ToString() )) {
			audioSource.PlayOneShot( hitBlockClip, 0.3f );
			if (BallCollidedEvent != null) {
				BallCollidedEvent( col.collider, gameObject );
			}
		} else if (col.gameObject.CompareTag( MyTags.Unbreakable.ToString() )) {
			audioSource.PlayOneShot( hitUnbreakableClip, 0.4f );
		} else if (col.gameObject.CompareTag( MyTags.Player.ToString() )) {
			audioSource.PlayOneShot( hitPlayerClip, 0.5f );
		} else {
			audioSource.PlayOneShot( hitOtherClip, 0.6f );
		}

		//dealing with angles
		if (col.gameObject.CompareTag( MyTags.Wall.ToString() )) {
			Debug.Log( col.relativeVelocity.ToString() );
			
			//this.rigidBody.velocity = new Vector2 (Mathf.Sign( rigidBody.velocity.x ) * Mathf.Max( this.rigidBody.velocity.x, 1f ), rigidBody.velocity.y);
        }
	}

	private void Subscribe() {
		ActionMaster.LaunchEvent += Launch;
		ActionMaster.EndGameEvent += ResetBalls;
		GUIController.LaunchEvent += Launch;
	}

	private void Unsubscribe() {
		ActionMaster.LaunchEvent -= Launch;
		ActionMaster.EndGameEvent -= ResetBalls;
		GUIController.LaunchEvent -= Launch;
	}

	public static void ResetBalls() {
		if (balls != null) {
			if (balls.Count > 0) {
				balls.Clear();
			}
		}
    }


}
