using UnityEngine;
using System.Collections;

abstract public class PowerUps : MonoBehaviour {

	[SerializeField] protected float duration = 15f;
	protected bool refresh;
	protected float baseProcChance;
	[SerializeField] protected float procChance = 5f;

	public abstract void PickUp ();
	protected abstract void Subscribe();
	protected abstract void Unsubscribe ();
	protected abstract void EndOfEffect ();
	protected abstract void ActiveEffect (Collider2D col, GameObject ball);

	protected virtual IEnumerator ControlEffectDuration(float duration){
		float startTime = Time.timeSinceLevelLoad;
		float currentTime = startTime;
		while (currentTime - startTime < duration) {
			currentTime = Time.timeSinceLevelLoad;
			yield return new WaitForSeconds(0.25f);
		}
		Unsubscribe ();
		EndOfEffect ();
	}


	void OnTriggerEnter2D(Collider2D col){
		if (col.CompareTag (MyTags.Player.ToString ())) {
			PickUp ();
		}
	}

}