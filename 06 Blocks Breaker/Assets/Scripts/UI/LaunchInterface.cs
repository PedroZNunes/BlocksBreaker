﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LaunchInterface : MonoBehaviour {

	[SerializeField] private GameObject tapCircle;
	[SerializeField] private GameObject firstLaunchText;
	[SerializeField] private GameObject arrow;

	private GUIController guiController;
	private Player player;
	private Vector2 startPos, endPos, direction;

	void Awake () {
		Debug.Assert (tapCircle != null, "Tap Circle not set in inspector");

		guiController = FindObjectOfType<GUIController> ();
		Debug.Assert (guiController != null, "GUI Controller not found.");

		player = FindObjectOfType<Player> ();
		Debug.Assert (player != null, "Player not found.");
	}
	

	void Update () {
		tapCircle.transform.position = Camera.main.WorldToScreenPoint(player.transform.position + (Vector3.up * 0.25f));
	}

	public void OnTapCircle(){
		player.canMove = false;
		startPos = arrow.transform.position;
		GetComponent<Animator> ().SetBool ("isClicked", true);
	}

	public void OnDrag(){
		Vector2 currentPos;
		if (Input.touchCount >0)
			currentPos = Input.GetTouch(0).position;
		else
			currentPos = Input.mousePosition;
		Vector2 offset = (currentPos - startPos);
		float angle = Mathf.Atan2 (offset.y, offset.x) * Mathf.Rad2Deg;
		arrow.transform.rotation = Quaternion.LookRotation (Vector3.forward, offset);
	}

	public void OnReleaseCircle(){
		player.canMove = true;
		if (Input.touchCount > 0)
			endPos = Input.GetTouch (0).position;
		else
			endPos = Input.mousePosition;
		print ("StartPos: " + startPos + " - EndPos: " + endPos + " - Offset: " + (endPos - startPos).normalized);
		direction = (endPos - startPos).normalized;
		guiController.ProcLaunchEvent (direction);
	}

	public void ActivateFirstLaunch(){
		tapCircle.SetActive (true);
		firstLaunchText.SetActive (true);
	}

	public void ActivateLaunch(){
		tapCircle.SetActive (true);
		firstLaunchText.SetActive (false);
	}



}
