using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour {

	public delegate void PowerUpsDropHandler (Vector3 blockPosition, float dropChancePercent);
	public static event PowerUpsDropHandler PowerUpDropEvent;
	public static event System.Action BlockDestroyedEvent;

	public static int Count;

	public int hp;
	[SerializeField] private Sprite[] sprites;

	[SerializeField] private int scorePerHit;

	static private float powerUpDropChancePercent = 0.3f;
	private SpriteRenderer spriteRenderer;
	private Vector3 desiredPosition;
	private Vector3 blockDisplacement = new Vector3 (0f, 10f, 0f);


	void OnValidate(){
		if (sprites.Length != hp) {
			sprites = new Sprite[hp];
		}
	}

	void Awake(){
		desiredPosition = transform.position;
		transform.position += blockDisplacement;
		Count = 0;
	}

	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
		Count++;
	}

	public void StartAnimation(){
		StartCoroutine (StartAnimationCR ());
	}

	IEnumerator StartAnimationCR(){
		float delayInSeconds = (transform.position.y * 15 / 100) + (transform.position.x * 5 / 100);
		Vector3 startPosition = transform.position;
		yield return new WaitForSeconds (delayInSeconds);
		for ( float i = 0f; i<=1f; i+= 0.1f) {
			if (i > 0.9f)
				i = 1f;
			transform.position = Vector3.Lerp (startPosition, desiredPosition, i);
			yield return null;
		}
		GetComponent<AudioSource> ().Play ();
	}

	void OnCollisionEnter2D(Collision2D col){
		if (col.gameObject.CompareTag(MyTags.Ball.ToString())) {
			TakeHit ();
		}
	}

	public void TakeHit(){
		if (PowerUpDropEvent != null)
			PowerUpDropEvent (transform.position, powerUpDropChancePercent);
		hp--;
//		ScoreManager.AddPoints (scorePerHit);
		if (hp <= 0) {
			DestroyBlock ();
		} else {
			spriteRenderer.sprite = sprites [hp-1];
		}
	}

	void DestroyBlock(){
		Count--;
		if (BlockDestroyedEvent != null)
			BlockDestroyedEvent ();
		Destroy (gameObject);
	}


}
