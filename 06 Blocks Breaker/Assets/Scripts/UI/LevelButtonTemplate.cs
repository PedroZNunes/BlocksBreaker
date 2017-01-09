using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(Image))]
public class LevelButtonTemplate : MonoBehaviour{

	public Text LevelText;
	public bool isUnlocked;
	public int worldNumber;

	private LevelManager levelManager;
	private Button button;
		
	void Start(){
		button = GetComponent<Button> ();
		button.interactable = (isUnlocked);
		button.onClick.AddListener (()=>ButtonClicked());
		}

	void ButtonClicked(){
		levelManager = FindObjectOfType<LevelManager> ();
		levelManager.LoadLevel ("02 Level_" + worldNumber + LevelText.text);
	}
}
