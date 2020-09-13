using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldSlider : MonoBehaviour {
	
	[SerializeField] private GameObject worldsGridLayout;
	[SerializeField] private Canvas canvas;

	private Vector2 previousPosition, startDragPos, endDragPos;
	private RectTransform rectTransform;
	private HorizontalLayoutGroup layoutGroup;
	private WorldManager worldManager;

	private float moveAmount, minimumOffset;
	private bool isLerping = false;

	void Awake(){
		rectTransform = this.transform as RectTransform;
		minimumOffset = (rectTransform.rect.width/5f) * canvas.scaleFactor;
		Debug.Log ("Current width for this resolution: " + rectTransform.rect.width);
		Debug.Log ("Minimum Offset to proc slide: " + minimumOffset);
		layoutGroup = GetComponentInChildren<HorizontalLayoutGroup> ();
		worldManager = FindObjectOfType<WorldManager> ();
	}

	void Start(){
		
		moveAmount = (worldsGridLayout.GetComponent<RectTransform>().rect.width + layoutGroup.spacing) * canvas.scaleFactor;
		Debug.Log ("Move Amount:" + moveAmount);
		previousPosition = worldsGridLayout.transform.position;
		Debug.Log (string.Format ("Starting Position set to ({0}, {1})", previousPosition.x, previousPosition.y));
	}

	public void OnBeganDrag(){
		if (Input.touchCount >0)
			startDragPos = Input.GetTouch(0).position;
		else
			startDragPos = Input.mousePosition;
		
		Debug.Log (string.Format ("Drag Began. StartPos({0}, {1})", startDragPos.x, startDragPos.y));
	}

	public void OnDrag(){
		Vector2 currentPos;
		if (Input.touchCount >0)
			currentPos = Input.GetTouch(0).position;
		else
			currentPos = Input.mousePosition;
		Vector2 offset = (currentPos - startDragPos);
		worldsGridLayout.transform.position = new Vector2 ( previousPosition.x + (offset.x/2f),worldsGridLayout.transform.position.y);
	}

	public void OnEndDrag(){
		if (Input.touchCount >0)
			endDragPos = Input.GetTouch(0).position;
		else
			endDragPos = Input.mousePosition;
		
		Vector2 offSet = endDragPos - startDragPos;
		if (Mathf.Abs (offSet.x) > minimumOffset) {
			Debug.Log ("Dragging to next Screen");
			if (offSet.x > 0) {		
				PreviousWorldGrid ();
			} else {
				NextWorldGrid ();
			}
		} else {
			SlideBackToPosition ();
			Debug.Log (string.Format("Sliding back to position. CurrentTransPosition: ({0}, {1}) // DesiredTransPosition: ({2}, {3})", transform.position.x, transform.position.y, previousPosition.x, previousPosition.y));
		}
		Debug.Log (string.Format ("Drag Ended. EndPos({0}, {1})", endDragPos.x, endDragPos.y));
	}

	IEnumerator LerpGridToPosition(Vector2 desiredPosition){
		isLerping = true;
		Vector2 currentPosition = worldsGridLayout.transform.position;
		for (float ratio = 0f; ratio <= 1f; ratio+=0.1f){
			if (ratio > 0.9f)
				ratio = 1f;
			worldsGridLayout.transform.position = Vector2.LerpUnclamped (currentPosition, desiredPosition, ratio); //consider the movement from the drag. which is offset. and it is local. it should be offset 
			yield return null;
		}

		isLerping = false;
		previousPosition = worldsGridLayout.transform.position;
	}

	void SlideBackToPosition (){
		StartCoroutine (LerpGridToPosition(previousPosition));
	}

	public void NextWorldGrid ()
	{
		bool isValid;
		Vector2 desiredPosition = new Vector2 (previousPosition.x - moveAmount, previousPosition.y);
		if (!isLerping) {
			isValid = worldManager.NextWorld ();
			if (isValid)
				StartCoroutine (LerpGridToPosition (desiredPosition));
			else
				SlideBackToPosition ();
		}
	}

	public void PreviousWorldGrid ()
	{
		bool isValid = false;
		Vector2 desiredPosition = new Vector2 (previousPosition.x + moveAmount, previousPosition.y);
		if (!isLerping) {
			isValid = worldManager.PreviousWorld ();
			if (isValid)
				StartCoroutine (LerpGridToPosition (desiredPosition));
			else
				SlideBackToPosition ();
		}
	}

}
