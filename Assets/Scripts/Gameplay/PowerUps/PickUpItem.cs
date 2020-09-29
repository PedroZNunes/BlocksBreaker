using UnityEngine;
using System.Collections;
using System;

[RequireComponent (typeof(Collider2D), typeof(SpriteRenderer))]
public class PickUpItem : MonoBehaviour {
	public delegate void PickedUpHandler (PowerUpsEnum name);
	static public event PickedUpHandler PowerUpPickedUpEvent;
	//[SerializeField] private PowerUp powerUp;
	[SerializeField] private PowerUpsEnum powerUpName;

	private float speed = 2.5f;
	[SerializeField] private AudioClip soundEffect;
	[SerializeField] private AudioSource audioSource;
	//private float defaultPitch;
	//private float pitchVariance;

	void Start(){
		//Debug.Assert (powerUp != null, "PowerUp not set");
		Debug.Assert (soundEffect != null, "soundEffect not found");
	}

	void Update(){
		transform.Translate (Vector2.down * speed * Time.deltaTime);
	}
		

	void OnTriggerEnter2D (Collider2D col){
		if (col.CompareTag (MyTags.Player.ToString ())) {
			if (PowerUpPickedUpEvent != null)
				PowerUpPickedUpEvent (powerUpName);
			audioSource.clip = soundEffect;
			audioSource.Play ();
			audioSource.transform.SetParent (transform.parent);
			Destroy (audioSource.gameObject, audioSource.clip.length);
			Destroy (gameObject);
		} else if (col.CompareTag (MyTags.BallShredder.ToString ())) {
			Destroy (gameObject);
		}
	}
}
