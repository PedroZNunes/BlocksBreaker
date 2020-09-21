using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SpriteRenderer))]
public class MultiBallInstance : MonoBehaviour {

	[SerializeField] private GameObject multiBallEffect;
	[SerializeField] private GameObject ballPrefab;
	private GameObject activeEffect;
	private Transform dynamic;
	static private int Count;


	void Awake(){
		Debug.Assert (multiBallEffect != null, "Explosive Effect not set in the inspector");
		Debug.Assert (ballPrefab != null, "Ball Prefab not set in inspector.");
		dynamic = GameObject.FindWithTag (MyTags.Dynamic.ToString ()).transform;
		Subscribe ();
		Count++;
	}

	void Start(){
		activeEffect = Instantiate (multiBallEffect, this.transform.position, Quaternion.identity, this.transform) as GameObject;
	}

	void ActiveEffect (Collider2D col, GameObject ball) {
		if (transform.parent == ball.transform) {
			GameObject newBall = Instantiate (ballPrefab, ball.transform.position, ball.transform.rotation, dynamic) as GameObject;
			Rigidbody2D ballRB = ball.GetComponent<Rigidbody2D> ();
			newBall.GetComponent<Rigidbody2D> ().velocity = new Vector2 (-ballRB.velocity.x, ballRB.velocity.y);
			Destroy (activeEffect);
			Count--;
			TestEndOfEffect ();
			Destroy (gameObject);
		}
	}

	private void TestEndOfEffect (){
		if (Count <= 0){
			Unsubscribe ();
			PlayerBuffs.isMulti = false;
		}
	}

	void OnDisable(){Unsubscribe ();}

	private void Subscribe (){ Ball.BallCollidedEvent += ActiveEffect; }
	private void Unsubscribe(){ Ball.BallCollidedEvent -= ActiveEffect; }
}
