using UnityEngine;
using System.Collections;

public class GainHP : PowerUps {

	private Player player;
	// Use this for initialization
	void Start () {
		player = FindObjectOfType<Player> ();
		Debug.Assert (player != null, "player not found");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void PickUp(){
		player.GainHP ();
	}

	protected override void Subscribe (){}

	protected override void Unsubscribe(){}

	protected override void EndOfEffect(){}

	protected override void ActiveEffect (Collider2D col, GameObject ball) {}
}
