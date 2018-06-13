using Assets;
using GameAnalyticsSDK;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public WorldController WorldController;
    public BullsController BullsController;
    public HousesGenerator HousesGenerator;
    public Black Black;
    private bool _started;

    public static GameStats GameStats;

    private void Start()
    {
        GameAnalytics.Initialize();
        SceneManager.sceneLoaded += SceneLoaded;
        SceneManager.sceneUnloaded += SceneUnloaded;
    }

    private void SceneUnloaded(Scene scene)
    {
        if (scene.name != "MainScene")
            return;
        StopGame();
    }

    private void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch(scene.name)
        {
            case "MainScene":
            {
                InitGame();
                StartCoroutine(StartGame());
                break;
            }
            case "ScoreScreen":
            {
                StartCoroutine(Black.Show());
                break;
            }
        }       
    }

    void Update()
    {
        if (!_started)
            return;

        GameStats.IncreaseScore();
        GameStats.HandleSpeed();
    }

    public void InitGame()
    {
        GameStats = new GameStats();
        WorldController.InitGame();
        HousesGenerator.InitGame();
    }

    public IEnumerator StartGame()
    {
        yield return Black.Show();
        Black.gameObject.SetActive(false);
        GameObject.FindGameObjectWithTag("controls").SetActive(false);
        GameStats.StartGame();
        WorldController.StartGame();
        BullsController.StartGame();
        HousesGenerator.StartGame();
        _started = true;
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "game");
    }

    public void StopGame()
    {
        _started = false;
        WorldController.StopGame();
        BullsController.StopGame();
        HousesGenerator.StopGame();
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "game", GameStats.Score);
    }
}
