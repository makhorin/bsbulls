using System;
using System.Linq;
using Assets;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    public Text PlayerScoreField;
    public Text PlayerTimeField;
    public Score[] Scores;
    
    

    void Awake()
    {
        foreach (var s in Scores)
        {
            s.NameField.gameObject.SetActive(false);
            s.ScoreField.gameObject.SetActive(false);
        }
    }

    void Start ()
    {
        GameStats.CanStartGame = true;
        PlayerScoreField.text = GameStats.Dead.ToString();
        var t = TimeSpan.FromSeconds(GameStats.RunTime);
        PlayerTimeField.text = string.Format("{0:d2}:{1:d2}:{2:d2}", t.Hours, t.Minutes, t.Seconds);
    }

    
    void Update ()
    {
        if (GameStarter.ScoresFinished)
        {
            enabled = false;
            return;
        }

        if (GameStarter.Scores != null)
        {
            for (var i = 0; i < Scores.Length && i < GameStarter.Scores.Length; i++)
            {
                var userProfile = GameStarter.UserProfiles.FirstOrDefault(u => u.id == GameStarter.Scores[i].userID);
                var userName = userProfile == null ? GameStarter.Scores[i].userID : userProfile.userName;
                Scores[i].NameField.text = string.Format("{0}. {1}", GameStarter.Scores[i].rank, userName);
                Scores[i].ScoreField.text = GameStarter.Scores[i].formattedValue;
                Scores[i].NameField.gameObject.SetActive(true);
                Scores[i].ScoreField.gameObject.SetActive(true);
            }

            enabled = false;
        }
    }

    public void Restart()
    {
        var gs = FindObjectOfType<GameStarter>();
        gs.HandleStart();
    }
}

[Serializable]
public class Score
{
    public Text NameField;
    public Text ScoreField;
}
