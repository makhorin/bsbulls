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

    private void Start()
    {
        _lastDash = Time.time;
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
        GameStats.IsBackBull = false;
    }

    void Update ()
    {
        var deltaSpeed = GameStats.CurrentSpeed - GameStats.Speed;
        var pos = transform.position.x;
        var speed = 0f;

        if (GameStats.GameOver)
            speed = GameStats.CurrentSpeed * GameSettings.BackBullDashSpeedMultiplier;
        else if (deltaSpeed > 0f)
            speed = deltaSpeed + (_isInDash ? GameStats.CurrentSpeed : 0f) * GameSettings.BackBullDashSpeedMultiplier;
        else if (deltaSpeed < 0f && pos > _left)
            speed = deltaSpeed;
        else if (Math.Abs(deltaSpeed) < 0.001f)
        {
            if (_isInDash)
                speed = GameStats.CurrentSpeed * GameSettings.BackBullDashSpeedMultiplier;
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
                    GameStats.IsBackBull = false;
                }
            }  
        }
        else if (pos <= _left && Time.time - _lastDash > GameSettings.DashCooldownS && 
            GameSettings.BackBullDashChance > GameSettings.Rnd.NextDouble() && !GameStats.IsFrontBull && !GameStats.IsStrip)
        {
            _lastDash = Time.time;
            _isInDash = true;
            GameStats.IsBackBull = true;
        }
            
    }
}
