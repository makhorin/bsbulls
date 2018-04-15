using System;
using System.Collections;
using System.Linq;
using Assets;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;

public class GameStarter : MonoBehaviour
{
    public AudioSource Intro;
    public AudioSource Ost;
    public AudioSource PreStomp;
    public AudioSource Stomp;
    public AudioSource Crowd;
    public AudioSource Menu;
    public AudioSource Strip;
    public SpriteRenderer Black;

    public AudioClip OstSound;
    public AudioClip PreOstSound;

    private bool _fading;

    public static bool ScoresFinished;



    void Start ()
    {
        DontDestroyOnLoad(gameObject);
        PlayerPrefs.SetFloat("Intro",Intro.volume);
        PlayerPrefs.SetFloat("Ost", Ost.volume);
        PlayerPrefs.SetFloat("PreStomp", PreStomp.volume);
        PlayerPrefs.SetFloat("Stomp", Stomp.volume);
        PlayerPrefs.SetFloat("Crowd", Crowd.volume);
        PlayerPrefs.SetFloat("Menu", Menu.volume);
        PlayerPrefs.Save();
        SceneManager.sceneLoaded += SceneManagerOnSceneLoaded;
    }
    
    void Update ()
    {
        if (GameController.GameStats != null && GameController.GameStats.GameOver)
            HandleGameOver();
        else if (_strip)
        {
            _strip = false;
            StartCoroutine("StripCoroutine");
        }
    }

    public void HandleStart()
    {
        ResetSounds();
        Ost.Stop();
        Crowd.Stop();
        Stomp.Stop();
        Menu.Stop();
        Intro.Play();
        ScoresFinished = false;
        Scores = null;
        UserProfiles = null;
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);  
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
        Intro.volume = PlayerPrefs.GetFloat("Intro", Intro.volume);
        Ost.volume = PlayerPrefs.GetFloat("Ost", Ost.volume);
        PreStomp.volume = PlayerPrefs.GetFloat("PreStomp", PreStomp.volume);
        Stomp.volume = PlayerPrefs.GetFloat("Stomp", Stomp.volume);
        Crowd.volume = PlayerPrefs.GetFloat("Crowd", Crowd.volume);
        Menu.volume = PlayerPrefs.GetFloat("Menu", Menu.volume);
    }

    private void HandleGameOver()
    {
        if (_fading)
            return;
        _fading = true;
        StartCoroutine("FadeOnGameOver");
    }

    private static bool _strip;
    public static void ToggleStrip()
    {
        _strip = true;
    }

    IEnumerator StripCoroutine()
    {
        PreStomp.Pause();
        Ost.Pause();
        Stomp.Pause();
        Crowd.Pause();

        Ost.volume = 0;
        Stomp.volume = 0;
        PreStomp.volume = 0;
        Crowd.volume = 0;

        Strip.Play();

        var timeToExit = Time.time + 1f;
        while (Time.time < timeToExit)
            yield return null;

        var elapsed = 0.0f;
        var duration = 1f;

        Ost.UnPause();
        Crowd.UnPause();
        Stomp.UnPause();
        PreStomp.UnPause();
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            var percentComplete = elapsed / duration;
            Ost.volume = Math.Min(PlayerPrefs.GetFloat("Ost", 1f), percentComplete);
            Stomp.volume = Math.Min(PlayerPrefs.GetFloat("Stomp", 1f), percentComplete);
            Crowd.volume = Math.Min(PlayerPrefs.GetFloat("Crowd", 1f), percentComplete);
            PreStomp.volume = Math.Min(PlayerPrefs.GetFloat("PreStomp", 1f), percentComplete);
            yield return null;
        }
    }

    IEnumerator FadeOnGameOver()
    {
        var elapsed = 0.0f;
        var duration = 3f;
        var startCrowd = Crowd.volume;
        var startStomp = Stomp.volume;
        var startOst = Ost.volume;
        var ostMin = 0.4f;
        try
        {
#if UNITY_ANDROID
            if (PlayGamesPlatform.Instance.localUser.authenticated)
                PlayGamesPlatform.Instance.ReportScore(GameController.GameStats.MaxScore, _leaderBoard, OnScoreReported);
#endif
        }
        catch(Exception e)
        {
            Debug.LogError(e);
        }

        var blk = Instantiate(Black);
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            var percentComplete = elapsed / duration;

            Crowd.volume = Math.Max(0f,startCrowd - percentComplete);
            Stomp.volume = Math.Max(0f, startStomp - percentComplete);
            Ost.volume = Math.Max(ostMin, startOst - percentComplete);

            blk.color = new Color(255,255,255,percentComplete);

            yield return null;
        }

        Crowd.Stop();
        Stomp.Stop();
        SceneManager.LoadScene("ScoreScreen");
        GameController.GameStats.GameOver = false;
        _fading = false;
    }

    public static IScore[] Scores;
    public static IUserProfile[] UserProfiles;
    private string _leaderBoard = "CgkIj9Sz_8UXEAIQAQ";

    
#if UNITY_ANDROID
    private void OnScoresLoaded(LeaderboardScoreData leaderboardScoreData)
    {
        try
        {
            var scores = leaderboardScoreData.Scores;
            if (scores == null)
            {
                Debug.LogWarning("No scores");
                ScoresFinished = true;
            }
            else
            {
                Debug.Log("Got scores");
                Scores = scores;
                PlayGamesPlatform.Instance.LoadUsers(Scores.Select(s => s.userID).ToArray(), OuUsersLoaded);
            }
        }
        catch(Exception e)
        {
            Debug.LogError(e);
        }
    }
#endif
    private void OuUsersLoaded(IUserProfile[] userProfiles)
    {
        try
        {
            if (userProfiles == null)
            {
                Debug.LogWarning("No scores");
                ScoresFinished = true;
            }
            else
            {
                Debug.Log("Got scores");
                UserProfiles = userProfiles;
            }
        }
        catch(Exception e)
        {
            Debug.LogError(e);
        }
    }

    private void OnScoreReported(bool reported)
    {
        try
        {
            if (!reported)
            {
                Debug.LogWarning("Report error");
                ScoresFinished = true;
            }
            else
            {
                Debug.Log("Reported");
#if UNITY_ANDROID
                PlayGamesPlatform.Instance.LoadScores(_leaderBoard, LeaderboardStart.TopScores, 5, LeaderboardCollection.Public, LeaderboardTimeSpan.AllTime, OnScoresLoaded);
#endif
            }
        }
        catch(Exception e)
        {
            Debug.LogError(e);
        }
        
    }
}

[Serializable]
public class DelayedObject
{
    public GameObject Obj;
    public float DelayS;
}
