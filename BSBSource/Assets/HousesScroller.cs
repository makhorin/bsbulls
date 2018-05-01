using System;
using UnityEngine;

public class HousesScroller : MonoBehaviour {

    public NextBld[] NextPossibleHouses;
    public float YOffset;
    public virtual bool CanBeShown
    {
        get {return true;}
    }
    protected virtual void Start()
    {
        transform.Translate(-GameController.GameStats.Speed, 0f, 0f);
    }

    protected virtual void Update ()
    {
       transform.Translate(-GameController.GameStats.Speed, 0f, 0f);
    }
}

[Serializable]
public class NextBld
{
    public HousesScroller Bld;
    public float XOffset;
}
