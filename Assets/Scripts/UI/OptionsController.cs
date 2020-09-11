using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionsController : MonoBehaviour {

	[SerializeField] private Slider musicSlider, effectsSlider;

	private LevelManager levelManager;
	private AudioController audioController;

	/// <summary>
	/// Effects and Music Volume should make the bridge between slider.value and PlayerPrefsManager.get...()
	/// </summary>



    void Start () {

		levelManager = FindObjectOfType<LevelManager> ();
		Debug.Assert (levelManager != null, "Level Manager not Found");

		audioController = levelManager.GetComponent<AudioController>();


		musicSlider.value	= audioController.GetMusicVolume();
		effectsSlider.value = audioController.GetEffectsVolume();
	}

	void Update () {
		if (Input.GetButtonDown ("Cancel")) {
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


    public void SaveAndExit (){
		SaveAll ();
		levelManager.LoadLevel ("01a Start");
	}


	public void SetDefaults (){
		musicSlider.value = 1f;
		effectsSlider.value = 1f;
	}

	void OnDisable(){ SaveAll (); }
}
