using System;
using Assets;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Text PlayerScoreField;
    public Text PlayerTimeField;
    public GameSettings Settings;
    private float _step;

    void Start()
    {
        _step = (Settings.MaxSpeed - Settings.DefaultSpeed) / (Settings.SecondsToReachMaxSpeed * 60);
    }

    void Update ()
    {
        PlayerScoreField.text = GameStats.Dead.ToString();
        var t = TimeSpan.FromSeconds(GameStats.GetRunTime());
        PlayerTimeField.text = string.Format("{0:d2}:{1:d2}:{2:d2}", t.Hours, t.Minutes, t.Seconds);
        Settings.CurrentSpeed += _step;
    }
}
