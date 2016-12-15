using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class MultiBall : PowerUps {

	[SerializeField] private GameObject ballPrefab;
	[SerializeField] private GameObject multiballEffectPrefab;
	[SerializeField] private int maxBallCount = 3;

	private GameObject multiballEffect;
	private Transform ballParent;
	private AudioSource audioSource;
	private Coroutine effectDuration;
	private WaitForSeconds cooldownTimer = new WaitForSeconds (0.1f);
	private bool onCooldown;

	void Awake(){
		baseProcChance = procChance;
		audioSource = GetComponent<AudioSource> ();
	}

	void Start (){
		ballParent = GameObject.FindGameObjectWithTag (MyTags.Dynamic.ToString ()).transform;
	}

	void OnDisable(){ Unsubscribe (); }

	public override void PickUp (){
		if (PlayerBuffs.isMulti == false) {
			PlayerBuffs.isMulti = true;
			multiballEffect = Instantiate (multiballEffectPrefab, this.transform) as GameObject;
			Subscribe ();
			effectDuration = StartCoroutine (ControlEffectDuration (duration));
		} else {
			StopCoroutine (effectDuration);
			effectDuration = StartCoroutine (ControlEffectDuration (duration));
			//resetui
		}
	}

	protected override void EndOfEffect(){
		Destroy (multiballEffect);
	}


	protected override void ActiveEffect (Collider2D col, GameObject ball) {
		if (!onCooldown) {
			
			Ball[] balls = FindObjectsOfType<Ball> ();
			if (balls.Length > 0)
				procChance = procChance / balls.Length;
			
			if (Random.Range (0f, 100f) <= procChance) {
				if (Ball.Count < maxBallCount) {
					audioSource.Play ();
					GameObject newBall = Instantiate (ballPrefab, ball.transform.position, ball.transform.rotation, ballParent) as GameObject;
					Rigidbody2D ballRB = ball.GetComponent<Rigidbody2D> ();
					newBall.GetComponent<Rigidbody2D> ().velocity = new Vector2 (-ballRB.velocity.x, -ballRB.velocity.y);
					StartCoroutine (Cooldown ());
				}
				procChance = baseProcChance;
			} else {
				procChance += procChance;
			}
		}
	}

	IEnumerator Cooldown(){
		onCooldown = true;
		yield return cooldownTimer;
		onCooldown = false;
	}


	protected override void Subscribe (){ Ball.BallCollidedEvent += ActiveEffect; }

	protected override void Unsubscribe(){
		Ball.BallCollidedEvent -= ActiveEffect;
		PlayerBuffs.isMulti = false;
	}

}