using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{

    [SerializeField] private AudioMixer audioMixer;

    private static readonly int soundMultiplier = 50;
    static private AudioController instance;

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

        Debug.Assert(instance.audioMixer != null, "no audiomixer found");
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadVolumes();
    }

    /// <summary>
    /// saves new volumes
    /// </summary>
    /// <param name="musicVolume">normalized music volume (0 to 1)</param>
    /// <param name="effectsVolume">normalized effects volume (0 to 1)</param>
    static public void SaveVolumes(float musicVolume, float effectsVolume)
    {
        instance.SetMusicVolume(musicVolume);
        instance.SetEffectsVolume(effectsVolume);
    }


    private void SetMusicVolume(float normalizedVolume)
    {
        //save the normalized value in playerprefs
        PlayerPrefsManager.SetMusicVolume(Mathf.Clamp01(normalizedVolume));
        //transform slider value to range in db
        float volume = (normalizedVolume - 1) * soundMultiplier;
        //send that value to the mixer
        instance.audioMixer.SetFloat("musicVolume", volume);
    }

    /// <summary>
    /// gets the music volume
    /// </summary>
    /// <returns>returns the music's volume normalized value(0 to 1)</returns>
    static public float GetMusicVolume()
    {
        //get normalized value from playerprefs
        return PlayerPrefsManager.GetMusicVolume();
    }


    private void SetEffectsVolume(float normalizedVolume)
    {
        //save the normalized value in playerprefs
        PlayerPrefsManager.SetEffectsVolume(Mathf.Clamp01(normalizedVolume));
        //transform slider value to range in db
        float volume = (normalizedVolume - 1) * soundMultiplier;
        //send that value to the mixer
        instance.audioMixer.SetFloat("effectsVolume", volume);
    }

    static public float GetEffectsVolume()
    {
        //get normalized value from playerprefs
        return PlayerPrefsManager.GetEffectsVolume();
    }

    static public void LoadVolumes()
    {
        instance.SetMusicVolume(GetMusicVolume());
        instance.SetEffectsVolume(GetEffectsVolume());
    }
}
