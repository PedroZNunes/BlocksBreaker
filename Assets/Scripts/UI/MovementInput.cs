using UnityEngine;
using System;

public class MovementInput {


	public int UpdateDirection() {
		int direction = 0;
		if (Input.touchCount > 0 && Input.GetTouch (0).phase != TouchPhase.Canceled) {
			Vector3 touchPosition = Camera.main.ScreenToWorldPoint (Input.GetTouch (0).position);
			//float movementIntensity = Mathf.Clamp (((touchPosition.x)), -1f, 1f);
			direction = AssignDirection(touchPosition.x);
		} else if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Ended) {
			direction = AssignDirection(0f);
		}

		if (Input.GetMouseButton (0)) {
			Vector3 mousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			//float movementIntensity = Mathf.Clamp (((mousePosition.x)), -1f, 1f);
				//player.AssignDirection (movementIntensity);
			direction = AssignDirection(mousePosition.x);
		} else if (Input.GetMouseButtonUp (0)) {
			//player.AssignDirection (0f);
			direction = AssignDirection(0f);
		}

		return direction;
	}

	private int AssignDirection(float touchPositionX)
    {
		int direction = Mathf.Clamp(
						Mathf.RoundToInt(
							Mathf.Sign(touchPositionX)), -1, 1);
		return direction;
	}

	

}
