using Assets;
using UnityEngine;

public class StripController : HousesScroller
{
    private bool _toggled;
    private float _lastPressRight;
    private int _minTaps;

    public Transform GPoint;

    public override bool CanBeShown
    {
        get
        {
            return GameController.GameStats.ShowStrip;
        }
    }

    protected override void Start ()
    {
        base.Start();
        _minTaps = GameSettings.MinTapsForStrip;
        GameController.GameStats.IsStrip = true;
    }

    private void OnDestroy()
    {
       GameController.GameStats.IsStrip = false;
    }
}
