﻿using System;
using UnityEngine;

public class HousesScroller : MonoBehaviour {

    public NextBld[] NextPossibleHouses;
    public float YOffset;
    private Bounds _bounds;
    public Bounds Bounds
    {
        get
        {
            return _bounds;
        }
    }

    private void Awake()
    {
        GetBounds();
    }

    void Update ()
    {
        transform.Translate(-GameController.GameStats.Speed, 0f, 0f);
    }

    void GetBounds()
    {
        _bounds = new Bounds(transform.position, Vector3.zero);
        var renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            _bounds.Encapsulate(renderer.bounds);
        }
    }
}

[Serializable]
public class NextBld
{
    public HousesScroller Bld;
    public float XOffset;
    public short LayerOffset;
}
