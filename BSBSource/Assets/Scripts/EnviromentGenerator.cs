using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnviromentGenerator : MonoBehaviour
{
    public HousesScroller[] EnviromentPatterns;
    
    private readonly List<HousesScroller> _currentEnviroment = new List<HousesScroller>();
    private readonly Quaternion _rotation = Quaternion.identity;

    public float Y;
    public float XOffset;

    protected virtual void Start()
    {
        if (EnviromentPatterns.Length == 0)
            return;

        var x = GameSettings.LeftBorder + XOffset;
        var newEnv = EnviromentPatterns[GameSettings.Rnd.Next(0, EnviromentPatterns.Length)];
        float? offset = null;
        do
        {
            var go = Instantiate(newEnv, new Vector3(0f, -100f, 0f), _rotation);
            go.transform.SetPositionAndRotation(new Vector3((offset.HasValue ?
                offset.Value
                : x + go.Bounds.extents.x), Y + go.Bounds.extents.y + go.YOffset, 0f), _rotation);
            _currentEnviroment.Add(go);
            x += go.Bounds.extents.x;
            var newHouse = newEnv.NextPossibleHouses[GameSettings.Rnd.Next(0, newEnv.NextPossibleHouses.Length)];
            offset = go.transform.position.x + newHouse.XOffset;
            newEnv = newHouse.Bld;
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
        var rightSide = env.transform.position.x + env.Bounds.extents.x - 0.2f;
        if (rightSide > GameSettings.RightBorder)
            return;

        var newEnv = env.NextPossibleHouses[GameSettings.Rnd.Next(0, env.NextPossibleHouses.Length)];

        var go = Instantiate(newEnv.Bld, new Vector3(env.transform.position.x + newEnv.XOffset, -100f, 0f), _rotation);
        var y = go.Bounds.extents.y + Y + go.YOffset;
        go.transform.SetPositionAndRotation(new Vector3(go.transform.position.x, y, 0f), _rotation);
        go.GetComponent<SpriteRenderer>().sortingOrder = env.GetComponent<SpriteRenderer>().sortingOrder + newEnv.LayerOffset;
        _currentEnviroment.Add(go);
    }

    protected virtual HousesScroller SelectBld()
    {
        return EnviromentPatterns[GameSettings.Rnd.Next(0, EnviromentPatterns.Length)];
    }

    private void RemoveFarObjects()
    {
        var toDestroy = new List<HousesScroller>();
        foreach (var env in _currentEnviroment)
        {
            if (env != null && env.transform.position.x < GameSettings.LeftBorder - env.Bounds.size.x)
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
    public HousesScroller Bld;
    public float Chance;
    public bool IsStrip;
}
