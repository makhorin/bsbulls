using System;
using Assets;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Text PlayerScoreField;
    public Text PlayerTimeField;
    private float _step;

    void Start()
    {
        _step = (GameSettings.MaxSpeed - GameSettings.DefaultSpeed) / (GameSettings.SecondsToReachMaxSpeed * 60);
    }

    void Update ()
    {
        GameStats.IncreaseScore();
        PlayerScoreField.text = GameStats.Score.ToString();
        var t = TimeSpan.FromSeconds(GameStats.GetRunTime());
        PlayerTimeField.text = string.Format("{0:d2}:{1:d2}:{2:d2}", t.Hours, t.Minutes, t.Seconds);
        GameStats.CurrentSpeed += _step;
    }
}
