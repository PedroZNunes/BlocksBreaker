using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource), typeof(Animator))]
public class MenuButton : MonoBehaviour
{

    [SerializeField] private AudioClip clickSound;

    private AudioSource audioSource;
    private Animator animator;

    // public static event Action ResumeEvent;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

        Debug.Assert(clickSound != null, "No click sound");
        animator.speed = Random.Range(0.75f, 1.25f);
    }


    public void PlayClickSound()
    {
        audioSource.PlayOneShot(clickSound, 0.3f);
    }


    public void TryAgainWrapper() { StartCoroutine(TryAgain()); }
    public void MainMenuWrapper() { StartCoroutine(MainMenu()); }
    public void PlayWrapper() { StartCoroutine(Play()); }
    public void OptionsWrapper() { StartCoroutine(Options()); }
    public void ResumeOptionsWrapper()
    {
        StartCoroutine(Resume());

    }
    public void StatsWrapper() { StartCoroutine(Stats()); }
    public void QuitWrapper() { StartCoroutine(Quit()); }


    IEnumerator TryAgain()
    {
        PlayClickSound();
        Time.timeScale = 1f;
        yield return (!audioSource.isPlaying);
        LevelManager.ReloadCurrentLevel();

    }

    IEnumerator MainMenu()
    {
        PlayClickSound();
        Time.timeScale = 1f;
        yield return (!audioSource.isPlaying);
        LevelManager.LoadMenu(MenuScenes.Start);
    }

    IEnumerator Play()
    {
        PlayClickSound();
        Time.timeScale = 1f;
        yield return (!audioSource.isPlaying);
        LevelManager.LoadMenu(MenuScenes.LevelSelect);
    }

    IEnumerator Options()
    {
        PlayClickSound();
        Time.timeScale = 1f;
        yield return (!audioSource.isPlaying);
        LevelManager.LoadMenu(MenuScenes.Options);
    }

    IEnumerator Stats()
    {
        PlayClickSound();
        Time.timeScale = 1f;
        yield return (!audioSource.isPlaying);
        LevelManager.LoadMenu(MenuScenes.Stats);
    }

    IEnumerator Quit()
    {
        PlayClickSound();
        yield return (!audioSource.isPlaying);

        if (LevelManager.isSceneALevel)
        {
            OptionsController optController = FindObjectOfType<OptionsController>();
            optController.SaveAndExit();
        }
        else
        {
            Application.Quit();
        }

    }

    IEnumerator Resume()
    {
        PlayClickSound();
        yield return (!audioSource.isPlaying);
        //call options controller resume
        OptionsController optController = FindObjectOfType<OptionsController>();
        if (optController != null)
        {
            if (LevelManager.isSceneALevel)
            {
                optController.Resume();
            }
            else
            {
                Debug.LogError("using resume in a non-level scene");
                optController.SaveAndExit();
            }
        }
    }


}
