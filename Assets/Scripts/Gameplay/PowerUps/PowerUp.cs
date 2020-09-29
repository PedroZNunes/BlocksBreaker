using UnityEngine;
using System.Collections;


public enum PowerUpsEnum { MultiBall, ElectricBall, GainHP, ExplosiveBall };

abstract public class PowerUp : MonoBehaviour
{
	[SerializeField]
	PowerUpsEnum powerUpName;

	[SerializeField]
	float dropChancePercent;

	[SerializeField]
	GameObject dropPrefab;

	static private Transform dropInstanceParent;


	public abstract void OnPickUp();

	protected abstract void Subscribe();
	
	protected abstract void Unsubscribe();



	public PowerUpsEnum GetName() { return powerUpName; }

	public float GetDropChance() { return dropChancePercent; }

	public void Drop(Vector3 blockPosition)
	{
		dropInstanceParent = GameObject.FindWithTag(MyTags.Dynamic.ToString()).transform;

		Instantiate(dropPrefab, blockPosition, Quaternion.identity, dropInstanceParent);
	}
}
