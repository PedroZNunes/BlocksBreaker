using UnityEngine;
using System.Collections;


[RequireComponent(typeof (AudioSource))]
public class MenuButton : MonoBehaviour {

	private LevelManager levelManager;
	private AudioSource audioSource;
	[SerializeField] private AudioClip clickSound;

	void Awake (){
		audioSource = GetComponent<AudioSource> ();
		Debug.Assert (clickSound != null, "No click sound");
		FindObjectOfType<Animator> ().speed = Random.Range (0.75f, 1.25f);
	}

	void Start(){
		levelManager = FindObjectOfType<LevelManager> ();
		Debug.Assert (levelManager != null, "Level Manager not found");
	}


	public void PlayClickSound(){
		audioSource.PlayOneShot (clickSound, 0.3f);
	}


	public void TryAgainWrapper(){	StartCoroutine (TryAgain ());	}
	public void MainMenuWrapper(){	StartCoroutine (MainMenu ());	}
	public void PlayWrapper(){		StartCoroutine (Play ());		}
	public void OptionsWrapper(){	StartCoroutine (Options ());	}
	public void StatsWrapper(){	StartCoroutine (Stats ());	}


	IEnumerator TryAgain(){
		Time.timeScale = 1f;
		levelManager.ResetProgress ();
		yield return (!audioSource.isPlaying);
		levelManager.LoadLevel("02 Level_01");
	}
		
	IEnumerator MainMenu(){
		Time.timeScale = 1f;
		yield return (!audioSource.isPlaying);
		levelManager.LoadLevel ("01a Start");
	}

	IEnumerator Play(){
		Time.timeScale = 1f;
		yield return (!audioSource.isPlaying);
		levelManager.LoadLevel ("02 Level_01");
	}

	IEnumerator Options(){
		yield return (!audioSource.isPlaying);
		levelManager.LoadLevel ("01b Options");
	}

	IEnumerator Stats (){
		yield return (!audioSource.isPlaying);
		levelManager.LoadLevel ("01c Stats");
	}

	public void Quit(){
		Application.Quit ();
	}



}
