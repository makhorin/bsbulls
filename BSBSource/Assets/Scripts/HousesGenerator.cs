using System;
using System.Collections.Generic;
using UnityEngine;

public class HousesGenerator : MonoBehaviour
{
    private float _lastSpecBld;

    public HousesScroller[] EnviromentPatterns;

    private readonly List<HousesScroller> _currentEnviroment = new List<HousesScroller>();
    private readonly Quaternion _rotation = Quaternion.identity;

    public float Y;
    public float XOffset;

    protected void Start()
    {
        _lastSpecBld = Time.time;
        if (EnviromentPatterns.Length == 0)
            return;

        var newEnv = EnviromentPatterns[GameSettings.Rnd.Next(0, EnviromentPatterns.Length)];
        var offset = GameSettings.LeftBorder;
        var sortingOrder = 0;
        do
        {
            var go = Instantiate(newEnv, new Vector3(offset, Y + newEnv.YOffset, 0f), _rotation);
            _currentEnviroment.Add(go);
            SetSortingOrger(go.gameObject, sortingOrder);
            sortingOrder-=10;

            var nextPossible = newEnv.NextPossibleHouses.Length;
            var index = GameSettings.Rnd.Next(0, nextPossible);
            NextBld newHouse;
            do
            {
                newHouse = newEnv.NextPossibleHouses[index];
                index++;
                if (nextPossible <= index)
                    index = 0;
            } while (!newHouse.Bld.CanBeShown);
            offset = go.transform.position.x + newHouse.XOffset;
            newEnv = newHouse.Bld;
        } while (offset < GameSettings.RightBorder);
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

        var env = _currentEnviroment[_currentEnviroment.Count - 1];
        var rightSide = env.transform.position.x;
        if (rightSide > GameSettings.RightBorder)
            return;

        var nextPossible = env.NextPossibleHouses.Length;
        var index = GameSettings.Rnd.Next(0, nextPossible);
        NextBld newHouse;
        do
        {
            newHouse = env.NextPossibleHouses[index];
            index++;
            if (nextPossible <= index)
                index = 0;
        } while (!newHouse.Bld.CanBeShown);
        
        var go = Instantiate(newHouse.Bld, new Vector3(env.transform.position.x + newHouse.XOffset, -100f, 0f), _rotation);
        var y = Y + go.YOffset;
        go.transform.SetPositionAndRotation(new Vector3(go.transform.position.x, y, 0f), _rotation);
        SetSortingOrger(go.gameObject, env.GetComponent<SpriteRenderer>().sortingOrder - 1);
        _currentEnviroment.Add(go);
    }

    private void RemoveFarObjects()
    {
        if (_currentEnviroment.Count <= 7)
            return;

        while (_currentEnviroment.Count > 7)
        {
            Destroy(_currentEnviroment[0].gameObject);
            _currentEnviroment.RemoveAt(0);
        }

        for (var i = 0; i < _currentEnviroment.Count; i++)
            SetSortingOrger(_currentEnviroment[i].gameObject, i * -10);
    }

    void SetSortingOrger(GameObject go, int order)
    {
        var baseRenderer = go.GetComponent<SpriteRenderer>().sortingOrder;
        foreach (var renderer in go.GetComponentsInChildren<SpriteRenderer>())
            renderer.sortingOrder = renderer.sortingOrder - baseRenderer + order;
    }

    protected HousesScroller SelectBld()
    {
        return EnviromentPatterns[GameSettings.Rnd.Next(0, EnviromentPatterns.Length)];
    }
}

[Serializable]
public class BldWithChance
{
    public HousesScroller Bld;
    public float Chance;
    public bool IsStrip;
}
