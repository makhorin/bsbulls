using System;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    public Text PlayerScoreField;
    public Text PlayerTimeField;
    public Score[] Scores;

    private int _score;

    void Start ()
    {
        _score = PlayerPrefs.GetInt("score", 0);
        PlayerScoreField.text = _score.ToString();
        var t = TimeSpan.FromSeconds(PlayerPrefs.GetFloat("runTime", 0f));
        PlayerTimeField.text = string.Format("{0:d2}:{1:d2}:{2:d2}", t.Hours, t.Minutes, t.Seconds);
        GameSettings.CanStartGame = true;
    }
}

[Serializable]
public class Score
{
    public Text NameField;
    public Text ScoreField;
}
