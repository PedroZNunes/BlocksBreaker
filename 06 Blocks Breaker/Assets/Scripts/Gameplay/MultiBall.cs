using UnityEngine;
using System.Collections;

public class MultiBall : PowerUps {

	[SerializeField] private GameObject ballPrefab;
	[SerializeField] private GameObject multiBallInstancePrefab;


	private GameObject multiballEffect;
	private Transform ballParent;
	private AudioSource audioSource;
	private float baseProcChance;


	void Awake(){
		audioSource = GetComponent<AudioSource> ();
	}

	void Start (){
		ballParent = GameObject.FindGameObjectWithTag (MyTags.Dynamic.ToString ()).transform;
	}

	void OnDisable(){ Unsubscribe (); }

	public override void PickUp (){
		PlayerBuffs.isMulti = true;
		Ball[] balls = FindObjectsOfType<Ball> ();
		for (int i = 0; i < balls.Length; i++) {
			MultiBallInstance instance = balls [i].GetComponentInChildren<MultiBallInstance> ();
			if (instance == null) {
				Instantiate (multiBallInstancePrefab, balls [i].transform.position, Quaternion.identity, balls [i].transform);
			}
		}
	}

	protected override void Subscribe (){}
	protected override void Unsubscribe(){}

}