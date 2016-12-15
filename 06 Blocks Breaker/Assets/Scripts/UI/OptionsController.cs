using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionsController : MonoBehaviour {

	[SerializeField] private AudioMixer audioMixer;
	[SerializeField] private Slider musicSlider, effectsSlider;

	private LevelManager levelManager;
	private float EffectsVolume {
		get { 
			float volume;
			audioMixer.GetFloat ("effectsVolume", out volume);
			return volume;
		}
		set{ audioMixer.SetFloat ("effectsVolume", value); }
	}

	private float MusicVolume{
		get{ 
			float volume;
			audioMixer.GetFloat ("musicVolume", out volume);
			return volume;
		}
		set{ audioMixer.SetFloat ("musicVolume", value);}
	}




	void Start () {

		levelManager = FindObjectOfType<LevelManager> ();
		Debug.Assert (levelManager != null, "Level Manager not Found");

		musicSlider.value = PlayerPrefsManager.GetMusicVolume ();
		effectsSlider.value = PlayerPrefsManager.GetEffectsVolume ();
	}

	void Update () {
		EffectsVolume = effectsSlider.value;
		MusicVolume = musicSlider.value;
	}

	public void SaveAll(){
		PlayerPrefsManager.SetEffectsVolume (effectsSlider.value);
		PlayerPrefsManager.SetMusicVolume (musicSlider.value);
	}

	public void SaveAndExit (){
		SaveAll ();
		levelManager.LoadLevel ("01a Start");
	}


	public void SetDefaults (){
		musicSlider.value = 0f;
		effectsSlider.value = 0f;
	}

	void OnDisable(){ SaveAll (); }
}
