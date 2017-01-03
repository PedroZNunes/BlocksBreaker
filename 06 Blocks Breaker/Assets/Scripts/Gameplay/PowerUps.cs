using UnityEngine;
using System.Collections;

abstract public class PowerUps : MonoBehaviour {


	public abstract void PickUp ();
	protected abstract void Subscribe();
	protected abstract void Unsubscribe ();

	public GameObject pickUpPrefab;
	private Transform itemParent;

	void Awake(){
		itemParent = GameObject.FindWithTag (MyTags.Dynamic.ToString ()).transform;
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.CompareTag (MyTags.Player.ToString ())) {
			PickUp ();
		}
	}

	public void Drop(Vector3 blockPosition){
		Instantiate (pickUpPrefab, blockPosition, Quaternion.identity, itemParent);
	}

}