using UnityEngine;

public class BullsController : MonoBehaviour
{
    public FrontBull FrontBull;
    public BackBull BackBull;
    public GirlController Girl;

    public GameSettings Settings;
    private float _lastFrontBull;
    private FrontBull _frontBull;
    private int _bullsSinceLastGirl;
    private bool _started;

    void Update()
    {
        if (!_started)
            return;
        GenerateFrontBulls();
        RemoveFarObjects();
    }

    internal void StartGame()
    {
        _lastFrontBull = Time.time;
        _started = true;

        for (var i = 0; i < 3; i++)
        {
            var xPos = GameSettings.LeftBorder + GameSettings.BullMinOffset +
                       (float)GameSettings.Rnd.NextDouble() * (GameSettings.BullMaxOffset - 1.5f);
            var go = Instantiate(BackBull, new Vector3(xPos, GameSettings.Ground[i] + 1f), Quaternion.identity);
            go.SetSettings(i, xPos);
        }
    }

    public void StopGame()
    {
        _started = false;
        _frontBull = null;
    }

    private void RemoveFarObjects()
    {
        if (_frontBull != null && _frontBull.transform.position.x < GameSettings.LeftBorder - 5f)
        {
            Destroy(_frontBull);
            _frontBull = null;
        }
    }


    private void GenerateFrontBulls()
    {
        if (Time.time - _lastFrontBull < GameSettings.FrontBullCooldownS || GameController.GameStats.IsStrip || GameController.GameStats.IsBackBull)
            return;

        var chance = GameSettings.Rnd.NextDouble();

        if (chance < 1f - GameSettings.FrontBullChance)
            return;
        var line = GameSettings.GetRandomLine();
        var go = Instantiate(FrontBull, new Vector3(GameSettings.RightBorder, GameSettings.Ground[line] + 1f), Quaternion.identity);
        go.SetSettings(line);
        _frontBull = go;
        _lastFrontBull = Time.time;

        _bullsSinceLastGirl++;

        if (_bullsSinceLastGirl >= GameSettings.BullsPerGirl)
        {
            var grl = Instantiate(Girl, new Vector2(GameSettings.LeftBorder, GameSettings.GirlY), Quaternion.identity);
            grl.TargetBull = _frontBull;
            _bullsSinceLastGirl = 0;
        }
    }
}
