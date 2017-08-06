using Assets;
using UnityEngine;

public class WaitingRunnerController : MonoBehaviour
{
    public int MinToAdd;
    public int MaxToAdd;
    public bool ShouldJump;
    public float ChanceToBeShown;
    public RunnerController RunnerObj;
    private GameSettings _settings;

    public int MinTaps;

    private bool _show;
    private int? toGenerate;

    public void SetSettings(GameSettings settings)
    {
        _settings = settings;
    }

    void Start ()
    {
        _show = GameSettings.Rnd.NextDouble() > 1f - ChanceToBeShown;
        if (!_show)
            Destroy(gameObject);
    }
    
    void Update ()
    {
        if (GameStats.GameOver)
            return;

        if (transform.position.x <= _settings.LeftBorder)
            return;

        if (MinTaps > 0)
        {
            if (InputHelper.LeftTap())
                MinTaps--;
            return;
        }

        if (!toGenerate.HasValue)
            return;

        enabled = false;
        for (int i = 0; i < toGenerate.Value; i++)
        {
            var go = Instantiate(RunnerObj, transform.position, Quaternion.identity);
            go.SetSettings(_settings, _settings.GetRandomLine(), ShouldJump);
        }
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (!_show || toGenerate.HasValue || collider.gameObject.tag != "Runner")
            return;
        toGenerate = GameSettings.Rnd.Next(MinToAdd, MaxToAdd);
    }
}
