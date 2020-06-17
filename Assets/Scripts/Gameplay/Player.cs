using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Animator), typeof(AudioSource))]
public class Player : MonoBehaviour {

	const int MAXHP = 3;

	public bool canMove;
	public int hp;

	[SerializeField] private AudioClip takeHitClip;
	[Tooltip("Set elements according to HP")]
	[SerializeField] private Sprite[] hpSprites = new Sprite[HPARRAYSIZE];
	[SerializeField] private float speed = 12f;
	[SerializeField] private SpriteRenderer spriteRenderer;
	private const int HPARRAYSIZE = 4;
	private AudioSource audioSource;
	private Animator animator;
	private Vector2 direction;


	void OnValidate(){
		if (hpSprites.Length != HPARRAYSIZE) {
			Debug.LogWarning ("Don't change the size of the HP array.");
		}
	}

	void Awake(){
		animator = GetComponent<Animator> ();
		audioSource = GetComponent<AudioSource> ();
	}

	void Start(){
		spriteRenderer.sprite = hpSprites [hp];

	}

	public void AssignDirection(float movementIntensity){
		direction = new Vector2 (movementIntensity, 0f);
	}

	void Update(){
		Move ();	
	}


	public void Move(){
		if (canMove) {
			if (direction != Vector2.zero) {
				Vector2 velocity = direction * speed;
				transform.Translate (velocity * Time.deltaTime);
				CheckBorders ();
			}
		}
	}

	void CheckBorders(){
		float screenWidth = Camera.main.orthographicSize * Camera.main.aspect;
		float borderHorizontal = (screenWidth - (spriteRenderer.sprite.bounds.extents.x * transform.localScale.x));
		if (transform.position.x > borderHorizontal){
			transform.position = new Vector2 (borderHorizontal, transform.position.y);
		}else if (transform.position.x < - borderHorizontal){
			transform.position = new Vector2 (-borderHorizontal, transform.position.y);			
		}
	}
		
	public void TakeHit (){
		animator.SetTrigger ("TakeHit");
		audioSource.PlayOneShot(takeHitClip, 0.3f);
		hp--;
		spriteRenderer.sprite = hpSprites [hp];
	}

	public void GainHP(){
		if (hp < MAXHP) {
			hp++;
			spriteRenderer.sprite = hpSprites [hp];
		}
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