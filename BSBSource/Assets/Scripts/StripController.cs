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
            return false;//GameController.GameStats.ShowStrip;
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

    protected override void Update ()
    {
        base.Update();
        if (GameController.GameStats.GameOver || 
            transform.position.x < (GameSettings.RightBorder + GameSettings.LeftBorder) / 2f ||
            _minTaps <= 0)
        {
            Time.timeScale = 1f;
            GameController.GameStats.IsStrip = false;
            return;
        }  
        else if (transform.position.x > GameSettings.RightBorder - 0.5f)
            return;

        if (InputHelper.RightTap() || InputHelper.LeftTap())
        {
            _minTaps--;
            return;
        }

        var runners = FindObjectsOfType<RunnerController>();

        foreach (var runner in runners)
            runner.HandleStrip(GPoint.position);

        if (_toggled)
            return;
        
        TutorController.ShowTutor(KeyCode.RightArrow, int.MaxValue);
        GameStarter.ToggleStrip();
        Time.timeScale = 0.2f;
        _toggled = true;
    }
}
