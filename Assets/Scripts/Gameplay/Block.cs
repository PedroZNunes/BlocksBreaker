using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Block : MonoBehaviour {

	public delegate void PowerUpsDropHandler (Vector3 blockPosition, float dropChancePercent);
	public static event PowerUpsDropHandler PowerUpDropEvent;
	public static event Action BlockDestroyedEvent;
	public static event Action NoBlocksLeftEvent;

	public static int Count;

	public int hp;
	[SerializeField] private Sprite[] sprites;

	[SerializeField] private int scorePerHit;

	static private float powerUpDropChancePercent = 0.3f;
	private SpriteRenderer spriteRenderer;
	private Vector3 desiredPosition;
	/// <summary>
	/// Offset that affects all blocks so that they show above mid-screen
	/// </summary>
	private Vector3 blockDisplacement = new Vector3 (0f, 10f, 0f);

	static private List<Block> blocks;

	void OnValidate(){
		if (sprites.Length != hp) {
			sprites = new Sprite[hp];
		}
	}

    private void OnEnable() {
		ActionMaster.EndGameEvent += ResetBlocks;
    }

    private void OnDisable() {
		ActionMaster.EndGameEvent -= ResetBlocks;
	}

    void Awake(){
		if (blocks == null) {
			blocks = new List<Block>();
        }

		desiredPosition = transform.position;
		transform.position += blockDisplacement;

        foreach (Block anotherBlock in blocks) {
			if ( this.transform.position == anotherBlock.transform.position) {
				Debug.LogWarning( "Stacking blocks detected at position: " + this.transform.position.ToString() );
				Destroy( gameObject );
				return;
            }
        }
		blocks.Add( this );
	}

	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
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
		hp--;
//		ScoreManager.AddPoints (scorePerHit);
		if (hp <= 0) {
			if (PowerUpDropEvent != null)
				PowerUpDropEvent( transform.position, powerUpDropChancePercent );
			DestroyBlock ();
		} else {
			spriteRenderer.sprite = sprites [hp-1];
		}
	}

	void DestroyBlock(){
		blocks.Remove( this );
		if (BlockDestroyedEvent != null)
			BlockDestroyedEvent ();
		if (blocks.Count <= 0) {
			if (NoBlocksLeftEvent != null) {
				NoBlocksLeftEvent();
			}
        }
		Destroy (gameObject);
	}

	private void ResetBlocks() {
		if (blocks != null) {
            if (blocks.Count > 0) {
				blocks.Clear();
            }
        }
    }

}
