using System;
using Assets;
using UnityEngine;

public class BackBull : MonoBehaviour
{
    private GameSettings _settings;
    private bool _isInDash;
    private float _left;
    private float _lastDash;
    private bool _soundPlayed;
    private AudioSource _mooo;

    public void SetSettings(GameSettings settings, int line, float left)
    {
        _settings = settings;
        _left = left;
        gameObject.layer = _settings.BullLineLayers[line];
        _mooo = GetComponent<AudioSource>();
        foreach (var sp in GetComponentsInChildren<SpriteRenderer>())
            sp.sortingLayerName = _settings.BullSortingLayers[line];
        foreach (var sp in GetComponentsInChildren<ParticleSystem>())
            sp.GetComponent<Renderer>().sortingLayerName = _settings.BullSortingLayers[line];
    }


    void Update ()
    {
        var deltaSpeed = _settings.CurrentSpeed - _settings.Speed;
        var pos = transform.position.x;
        var speed = 0f;

        if (GameStats.GameOver)
            speed = _settings.CurrentSpeed * _settings.BackBullDashSpeedMultiplier;
        else if (deltaSpeed > 0f)
            speed = deltaSpeed + (_isInDash ? _settings.CurrentSpeed : 0f) * _settings.BackBullDashSpeedMultiplier;
        else if (deltaSpeed < 0f && pos > _left)
            speed = deltaSpeed;
        else if (Math.Abs(deltaSpeed) < 0.001f)
        {
            if (_isInDash)
                speed = _settings.CurrentSpeed * _settings.BackBullDashSpeedMultiplier;
            else if (pos > _left)
                speed = -_settings.BullStep;
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
                var maxDist = _settings.Center - _left;
                if (pos >= _left + maxDist  * _settings.BackBullApproachKoef)
                {
                    _isInDash = false;
                    _lastDash = Time.time;
                    _soundPlayed = false;
                }
            }  
        }
        else if (pos <= _left && Time.time - _lastDash > 5 && GameSettings.Rnd.NextDouble() > 1f - _settings.BackBullDashChance)
            _isInDash = true;
    }
}
