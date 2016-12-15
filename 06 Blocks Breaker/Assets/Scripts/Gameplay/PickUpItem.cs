using UnityEngine;
using System.Collections;
[RequireComponent (typeof(Collider2D), typeof(SpriteRenderer))]
public class PickUpItem : MonoBehaviour {


	private enum PowerUpList {MultiBall, ElectricBall, GainHP, ExplosiveBall}

	[SerializeField] PowerUpList powerUpType;
	private float speed = 4f;
	[SerializeField] private AudioClip soundEffect;
	private AudioSource audioSource;
	private PowerUps powerUp;
	private float defaultPitch;
	private float pitchVariance;

	void Start(){
		if (powerUpType == PowerUpList.MultiBall)
			powerUp = FindObjectOfType<MultiBall> ();
		else if (powerUpType == PowerUpList.ElectricBall)
			powerUp = FindObjectOfType<ElectricBall> ();
		else if (powerUpType == PowerUpList.GainHP)
			powerUp = FindObjectOfType<GainHP> ();
		else if (powerUpType == PowerUpList.ExplosiveBall)
			powerUp = FindObjectOfType<ExplosiveBall> ();
		Debug.Assert (powerUpType != null, "Power Up not found");
		Debug.Assert (soundEffect != null, "soundEffect not found");

		audioSource = GetComponentInChildren<AudioSource> ();

	}

	void Update(){
		transform.Translate (Vector2.down * speed * Time.deltaTime);
	}
		

	void OnTriggerEnter2D (Collider2D col){
		if (col.CompareTag (MyTags.Player.ToString ())) {
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
