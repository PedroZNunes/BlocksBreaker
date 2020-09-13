using UnityEngine;
using UnityEngine.UI;
using System;

public class OptionsController : MonoBehaviour {

	[SerializeField] private Slider musicSlider, effectsSlider;

	private LevelManager levelManager;
	private AudioController audioController;

	public static event Action UnpauseEvent;



	void Start () {

		levelManager = FindObjectOfType<LevelManager> ();
		Debug.Assert (levelManager != null, "Level Manager not Found");

		audioController = levelManager.GetComponent<AudioController>();

		musicSlider.value	= audioController.GetMusicVolume();
		effectsSlider.value = audioController.GetEffectsVolume();
	}

	void Update () {
		//handle ESC input. if ingame, resume. if in options scene, load the main menu.
		if (Input.GetButtonDown ("Cancel")) {
            if (levelManager.currentSceneName.StartsWith( "01" )) 
				FindObjectOfType<LevelManager> ().LoadLevel ("01a Start");
		}
	}

	public void SaveAll() {
        //o slider vai de 0 a 1. reduzir pra -1 a 0 e multiplicar por alguma constante pra dar o valor em db final.
        SaveMusic();
        SaveEffects();
    }

    public void SaveEffects() {
        audioController.SetEffectsVolume( effectsSlider.value );
    }

    public void SaveMusic() {
        audioController.SetMusicVolume( musicSlider.value );
    }

	public void Resume() {
		SaveAll();
		if(UnpauseEvent!= null) {
			UnpauseEvent();
        }
    }


    public void SaveAndExit (){
		SaveAll ();
		levelManager.LoadLevel ("01a Start");
	}


	public void SetDefaults (){
		musicSlider.value = 0.8f;
		effectsSlider.value = 0.8f;
	}

	void OnDisable(){ SaveAll (); }
}
