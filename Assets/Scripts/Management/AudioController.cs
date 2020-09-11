using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{

	[SerializeField] private AudioMixer audioMixer;
	[SerializeField] private int soundMultiplier = 50;


	// Start is called before the first frame update
	void Start()
    {
		LoadVolumes();
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	public void SetMusicVolume( float normalizedVolume ) {
		//save the normalized value in playerprefs
		PlayerPrefsManager.SetMusicVolume( Mathf.Clamp01( normalizedVolume ) );
		//transform slider value to range in db
		float volume = (normalizedVolume - 1) * soundMultiplier;
		//send that value to the mixer
		audioMixer.SetFloat( "musicVolume", volume );
	}

	/// <summary>
	/// gets the music volume
	/// </summary>
	/// <returns>returns the music's volume normalized value(0 to 1)</returns>
	public float GetMusicVolume() {
		//get normalized value from playerprefs
		return PlayerPrefsManager.GetMusicVolume();
    }

	public void SetEffectsVolume (float normalizedVolume) {
		//save the normalized value in playerprefs
		PlayerPrefsManager.SetEffectsVolume( Mathf.Clamp01( normalizedVolume ) );
		//transform slider value to range in db
		float volume = (normalizedVolume - 1) * soundMultiplier;
		//send that value to the mixer
		audioMixer.SetFloat( "effectsVolume", volume );
	}

	public float GetEffectsVolume() {
		//get normalized value from playerprefs
		return PlayerPrefsManager.GetEffectsVolume();
	}

	public void LoadVolumes() {
		SetMusicVolume( GetMusicVolume() );
		SetEffectsVolume( GetEffectsVolume() );
    }
}
