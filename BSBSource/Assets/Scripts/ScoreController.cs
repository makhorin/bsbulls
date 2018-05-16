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

    
    void Update ()
    {
        //if (GameStarter.Scores != null)
        //{
        //    for (var i = 0; i < Scores.Length && i < GameStarter.Scores.Length; i++)
        //    {
        //        var userProfile = GameStarter.UserProfiles.FirstOrDefault(u => u.id == GameStarter.Scores[i].userID);
        //        var userName = userProfile == null ? GameStarter.Scores[i].userID : userProfile.userName;
        //        Scores[i].NameField.text = string.Format("{0}. {1}", GameStarter.Scores[i].rank, userName);
        //        Scores[i].ScoreField.text = GameStarter.Scores[i].formattedValue;
        //        Scores[i].NameField.gameObject.SetActive(true);
        //        Scores[i].ScoreField.gameObject.SetActive(true);
        //    }
        //}
    }
}

[Serializable]
public class Score
{
    public Text NameField;
    public Text ScoreField;
}
