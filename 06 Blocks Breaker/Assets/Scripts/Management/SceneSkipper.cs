using UnityEngine;
using System.Collections;

public class SceneSkipper : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Invoke ("SkipScene", 60f);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.anyKey) {
			SkipScene ();
		}
	}

	public void SkipScene(){
		LevelManager levelManager = FindObjectOfType<LevelManager> ();
		levelManager.LoadLevel ("01a Start");
	}
}
