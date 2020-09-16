using UnityEngine;
using UnityEngine.UI;
using System;

public class OptionsController : MonoBehaviour {

	[SerializeField] private Slider musicSlider, effectsSlider;


	public static event Action UnpauseEvent;

	/// <summary>
	/// singleton Process
	/// </summary>
	static private OptionsController instance;

	void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}
	void Start () {
		musicSlider.value	= AudioController.GetMusicVolume();
		effectsSlider.value = AudioController.GetEffectsVolume();
	}

	void Update () {
		//handle ESC input. if ingame, resume. if in options scene, load the main menu.
		if (Input.GetButtonDown ("Cancel")) {
			if (!LevelManager.isSceneALevel)
				LevelManager.LoadMenu(MenuScenes.Start);
		}
	}

	public void SaveAll() {
		//o slider vai de 0 a 1. reduzir pra -1 a 0 e multiplicar por alguma constante pra dar o valor em db final.
		AudioController.SaveVolumes(musicSlider.value, effectsSlider.value);
    }

	public void Resume() {
		SaveAll();

		if(UnpauseEvent!= null) {
			UnpauseEvent();
        }
    }


    public void SaveAndExit (){
		SaveAll ();

		LevelManager.LoadMenu(MenuScenes.Start);
	}


	public void SetDefaults (){
		musicSlider.value = 0.8f;
		effectsSlider.value = 0.8f;
	}

	void OnDisable(){ SaveAll (); }
}
