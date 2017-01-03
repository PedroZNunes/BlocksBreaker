using UnityEngine;
using System.Collections;
[RequireComponent (typeof(Collider2D), typeof(SpriteRenderer))]
public class PickUpItem : MonoBehaviour {

	static public event System.Action PowerUpPickedUpEvent;
	private float speed = 4f;
	[SerializeField] private AudioClip soundEffect;
	private AudioSource audioSource;
	[SerializeField] private PowerUps powerUp;
	private float defaultPitch;
	private float pitchVariance;

	void Start(){

		Debug.Assert (powerUp != null, "PowerUp not set");
		Debug.Assert (soundEffect != null, "soundEffect not found");

		audioSource = GetComponentInChildren<AudioSource> ();

	}

	void Update(){
		transform.Translate (Vector2.down * speed * Time.deltaTime);
	}
		

	void OnTriggerEnter2D (Collider2D col){
		if (col.CompareTag (MyTags.Player.ToString ())) {
			if (PowerUpPickedUpEvent != null)
				PowerUpPickedUpEvent ();
			audioSource.clip = soundEffect;
			audioSource.Play ();
			powerUp.PickUp ();
			audioSource.transform.SetParent (transform.parent);
			Destroy (audioSource.gameObject, audioSource.clip.length);
			Destroy (gameObject);
		} else if (col.CompareTag (MyTags.BallShredder.ToString ())) {
			Destroy (gameObject);
		}
	}
}
