using Assets;
using UnityEngine;

public class StripController : MonoBehaviour
{

    private Collider2D _collider2D;
    private bool _toggled;
    void Start ()
    {
        _collider2D = GetComponent<Collider2D>();
    }

    void Update ()
    {
        if (GameStats.GameOver || _toggled)
            return;

        if (transform.position.x > GameStarter.Settings.RightBorder - 0.5f)
            return;
        _collider2D.enabled = true;
        TutorController.ShowTutor(KeyCode.RightArrow, int.MaxValue);
        GameStarter.ToggleStrip();
        _toggled = true;
    } 
}
