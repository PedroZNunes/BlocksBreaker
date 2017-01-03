using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerUpManager : MonoBehaviour {

	[SerializeField] private PowerUps[] powerUps;
	[Tooltip("Use the same order as the power ups array")]

	private ActionMaster actionMaster;
	private float lastProcTime = 0f;
	private float currentProcTime = 0f;
	private float currentDropChance = 0f;
	private float bonusPercentPerSecond = 2f;

	public enum PowerUpsEnum {MultiBall, ElectricBall, GainHP, ExplosiveBall};
	private PowerUpsEnum currentPowerUp;

	void OnEnable(){Block.PowerUpDropEvent += DropPowerUp;}
	void OnDisable(){Block.PowerUpDropEvent -= DropPowerUp;}

	void Start(){
		
	}

	public void IncrementTimer (){
		currentProcTime += Time.deltaTime;
	}

	void DropPowerUp (Vector3 blockPosition, float dropChancePercent){
		float bonusProc = currentProcTime * bonusPercentPerSecond;
		float j = Random.Range (0f, 100f);
		if (!PlayerBuffs.isBuffed)
			j = j / 2f;
		currentDropChance += dropChancePercent;
		if (j <= currentDropChance + bonusProc) {
			print ("PROC - " + (currentDropChance + bonusProc) + "%");
			int i = Random.Range (0, powerUps.Length);
			Transform parent = GameObject.FindGameObjectWithTag (MyTags.Dynamic.ToString ()).transform;
			Debug.Assert (parent != null, "Dynamic not found.");
			powerUps [i].Drop (blockPosition);
			currentProcTime = 0f;
			currentDropChance = 0f;
		}
	}
}
