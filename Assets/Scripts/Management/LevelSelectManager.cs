using System.Collections;
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

	private Color colorOn;
	private Color colorOff;

	public int worldNumber = 1;

	[SerializeField] private Sprite buttonSprite;
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

	public void AssignColors(Color colorOn, Color colorOff) {
		this.colorOn = colorOn;
		this.colorOff = colorOff;
    }

	public void CreateButtons(){
		if (!isLoaded) {
			for (int i = 0; i < LevelList.Count; i++) {
				Level level = LevelList [i];
				GameObject newButton = Instantiate (levelButtonPrefab, parent) as GameObject;
				newButton.transform.localScale = Vector3.one;

				Button button = newButton.GetComponent<Button>();
				button.image.color = colorOn;


				//TROCA O METODO DE ANIMAÇÃO DE SPRITE PRA COLOR
				//TROCA O SPRITE POR COLOR
				button.transition = Selectable.Transition.ColorTint;
				ColorBlock colors = button.colors;
				colors.disabledColor = colorOff;
				button.colors = colors;


				LevelButtonTemplate buttonTemplate = newButton.GetComponent<LevelButtonTemplate> ();
				buttonTemplate.LevelText.text = level.levelText;
				buttonTemplate.isUnlocked = level.isUnlocked;
				buttonTemplate.worldNumber = worldNumber;
			}
			isLoaded = true;
		}
	}

}



