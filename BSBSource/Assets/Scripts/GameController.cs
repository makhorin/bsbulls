using System;
using Assets;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Text PlayerScoreField;
    public Text PlayerTimeField;
    public WorldController WorldController;
    public BullsController BullsController;
    public SpriteRenderer Black;

    private bool _started;

    public static GameStats GameStats;

    private void Awake()
    {
        GameStats = new GameStats();
    }

    void Start()
    {
        SetStats();
    }

    void Update ()
    {
        if (!_started)
            StartGame();

        GameStats.IncreaseScore();
        SetStats();
        GameStats.HandleSpeed();
    }
    
    private void SetStats()
    {
        PlayerScoreField.text = GameStats.Score.ToString();
        var t = TimeSpan.FromSeconds(GameStats.GetRunTime());
        PlayerTimeField.text = string.Format("{0:d2}:{1:d2}:{2:d2}", t.Hours, t.Minutes, t.Seconds);
    }

    public void StartGame()
    {
        _started = true;
        WorldController.StartGame();
        BullsController.StartGame();
        GameStats.StartGame();
    }
}
