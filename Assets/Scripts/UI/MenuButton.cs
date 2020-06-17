using UnityEngine;
using System.Collections;


[RequireComponent(typeof (AudioSource))]
public class MenuButton : MonoBehaviour {

	[SerializeField] private AudioClip clickSound;

	private LevelManager levelManager;
	private AudioSource audioSource;

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
		yield return (!audioSource.isPlaying);
		levelManager.LoadLevel(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
	}
		
	IEnumerator MainMenu(){
		Time.timeScale = 1f;
		yield return (!audioSource.isPlaying);
		levelManager.LoadLevel ("01a Start");
	}

	IEnumerator Play(){
		Time.timeScale = 1f;
		yield return (!audioSource.isPlaying);
		levelManager.LoadLevel ("01d Level Select");
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
