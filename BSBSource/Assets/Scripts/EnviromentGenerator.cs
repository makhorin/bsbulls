using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnviromentGenerator : MonoBehaviour
{
    public EnviromentScroller[] EnviromentPatterns;
    
    private readonly List<EnviromentScroller> _currentEnviroment = new List<EnviromentScroller>();
    private readonly Quaternion _rotation = Quaternion.identity;

    public float Y;
    public float XOffset;

    protected virtual void Start()
    {
        if (EnviromentPatterns.Length == 0)
            return;

        var x = GameSettings.LeftBorder + XOffset;
        var newEnv = EnviromentPatterns[GameSettings.Rnd.Next(0, EnviromentPatterns.Length)];
        do
        {
            var go = Instantiate(newEnv, new Vector3(x - 0.05f, Y + newEnv.Height / 2f, 0f), _rotation);
            _currentEnviroment.Add(go);
            x += go.Width / 2f;
            newEnv = EnviromentPatterns[GameSettings.Rnd.Next(0, EnviromentPatterns.Length)];
            x += newEnv.Width / 2f;
        } while (x < GameSettings.RightBorder);
    }

    void Update()
    {
        GenerateEnviroment();
        RemoveFarObjects();
    }

    private void GenerateEnviroment()
    {
        if (EnviromentPatterns.Length == 0)
            return;

        var env = _currentEnviroment.Last();
        if (env.transform.position.x + env.Width / 2 > GameSettings.RightBorder)
            return;

        var newEnv = SelectBld();

        var pos = env.transform.position.x + newEnv.Width;
        var go = Instantiate(newEnv, new Vector3(pos - 0.05f, Y + newEnv.Height / 2f, 0f), _rotation);
        _currentEnviroment.Add(go);
    }

    protected virtual EnviromentScroller SelectBld()
    {
        return EnviromentPatterns[GameSettings.Rnd.Next(0, EnviromentPatterns.Length)];
    }

    private void RemoveFarObjects()
    {
        var toDestroy = new List<EnviromentScroller>();
        foreach (var env in _currentEnviroment)
        {
            if (env != null && env.transform.position.x < GameSettings.LeftBorder - env.Width)
            {
                toDestroy.Add(env);
            }
        }

        foreach (var des in toDestroy)
        {
            _currentEnviroment.Remove(des);
            Destroy(des.gameObject);
        }
    }
}

[Serializable]
public class BldWithChance
{
    public EnviromentScroller Bld;
    public float Chance;
    public bool IsStrip;
}
