using Assets;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Text PlayerScoreField;
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
    }

    public void StartGame()
    {
        _started = true;
        WorldController.StartGame();
        BullsController.StartGame();
        GameStats.StartGame();
    }
}
