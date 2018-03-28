using System;
using Assets;
using UnityEngine;

public class BullsController : MonoBehaviour
{
    public GameObject[] BullPatterns;
    public GirlController Girl;

    public GameSettings Settings;
    private GameObject _frontBull;
    private float _lastFrontBull;

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
        for (var i = 0; i < 3; i++)
        {
            var xPos = GameSettings.LeftBorder + (GameSettings.BullMinOffset * 1.3f) +
                       (float)GameSettings.Rnd.NextDouble() * (GameSettings.BullMaxOffset - GameSettings.BullMinOffset);
            var go = Instantiate(BullPatterns[GameSettings.Rnd.Next(0, BullPatterns.Length)], new Vector3(xPos, GameSettings.Ground[i] + 1f), Quaternion.identity);
            go.AddComponent<BackBull>().SetSettings(i, xPos);
        }
        _started = true;
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
        if (BullPatterns.Length < 1 || Time.time - _lastFrontBull < GameSettings.FrontBullCooldownS || GameController.GameStats.IsStrip || GameController.GameStats.IsBackBull)
            return;

        var chance = GameSettings.Rnd.NextDouble();

        if (chance < 1f - GameSettings.FrontBullChance)
            return;
        var line = GameSettings.GetRandomLine();
        var go = Instantiate(BullPatterns[GameSettings.Rnd.Next(0, BullPatterns.Length)], new Vector3(GameSettings.RightBorder, GameSettings.Ground[line] + 1f), Quaternion.identity);
        var fBull = go.AddComponent<FrontBull>();
        fBull.SetSettings(line);
        _frontBull = go;
        _lastFrontBull = Time.time;

        _bullsSinceLastGirl++;

        if (_bullsSinceLastGirl >= GameSettings.BullsPerGirl)
        {
            var grl = Instantiate(Girl, new Vector2(GameSettings.LeftBorder, GameSettings.GirlY), Quaternion.identity);
            grl.TargetBull = fBull;
            _bullsSinceLastGirl = 0;
        }
    }
}
