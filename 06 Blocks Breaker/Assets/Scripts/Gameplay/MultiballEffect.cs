using UnityEngine;
using System.Collections;

public class MultiballEffect : MonoBehaviour {

	[SerializeField] private GameObject trailBall;
	[SerializeField] private Sprite defaultSprite;
	[SerializeField] private float trailInterval = 0.3f;

	private Sprite ballSprite;
	private GameObject trailParent;
	private float timer = 0;

	void OnEnable(){
		ballSprite = defaultSprite;
		trailParent = GameObject.FindGameObjectWithTag (MyTags.Dynamic.ToString ());
	}


	void FixedUpdate () {		
		timer += Time.deltaTime;
		if (timer >= trailInterval) {
			
			timer = 0f;
			GameObject[] balls = GameObject.FindGameObjectsWithTag (MyTags.Ball.ToString ());
			for (int i = 0; i < balls.Length; i++) {
				SpriteRenderer[] sprites = balls [i].GetComponentsInChildren<SpriteRenderer> ();
				if (sprites.Length == 1) {
					ballSprite = defaultSprite;
				} else {
					for (int j = 0; j < sprites.Length; j++) {
						if (sprites [j].transform.parent.CompareTag (MyTags.Ball.ToString ())) {
							ballSprite = sprites [j].sprite;
						}
					}
				}
				
				Vector3 newTransform = new Vector3 (balls [i].transform.position.x, balls [i].transform.position.y, balls [i].transform.position.z + 1);
				GameObject trail = Instantiate (trailBall, newTransform, Quaternion.identity, trailParent.transform) as GameObject;
				trail.transform.localScale = balls [i].transform.localScale;
				trail.GetComponent<SpriteRenderer> ().sprite = ballSprite;
			}
		} 
	}


}
