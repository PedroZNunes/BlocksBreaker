using UnityEngine;
using System.Collections;

public class GainHP : PowerUps {

	private Player player;
	// Use this for initialization
	void Start () {
		

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void PickUp(){
		player = FindObjectOfType<Player> ();
		Debug.Assert (player != null, "player not found");
		player.GainHP ();
	}

	protected override void Subscribe (){}

	protected override void Unsubscribe(){}

}
