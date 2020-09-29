﻿using UnityEngine;
using System;

public class MovementInput {


	public int UpdateDirection() {
		int direction = 0;

#if UNITY_IOS || UNITY_ANDROID
		if (Input.touchCount > 0 && Input.GetTouch (0).phase != TouchPhase.Canceled){
			Vector3 touchPosition = Camera.main.ScreenToWorldPoint (Input.GetTouch (0).position);
			direction = AssignDirection(touchPosition.x);
		} 
		else if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Ended) {
			direction = AssignDirection(0f);
		}
#endif

		if (Input.GetMouseButton (0)) {
			Vector3 mousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			direction = AssignDirection(mousePosition.x);
		} else if (Input.GetMouseButtonUp (0)) {
			direction = AssignDirection(0f);
		}

		float horizontalDirection = 0f;
		if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
			horizontalDirection = -1f;
		
        } else if (!Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
        {
			horizontalDirection = 1f;
		}

		if (horizontalDirection != 0f)
        {
			direction = AssignDirection(horizontalDirection);
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
