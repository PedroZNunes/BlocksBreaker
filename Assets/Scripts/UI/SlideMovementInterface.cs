using UnityEngine;
using System.Collections;

public class SlideMovementInterface : MonoBehaviour {


	private Player player;

	void OnEnable (){
		player = FindObjectOfType<Player> ();
		Debug.Assert (player != null, "Player not found in the scene");

	}


	void Update() {
		if (Input.touchCount > 0 && Input.GetTouch (0).phase != TouchPhase.Canceled) {
			Vector3 touchPosition = Camera.main.ScreenToWorldPoint (Input.GetTouch (0).position);
			float movementIntensity = Mathf.Clamp (((touchPosition.x)), -1f, 1f);
			player.AssignDirection (movementIntensity);
		} else if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Ended) {
			player.AssignDirection (0f);
		}

		if (Input.GetMouseButton (0)) {
			Vector3 mousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			float movementIntensity = Mathf.Clamp (((mousePosition.x)), -1f, 1f);
			player.AssignDirection (movementIntensity);
		} else if (Input.GetMouseButtonUp (0)) {
			player.AssignDirection (0f);
		}
	}

}
