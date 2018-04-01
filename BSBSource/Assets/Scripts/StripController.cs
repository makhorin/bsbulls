using Assets;
using System.Collections.Generic;
using UnityEngine;

public class StripController : MonoBehaviour
{
    private bool _toggled;
    private float _lastPressRight;
    private HashSet<RunnerController> _runners = new HashSet<RunnerController>();
    private HashSet<RunnerController> _toRemove = new HashSet<RunnerController>();

    public Transform GPoint;

    void Start ()
    {
        GameController.GameStats.IsStrip = true;
    }

    private void OnDestroy()
    {
        GameController.GameStats.IsStrip = false;
    }

    void Update ()
    {
        if (GameController.GameStats.GameOver)
            return;

        if (transform.position.x > GameSettings.RightBorder - 0.5f)
        {
            return;
        }

        if (transform.position.x < (GameSettings.RightBorder + GameSettings.LeftBorder) / 2f)
        {
            GameController.GameStats.ToggleSlowMotion();
            enabled = false;
        }

        if (InputHelper.RightTap())
        {
            _lastPressRight = Time.time;
            return;
        }
        else if (Time.time - _lastPressRight <= GameSettings.LastPressOffset)
            return;

        foreach (var runner in _runners)
        {
            if (!_toRemove.Contains(runner))
                runner.HandleStrip(GPoint.position);
        }

        if (_toggled)
            return;

        TutorController.ShowTutor(KeyCode.RightArrow, int.MaxValue);
        GameStarter.ToggleStrip();
        GameController.GameStats.ToggleSlowMotion();
        _toggled = true;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        switch (collider.gameObject.tag)
        {
            case "Runner":
                var runner = collider.gameObject.GetComponent<RunnerController>();
                _runners.Add(runner);
                break;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        switch (collider.gameObject.tag)
        {
            case "Runner":
                var runner = collider.gameObject.GetComponent<RunnerController>();
                _toRemove.Add(runner);
                runner.RigidBody.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
                break;
        }
    }
}
