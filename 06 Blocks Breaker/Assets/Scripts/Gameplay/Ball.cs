using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(AudioSource))]
public class Ball : MonoBehaviour {

	public static event System.Action BallDestroyedEvent;

	public delegate void PowerUpsHandler (Collider2D col, GameObject ball);
	public static event PowerUpsHandler BallCollidedEvent;

	public static int Count = 0;
	[SerializeField] private AudioClip hitBlockClip, hitPlayerClip, hitUnbreakableClip, hitOtherClip, firstLaunchClip;
	[SerializeField] private float maxSpeed = 8f;
	[SerializeField] private Vector2 minAngles = new Vector2 (0f, 0f);
	[SerializeField] private Vector2 maxAngles = new Vector2 (1f, 1f);

	private ActionMaster actionMaster;
	private Transform spawnPoint;
	private Rigidbody2D rigidBody;
	private AudioSource audioSource;

	void OnDisable(){ ActionMaster.LaunchEvent -= Launch; GUIController.LaunchEvent -= Launch;}

	void Start () {
		audioSource = GetComponent<AudioSource> ();
		rigidBody = GetComponent<Rigidbody2D> ();
		actionMaster = FindObjectOfType<ActionMaster> ();
		Debug.Assert (actionMaster != null, "Action Master not found by the ball");
		Count++;
		GUIController.LaunchEvent += Launch;
		ActionMaster.LaunchEvent += Launch;
	}

	void Update(){
		Debug.DrawRay (transform.position, rigidBody.velocity.normalized);
	}
	
	void OnTriggerEnter2D (Collider2D trigger){
		if (trigger.CompareTag (MyTags.Shredder.ToString())){
			Destroy (gameObject);
		}
	}

	void OnDestroy(){
		Count--;
		if (BallDestroyedEvent != null)
			BallDestroyedEvent ();
	}

	void Launch(Vector2 direction){
		rigidBody.isKinematic = false;
		transform.parent = GameObject.FindGameObjectWithTag(MyTags.Dynamic.ToString()).transform;
		actionMaster.TriggerPlayMode ();
		rigidBody.velocity = direction * maxSpeed;
		audioSource.PlayOneShot (firstLaunchClip, 0.5f);
	}


	void OnCollisionEnter2D(Collision2D col){
		//Não funciona exatamente como esperado, mas funciona...
		Vector2 normalizedVelocity = new Vector2 ( Mathf.Clamp (Mathf.Abs(rigidBody.velocity.normalized.x), minAngles.x, maxAngles.x) * Mathf.Sign(rigidBody.velocity.x), 
				Mathf.Clamp (Mathf.Abs(rigidBody.velocity.normalized.y), minAngles.y, maxAngles.y) * Mathf.Sign(rigidBody.velocity.y)).normalized;
		rigidBody.velocity = normalizedVelocity * maxSpeed;
		if (col.gameObject.CompareTag (MyTags.Block.ToString ())) {
			audioSource.PlayOneShot (hitBlockClip, 0.3f);
			if (BallCollidedEvent != null) {
				BallCollidedEvent (col.collider, gameObject);
			}
		}else if (col.gameObject.CompareTag(MyTags.Unbreakable.ToString())){
			audioSource.PlayOneShot (hitUnbreakableClip, 0.4f);
		}else if (col.gameObject.CompareTag(MyTags.Player.ToString())){
			audioSource.PlayOneShot (hitPlayerClip, 0.5f);
		}else{
			audioSource.PlayOneShot (hitOtherClip, 0.6f);
		}
	}


}
