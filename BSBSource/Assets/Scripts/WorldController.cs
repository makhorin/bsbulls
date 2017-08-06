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

    public GameSettings Settings;

    void Start()
    {
        for (var i = 0; i < Settings.MaxRunners; i++)
        {
            var line = Settings.GetRandomLine();
            var runnerPosKoef = GameSettings.Rnd.NextDouble();
            var runner = Instantiate(RunnerPrefab, new Vector3(-Settings.RunnersRadius + Settings.Center + 2 * Settings.RunnersRadius  * (float)runnerPosKoef, Settings.Ground[line] + 0.1f), _rotation);
            runner.SetSettings(Settings, line,false);
        }
    }

    

    void Update()
    {
        GenerateObstacles();
        RemoveFarObjects();
    }

    private void RemoveFarObjects()
    {
        var toDestroy = new List<Obstacle>();
        foreach (var obstacle in _currentObstacles)
        {
            if (obstacle != null && obstacle.transform.position.x < Settings.LeftBorder - obstacle.Width)
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
        if (Time.time - _lastObstacle < Settings.ObstacleCooldownS)
            return;

        var chance = GameSettings.Rnd.NextDouble();

        if (chance > 1f - Settings.GroundObstaclesChance && GroundObstaclesPatterns != null && GroundObstaclesPatterns.Length > 0)
        {
            var line = Settings.GetRandomLine();
            var go = Instantiate(GroundObstaclesPatterns[GameSettings.Rnd.Next(0, GroundObstaclesPatterns.Length)], new Vector3(StartPoint.x, Settings.Ground[line]), _rotation);
            go.SetSettings(Settings, line);
            _currentObstacles.Add(go);
            _lastObstacle = Time.time;
        }
    }


}
