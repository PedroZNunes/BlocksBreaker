﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[System.Serializable]
public class Level{
	public string levelText;
	public bool isUnlocked;
}


public class LevelSelectManager : MonoBehaviour {

	public Sprite buttonSpriteOn;
	public Sprite buttonSpriteOff;
	public int worldNumber = 1;

	[SerializeField] private GameObject levelButtonPrefab;
	[SerializeField] private List<Level> LevelList;

	private Transform parent;
	private bool isLoaded = false;


	void OnValidate(){
		//Fill Text Names
		for (int i = 0; i < LevelList.Count; i++) {
			string levelName = string.Format("{0:00}",(i + 1));
			LevelList [i].levelText = levelName;
		}
	}


	void Start(){
		parent = transform;
		TestUnlocked ();
	}

	void TestUnlocked(){
		print (LevelList.Count);
		for (int i = 0; i < LevelList.Count; i++) {
			string levelID = worldNumber.ToString () + LevelList [i].levelText;
			LevelList [i].isUnlocked = PlayerPrefsManager.IsLevelUnlocked (levelID);
		}
	}

	public void CreateButtons(){
		if (!isLoaded) {
			for (int i = 0; i < LevelList.Count; i++) {
				Level level = LevelList [i];
				GameObject newButton = Instantiate (levelButtonPrefab, parent) as GameObject;
				newButton.transform.localScale = Vector3.one;


				newButton.GetComponent<Button> ().image.sprite = buttonSpriteOn;
				SpriteState spriteState = newButton.GetComponent<Button> ().spriteState;
				spriteState.disabledSprite = buttonSpriteOff;
				newButton.GetComponent<Button> ().spriteState = spriteState;

				LevelButtonTemplate button = newButton.GetComponent<LevelButtonTemplate> ();
				button.LevelText.text = level.levelText;
				button.isUnlocked = level.isUnlocked;
				button.worldNumber = worldNumber;
			}
			isLoaded = true;
		}
	}

}



