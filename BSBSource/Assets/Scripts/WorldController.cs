using System;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    public Obstacle[] GroundObstaclesPatterns;
    public RunnerController RunnerPrefab;
    private readonly List<Obstacle> _currentObstacles = new List<Obstacle>();
    private readonly Quaternion _rotation = Quaternion.identity;

    private float _lastObstacle;

    public Vector3 StartPoint;

    private bool _started;

    private List<RunnerController> _startRunners = new List<RunnerController>();

    void Start()
    {
        for (var i = 0; i < GameSettings.MaxRunners; i++)
        {
            var line = GameSettings.GetRandomLine();
            var runnerPosKoef = GameSettings.Rnd.NextDouble();
            var runner = Instantiate(RunnerPrefab,
                new Vector3(-GameSettings.RunnersRadius + GameSettings.Center + 2 * GameSettings.RunnersRadius * (float)runnerPosKoef,
                GameSettings.Ground[line] + 0.1f),
                _rotation);
            runner.SetSettings(line, false);
            runner.GetComponent<Animator>().Play("Idle");
            _startRunners.Add(runner);
        }

        if (_started)
            StartGame();
    }

    internal void StartGame()
    {
        _lastObstacle = Time.time;
        foreach (var r in _startRunners)
            r.GetComponent<Animator>().Play("Run");
        _startRunners.Clear();
        _started = true;
    }

    void Update()
    {
        if (!_started)
            return;
        GenerateObstacles();
        RemoveFarObjects();
    }

    private void RemoveFarObjects()
    {
        var toDestroy = new List<Obstacle>();
        foreach (var obstacle in _currentObstacles)
        {
            if (obstacle != null && obstacle.transform.position.x < GameSettings.LeftBorder - obstacle.Width)
            {
                toDestroy.Add(obstacle);
            }
        }

        foreach (var des in toDestroy)
        {
            _currentObstacles.Remove(des);
            Destroy(des.gameObject);
        }
    }


    private void GenerateObstacles()
    {
        if (Time.time - _lastObstacle < GameSettings.ObstacleCooldownS)
            return;

        var chance = GameSettings.Rnd.NextDouble();

        if (chance > 1f - GameSettings.GroundObstaclesChance && GroundObstaclesPatterns != null && GroundObstaclesPatterns.Length > 0)
        {
            var line = GameSettings.GetRandomLine();
            var go = Instantiate(GroundObstaclesPatterns[GameSettings.Rnd.Next(0, GroundObstaclesPatterns.Length)], new Vector3(StartPoint.x, GameSettings.Ground[line]), _rotation);
            go.SetSettings(line);
            _currentObstacles.Add(go);
            _lastObstacle = Time.time;
        }
    }


}
