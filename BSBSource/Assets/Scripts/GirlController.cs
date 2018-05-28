using System;
using Assets;
using UnityEngine;
public class GirlController : MonoBehaviour
{
    public FrontBull TargetBull;
    public SpriteRenderer Laser;
    public AudioSource LaserSound;
    private bool _laserUsed;
    private Animator _animator;

    void Start()
    {
        TutorController.ShowTutor(KeyCode.DownArrow, 1);
        GameController.GameStats.ShakeIt = true;
        _animator = GetComponent<Animator>();
    }


    void Update()
    {
        if (GameController.GameStats.GameOver || _laserUsed || !InputHelper.Swipe())
            return;
        _animator.Play("Scream");
    }

    void Fire()
    {
        try
        {
            if (Laser.transform.position.x > TargetBull.transform.position.x)
                return;
        }
        catch (Exception)
        {
            return;
        }

        var eyes = Laser.transform.position;
        var laserWidth = Laser.sprite.texture.width / Laser.sprite.pixelsPerUnit;

        var distance = Vector2.Distance(eyes, TargetBull.transform.position);
        var center = Vector2.MoveTowards(eyes, TargetBull.transform.position, distance / 2f);
        var scale = distance / laserWidth;
        Laser.transform.position = center;
        Laser.transform.localScale = new Vector3(scale, Laser.transform.localScale.y, 1f);

        var vectorToTarget = TargetBull.transform.position - (Vector3)center;
        var angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Laser.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        Laser.enabled = true;
        Destroy(Laser, 0.1f);
        LaserSound.Play();
        TargetBull.LaserHit();
        _laserUsed = true;
    }
}

