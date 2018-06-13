using System;
using System.Collections;
using Assets;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameStarter : MonoBehaviour
{
    public AudioSource Ost;
    public AudioSource PreStomp;
    public AudioSource Stomp;
    public AudioSource Crowd;
    public Black Black;
    public AudioClip OstSound;
    public AudioClip PreOstSound;
    public AudioClip Intro;
    private bool _fading;
    public GameObject[] DontDestroy;


    void Start ()
    {
        foreach(var dd in DontDestroy)
            DontDestroyOnLoad(dd);
        
        PlayerPrefs.SetFloat("Ost", Ost.volume);
        PlayerPrefs.SetFloat("PreStomp", PreStomp.volume);
        PlayerPrefs.SetFloat("Stomp", Stomp.volume);
        PlayerPrefs.SetFloat("Crowd", Crowd.volume);
        PlayerPrefs.Save();
        SceneManager.sceneLoaded += SceneManagerOnSceneLoaded;
    }

    private bool _starting;
    void Update ()
    {
        if (GameController.GameStats != null && GameController.GameStats.GameOver)
        {
            HandleGameOver();
        }
        else if (GameSettings.CanStartGame && !_starting)
        {
            _starting = InputHelper.LeftTap() || InputHelper.RightTap();
            if (_starting)
                StartCoroutine(HandleStart());
        }
    }

    public IEnumerator HandleStart()
    {
        ResetSounds();
        Ost.Stop();
        Crowd.Stop();
        Stomp.Stop();
        Ost.clip = Intro;
        Ost.loop = false;
        Ost.Play();
        yield return Black.Fade();
        SceneManager.LoadScene("MainScene");
    }

    private void SceneManagerOnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name != "MainScene")
            return;

        PreStomp.Play();
        Stomp.PlayDelayed(PreStomp.clip.length);
        Crowd.PlayDelayed(2f);

        StartCoroutine(PlayOst());
    }

    IEnumerator PlayOst()
    {
        Ost.clip = PreOstSound;
        Ost.loop = false;
        Ost.Play();
        yield return new WaitForSeconds(PreOstSound.length - 0.3f);
        Ost.clip = OstSound;
        Ost.loop = true;
        Ost.Play();
    }

    private void ResetSounds()
    {
        Ost.volume = PlayerPrefs.GetFloat("Ost", Ost.volume);
        PreStomp.volume = PlayerPrefs.GetFloat("PreStomp", PreStomp.volume);
        Stomp.volume = PlayerPrefs.GetFloat("Stomp", Stomp.volume);
        Crowd.volume = PlayerPrefs.GetFloat("Crowd", Crowd.volume);
    }

    private void HandleGameOver()
    {
        if (_fading)
            return;
        _starting = false;
        _fading = true;
        StartCoroutine(FadeOnGameOver());
    }

    IEnumerator FadeOnGameOver()
    {
        var elapsed = 0.0f;
        var duration = 3f;
        var startCrowd = Crowd.volume;
        var startStomp = Stomp.volume;
        var startOst = Ost.volume;
        var ostMin = 0.4f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            var percentComplete = elapsed / duration;

            Crowd.volume = Math.Max(0f,startCrowd - percentComplete);
            Stomp.volume = Math.Max(0f, startStomp - percentComplete);
            Ost.volume = Math.Max(ostMin, startOst - percentComplete);
            yield return null;
        }

        Black.gameObject.SetActive(true);
        yield return Black.Fade();

        Crowd.Stop();
        Stomp.Stop();
        SceneManager.LoadScene("ScoreScreen");
        GameController.GameStats.GameOver = false;
        _fading = false;
    }
}
