using System;
using Assets;
using UnityEngine;

public class BackBull : MonoBehaviour
{
    private bool _isInDash;
    private float _left;
    private float _lastDash;
    private bool _soundPlayed;
    private AudioSource _mooo;
    private Animator _animator;
    private void Start()
    {
        _lastDash = Time.time;
        _animator = GetComponent<Animator>();
    }

    public void SetSettings(int line, float left)
    {
        _left = left;
        gameObject.layer = GameSettings.BullLineLayers[line];
        _mooo = GetComponent<AudioSource>();
        foreach (var sp in GetComponentsInChildren<SpriteRenderer>())
            sp.sortingLayerName = GameSettings.BullSortingLayers[line];
        foreach (var sp in GetComponentsInChildren<ParticleSystem>())
            sp.GetComponent<Renderer>().sortingLayerName = GameSettings.BullSortingLayers[line];
    }

    void OnDestroy()
    {
        GameController.GameStats.IsBackBull = false;
    }

    void Update ()
    {
        var isRunning = GameController.GameStats.IsRunning;
        var pos = transform.position.x;
        var speed = 0f;

        if (GameController.GameStats.GameOver)
            speed = GameSettings.BullStep * GameSettings.BackBullDashSpeedMultiplier;
        else if (isRunning && pos > _left)
            speed = -GameSettings.BullStep;
        else
        {
            if (_isInDash)
                speed = GameSettings.BullStep * GameSettings.BackBullDashSpeedMultiplier;
            else if (pos > _left)
                speed = -GameSettings.BullStep;
        }

        transform.Translate(speed, 0f, 0f);
        HandleDash(pos);
    }

    private void HandleDash(float pos)
    {
        if (_isInDash)
        {
            if (!_soundPlayed)
                _mooo.Play();
            _soundPlayed = true;
            
            if (_isInDash)
            {
                var maxDist = GameSettings.Center - _left;
                if (Time.time - _lastDash > GameSettings.DashDurationS || 
                    pos >= _left + maxDist  * GameSettings.BackBullApproachKoef)
                {
                    _isInDash = false;
                    _soundPlayed = false;
                    GameController.GameStats.IsBackBull = false;
                }
            }  
        }
        else if (pos <= _left && Time.time - _lastDash > GameSettings.DashCooldownS && 
            GameSettings.BackBullDashChance > GameSettings.Rnd.NextDouble() && !GameController.GameStats.IsFrontBull && !GameController.GameStats.IsStrip)
        {
            _lastDash = Time.time;
            _isInDash = true;
            GameController.GameStats.IsBackBull = true;
        }
            
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                _animator.Play("BullRage");
                break;
        }
    }
}
