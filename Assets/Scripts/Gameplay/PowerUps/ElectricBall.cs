using UnityEngine;
using System.Collections;


public class ElectricBall : PowerUps {

	[SerializeField] private ElectricBallInstance electricBallInstance;

	void Awake(){

	}

	public override void PickUp (){
		Transform parent = FindObjectOfType<PowerUpManager>().transform;
		Instantiate (electricBallInstance, parent);
	}


	protected override void Subscribe (){}
	protected override void Unsubscribe(){}
}
