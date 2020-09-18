using UnityEngine;
using System.Collections;


[RequireComponent( typeof( Animator ), typeof( AudioSource ) )]
public class Player : MonoBehaviour {

	public bool canMove;

	[SerializeField] private Health health;

	[SerializeField] private AudioClip takeHitClip;
	[Tooltip("Set elements according to HP")]
	[SerializeField] private Sprite[] hpSprites;
	[SerializeField] private SpriteRenderer healthSpriteRenderer;
	[SerializeField] public Transform ballRespawn;
	private const int HPARRAYSIZE = 3;
	private AudioSource audioSource;
	private Animator animator;
	private Vector2 direction;

	public bool isDead { get { return health.isDead; } }

	[SerializeField] private float speed = 12f;


	void OnValidate() {
		if (hpSprites.Length != health.GetMax())
		{
			hpSprites = new Sprite[health.GetMax()];
		}
	}

	void Awake() {
		animator = GetComponent<Animator>();
		audioSource = GetComponent<AudioSource>();
		health.Initialize();
	}

	void Start() {
		UpdateSprite();
	}

	public void AssignDirection( float movementIntensity ) {
		direction = new Vector2( movementIntensity, 0f );
	}

	void Update() {
		Move();
	}


	public void Move() {
		if (canMove) {
			if (direction != Vector2.zero) {
				Vector2 velocity = direction * speed;
				transform.Translate( velocity * Time.deltaTime );
				CheckBorders();
			}
		}
	}

	void CheckBorders() {
		float screenWidth = Camera.main.orthographicSize * Camera.main.aspect;
		Bounds bounds = GetComponent<PolygonCollider2D>().bounds;
		Debug.Assert( bounds != null, "Collider bounds not found in the player" );

		float borderHorizontal = (screenWidth - (bounds.extents.x * transform.localScale.x));
		if (transform.position.x > borderHorizontal) {
			transform.position = new Vector2( borderHorizontal, transform.position.y );
		} else if (transform.position.x < -borderHorizontal) {
			transform.position = new Vector2( -borderHorizontal, transform.position.y );
		}
	}

	public void TakeHit() {
		animator.SetTrigger( "TakeHit" );
		audioSource.PlayOneShot( takeHitClip, 0.3f );
		health.TakeHit();
		//hp--;
		UpdateSprite();
	}

	public void GainHP() {
		health.HealUp();
		UpdateSprite();
	}

	

	private void UpdateSprite() {
		if (health.isDead) {
			healthSpriteRenderer.sprite = null;
			animator.SetTrigger( "Die" );
			return;
        }

		//healthSpriteRenderer.sprite = hpSprites[hp - 1];
		healthSpriteRenderer.sprite = hpSprites[health.current - 1];
	}

}
	
public class PlayerBuffs : Player {

	static public bool isMulti = false;
	static public bool isElectric = false;
	static public bool isExplosive = false;
	static public bool isBuffed = false;

	void Update(){
		isBuffed = (isMulti == true || isExplosive == true || isElectric == true);
	}


}