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
    public GameObject Shop;

    private float _step;
    private bool _started;

    private void Start()
    {
        SetStats();
        GameSettings.DefaultSpeed = 0f;
    }

    void Update ()
    {
        if (!_started)
            return;

        GameStats.IncreaseScore();
        SetStats();
        GameStats.CurrentSpeed += _step;
    }

    private void SetStats()
    {
        PlayerScoreField.text = GameStats.Score.ToString();
        var t = TimeSpan.FromSeconds(GameStats.GetRunTime());
        PlayerTimeField.text = string.Format("{0:d2}:{1:d2}:{2:d2}", t.Hours, t.Minutes, t.Seconds);
    }

    public void StartGame()
    {
        Destroy(Shop);
        GameStats.Reset();
        _started = true;
        GameSettings.DefaultSpeed = 5f;
        WorldController.StartGame();
        BullsController.StartGame();
    }

    public void BuyStamina()
    {
        GameSettings.MaxStamina++;
    }
}
