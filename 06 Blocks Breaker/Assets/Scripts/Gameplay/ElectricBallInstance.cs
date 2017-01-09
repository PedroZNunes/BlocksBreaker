using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ElectricBallInstance : MonoBehaviour {

	[SerializeField] private GameObject ShockExplosionPrefab;
	[SerializeField] private WaitForSeconds timeBetweenShocks = new WaitForSeconds (0.3f);
	[SerializeField] private LayerMask layerMask;
	[SerializeField] private float effectArea = 1.5f;
	[SerializeField] private int chainHits = 3;
	[SerializeField] private float procChance = 5f;
	[SerializeField] private float duration = 8f;

	private SpriteRenderer background;
	private AudioSource audioSource;
	private Coroutine effectDuration;
	private bool readyToDestroy = false;
	private float shockAnimationDuration;
	private Color activeColor = new Color (0.24f, 0.43f, 0.53f, 0.67f);
	private Color baseColor;
	private float baseProcChance;

	static private ElectricBallInstance instance;
	//singleton Process
	void Awake () {
		if (instance != null) {
			instance.ResetPowerUp ();
			Destroy (gameObject);
		} else {
			instance = this;
		}

		baseProcChance = procChance;
		audioSource = GetComponent<AudioSource> ();
		background = GameObject.FindGameObjectWithTag (MyTags.Background.ToString ()).GetComponent<SpriteRenderer> ();
		Debug.Assert (background != null, "Background not found. SpriteRenderer");
		baseColor = background.color;
	}


	void Start (){
		if (PlayerBuffs.isElectric == false) {
			Subscribe ();
			StartCoroutine (ChangeBackgroundColor (activeColor));
			effectDuration = StartCoroutine (ControlEffectDuration (duration));
			PlayerBuffs.isElectric = true;
		} else {
			ResetPowerUp ();
		}
	}

	IEnumerator ControlEffectDuration(float duration){
		float startTime = Time.timeSinceLevelLoad;
		float currentTime = startTime;
		while (currentTime - startTime < duration) {
			currentTime = Time.timeSinceLevelLoad;
			yield return new WaitForSeconds(0.25f);
		}
		Unsubscribe ();
		yield return StartCoroutine(EndOfEffect ());
		if (readyToDestroy)
			Destroy (gameObject);
	}

	void ResetPowerUp(){
		StopCoroutine (effectDuration);
		effectDuration = StartCoroutine (ControlEffectDuration (duration));
	}


	IEnumerator ChangeBackgroundColor(Color desiredColor){
		Color previousColor = background.color;
		for (float ratio = 0f; ratio < 1f; ratio += 0.1f) {
			background.color = Color.Lerp (previousColor, desiredColor, ratio);
			yield return null;
		}

	}

	IEnumerator ShockEffect (Collider2D col){
		int hits = 1;
		Vector2 blockPos = new Vector2 (col.transform.position.x, col.transform.position.y);
		GameObject shock = Instantiate (ShockExplosionPrefab, blockPos, Quaternion.identity, this.transform) as GameObject;
		shockAnimationDuration = shock.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).length;
		Destroy (shock, shockAnimationDuration);
		yield return timeBetweenShocks;
		while (hits <= chainHits) {
			Collider2D[] affectedBlocks = Physics2D.OverlapCircleAll (blockPos, effectArea, layerMask);
			if (affectedBlocks.Length == 0)
				break;
			int targetBlockIndex = Random.Range (0, affectedBlocks.Length);
			Block targetBlock = affectedBlocks [targetBlockIndex].GetComponent<Block> ();
			hits++;
			blockPos = new Vector2 (targetBlock.transform.position.x, targetBlock.transform.position.y);
			shock = Instantiate (ShockExplosionPrefab, blockPos, Quaternion.identity, this.transform) as GameObject;
			shockAnimationDuration = shock.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).length;
			Destroy (shock, shockAnimationDuration);
			targetBlock.TakeHit ();
			yield return timeBetweenShocks;
		}
	}

	void ActiveEffect (Collider2D col, GameObject ball) {
		if (Random.Range (0f, 100f) <= procChance) {
			audioSource.Play ();
			StartCoroutine (ShockEffect (col));
			procChance = baseProcChance;
		} else {
			procChance += procChance;
		}
	}

	IEnumerator EndOfEffect(){
		yield return StartCoroutine (ChangeBackgroundColor (baseColor));
		readyToDestroy = true;
	}

	void OnDisable(){ Unsubscribe ();}

	void Subscribe (){ Ball.BallCollidedEvent += ActiveEffect; }

	void Unsubscribe(){
		Ball.BallCollidedEvent -= ActiveEffect;
		PlayerBuffs.isElectric = false;
	}
}
