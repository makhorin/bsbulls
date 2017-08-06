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
    public AudioSource PreOst;
    public AudioSource PreStomp;
    public AudioSource Stomp;
    public AudioSource Crowd;
    public AudioSource Menu;
    public AudioSource Strip;
    public SpriteRenderer Black;

    private bool _fading;

    public static GameSettings Settings;

    public static bool ScoresFinished;



    void Start ()
    {
        DontDestroyOnLoad(gameObject);
        GameStats.CanStartGame = true;

        PlayerPrefs.SetFloat("Intro",Intro.volume);
        PlayerPrefs.SetFloat("Ost", Ost.volume);
        PlayerPrefs.SetFloat("PreOst", PreOst.volume);
        PlayerPrefs.SetFloat("PreStomp", PreStomp.volume);
        PlayerPrefs.SetFloat("Stomp", Stomp.volume);
        PlayerPrefs.SetFloat("Crowd", Crowd.volume);
        PlayerPrefs.SetFloat("Menu", Menu.volume);
        PlayerPrefs.Save();

        SceneManager.sceneLoaded += SceneManagerOnSceneLoaded;
    }
    
    void Update ()
    {
        if (GameStats.CanStartGame && InputHelper.Down())
            HandleStart();

        else if (GameStats.GameOver)
            HandleGameOver();

        else if (_strip)
        {
            _strip = false;
            StartCoroutine("StripCoroutine");
        }
    }

    private void HandleStart()
    {
        ResetSounds();
        Ost.Stop();
        Crowd.Stop();
        Stomp.Stop();
        Menu.Stop();
        Intro.Play();
        GameStats.CanStartGame = false;
        GameStats.Reset();
        ScoresFinished = false;
        Scores = null;
        UserProfiles = null;
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);  
    }

    private void SceneManagerOnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name != "MainScene")
            return;

        GameStats.StartGame();
        PreStomp.Play();
        Stomp.PlayDelayed(3f);
        PreOst.PlayDelayed(1f);
        Ost.PlayDelayed(7f);
        Crowd.PlayDelayed(2f);
        Stomp.PlayDelayed(3f);
    }

    private void ResetSounds()
    {
        Intro.volume = PlayerPrefs.GetFloat("Intro", Intro.volume);
        Ost.volume = PlayerPrefs.GetFloat("Ost", Ost.volume);
        PreOst.volume = PlayerPrefs.GetFloat("PreOst", PreOst.volume);
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
        PreOst.Pause();
        PreStomp.Pause();
        Ost.Pause();
        Stomp.Pause();
        Crowd.Pause();

        Ost.volume = 0;
        Stomp.volume = 0;
        PreOst.volume = 0;
        PreStomp.volume = 0;
        Crowd.volume = 0;

        Strip.Play();

        var timeToExit = Time.time + 2f;
        var prevSpeed = Settings.CurrentSpeed;
        Settings.CurrentSpeed = Settings.DefaultSpeed * 0.7f;
        while (Time.time < timeToExit)
        {
            yield return null;
        }

        var elapsed = 0.0f;
        var duration = 1f;

        Ost.UnPause();
        Crowd.UnPause();
        Stomp.UnPause();
        PreOst.UnPause();
        PreStomp.UnPause();
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            var percentComplete = elapsed / duration;
            Ost.volume = Math.Min(PlayerPrefs.GetFloat("Ost", 1f), percentComplete);
            Stomp.volume = Math.Min(PlayerPrefs.GetFloat("Stomp", 1f), percentComplete);
            Crowd.volume = Math.Min(PlayerPrefs.GetFloat("Crowd", 1f), percentComplete);
            PreOst.volume = Math.Min(PlayerPrefs.GetFloat("PreOst", 1f), percentComplete);
            PreStomp.volume = Math.Min(PlayerPrefs.GetFloat("PreStomp", 1f), percentComplete);
            Settings.CurrentSpeed = prevSpeed * percentComplete;
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

        if (!PlayGamesPlatform.Instance.localUser.authenticated)
        {
            PlayGamesPlatform.Activate();
            PlayGamesPlatform.Instance.localUser.Authenticate(OnAuthenticate);
        }
        else
            PlayGamesPlatform.Instance.ReportScore(GameStats.MaxDead, _leaderBoard, OnScoreReported);

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
        GameStats.GameOver = false;
        _fading = false;
    }

    public static IScore[] Scores;
    public static IUserProfile[] UserProfiles;
    private string _leaderBoard = "CgkIuv20-qEQEAIQAQ";

    private void OnAuthenticate(bool isAuthenticated)
    {
        if (!isAuthenticated)
        {
            Debug.LogWarning("Auth error");
            return;
        }

        Debug.Log("Authenticated");
        PlayGamesPlatform.Instance.ReportScore(GameStats.MaxDead, _leaderBoard, OnScoreReported);

    }

    private void OnScoresLoaded(LeaderboardScoreData leaderboardScoreData)
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

    private void OuUsersLoaded(IUserProfile[] userProfiles)
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

    private void OnScoreReported(bool reported)
    {
        if (!reported)
        {
            Debug.LogWarning("Report error");
            ScoresFinished = true;
        }
        else
        {
            Debug.Log("Reported");
            PlayGamesPlatform.Instance.LoadScores(_leaderBoard, LeaderboardStart.TopScores, 5, LeaderboardCollection.Public, LeaderboardTimeSpan.AllTime, OnScoresLoaded);
        }
    }
}

[Serializable]
public class DelayedObject
{
    public GameObject Obj;
    public float DelayS;
}
