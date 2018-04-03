﻿using System;
using System.Linq;
using Assets;
using UnityEngine;

public class RunnerController : MonoBehaviour
{
    private GameSettings _settings;
    private float _translateX;
    public Rigidbody2D RigidBody;
    private Animator _animation;
    private float _defaultSpeedMultiplier = 2;
    private float _maxSpeedMultiplier = 5;
    private float _minSpeedMultiplier = 1;
    private int _line;

    private Vector2 _shadowPos;
    private Vector2 _shadowScale;

    public Rigidbody2D[] Kishki;

    public AudioSource SoundHole;
    public AudioSource SoundHoleCry;
    public AudioSource SoundBullCry;
    public AudioSource SoundBanana;
    public AudioSource SoundBananaSmash;
    public AudioSource SoundBullSmash;

    private bool _canJump = true;
    private bool _bananaFall = false;
    private bool _instantDeath = false;
    private bool _jumpOnStart = false;
    

    public SpriteRenderer Shadow;
    public GameObject Blood;

    public event Action<GameObject> IamDead;

    private bool _dead;

    public void SetSettings(int line, bool jumpOnStart)
    {
        gameObject.layer = GameSettings.LineLayers[line];
        _line = line;
        foreach (var sp in GetComponentsInChildren<SpriteRenderer>())
            sp.sortingLayerName = GameSettings.RunnersSortingLayers[line];
        Shadow.sortingOrder = -1;
        _shadowScale = Shadow.transform.localScale;
        _shadowPos = Shadow.transform.position;
    }

    void Awake()
    {
        GameController.GameStats.RegisterRunner(this);
        RigidBody = GetComponent<Rigidbody2D>();
        _animation = GetComponent<Animator>();
        foreach (var sp in GetComponentsInChildren<SpriteRenderer>())
        {
            sp.sortingOrder = _line;
        }
        _animation.SetFloat("SpeedMultiplier", _defaultSpeedMultiplier);
        _animation.SetFloat("RunOffset", (float)GameSettings.Rnd.NextDouble());

        if (_jumpOnStart)
            _animation.Play("Jump");
    }

    void Update ()
    {

        var p = transform.position;
        if (p.x > GameSettings.RightBorder || 
            p.x < GameSettings.LeftBorder || 
            p.y > GameSettings.Air || 
            p.y < GameSettings.Ground.Last())
        {
            PlayDead();
            enabled = false;
            Destroy(gameObject);
        }

        if (_instantDeath || GameController.GameStats.GameOver)
        {
            HandleDeath();
        }
        else if (_bananaFall)
        {
            HandleFall();
        }
        else
        {
            HandleJump();
            HandleRun();
            HandleShadow();
        }
    }

    private void HandleShadow()
    {
        var ground = GameSettings.Ground[_line];
        Shadow.transform.position = new Vector3(Shadow.transform.position.x, _shadowPos.y, Shadow.transform.position.z);
        var delta = 1 - Math.Min((transform.position.y - 2f) - ground, 1f);
        Shadow.transform.localScale = new Vector2(Math.Min(_shadowScale.x,_shadowScale.x * delta),
            Math.Min(_shadowScale.y,_shadowScale.y * delta));
    }

    public void HandleStrip(Vector3 pos)
    {
        if (!_canJump)
            return;
        RigidBody.constraints |= RigidbodyConstraints2D.FreezePositionY;
        transform.position = Vector3.MoveTowards(transform.position, pos, GameController.GameStats.Speed * 2f);
        _animation.SetFloat("SpeedMultiplier", _defaultSpeedMultiplier);
        if (Math.Abs(Vector3.Distance(pos, transform.position)) < 0.1f)
        {
            PlayDead();
            Destroy(gameObject);
        }
    }

    private void HandleRun()
    {
        if (GameController.GameStats.Speed == 0f)
            return;

        if (Math.Abs(_translateX) < GameSettings.Step)
        {
            var centerDelta = transform.position.x - GameSettings.Center;
            _translateX = centerDelta * GameSettings.ApproachKoef;

            if (GameController.GameStats.IsRunning)
                _translateX = Math.Min(_translateX, 0f);

            _animation.SetFloat("SpeedMultiplier", _defaultSpeedMultiplier);
        }
        else
        {
            var sign = Math.Sign(_translateX);
            var step = sign * GameSettings.Step;
            _translateX -= step;
            transform.Translate(-step, 0f, 0f);

            if (GameController.GameStats.IsRunning)
                _animation.SetFloat("SpeedMultiplier", _maxSpeedMultiplier);
            else
                _animation.SetFloat("SpeedMultiplier", _minSpeedMultiplier);
        }        
    }

    private void HandleJump()
    {
        if (!_canJump || !InputHelper.Up())
            return;
        _canJump = false;
        var jumpKoef = (float)GameSettings.Rnd.NextDouble() * GameSettings.RandomJumpMultipier;
        var jump = GameSettings.MinJumpHeight + jumpKoef;
        if (GameController.GameStats.IsRunning)
            jump *= 1.2f;
        RigidBody.AddForce(transform.up * jump);
        _animation.Play("Jump");
    }

    private void HandleDeath()
    {
        foreach (var sp in GetComponentsInChildren<SpriteRenderer>())
        {
            sp.color = new Color(0, 0, 0, 0);
        }
        Destroy(GetComponent<Collider2D>());
        RigidBody.constraints |= RigidbodyConstraints2D.FreezePositionY;
        Destroy(RigidBody);
        SpawnKishki();
        enabled = false;
    }

    private void HandleFall()
    {
        transform.Translate(-GameController.GameStats.Speed, 0f, 0f);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Bull":
                _instantDeath = true;
                Destroy(gameObject, 5f);
                SoundBullSmash.Play();
                SoundBullCry.PlayDelayed(0.1f);
                SoundBananaSmash.PlayDelayed(0.5f);
                Shadow.enabled = false;
                PlayDead();
                break;
            case "Runner":
                foreach (var p in collision.contacts)
                {
                    if(p.point.y < transform.position.y || 
                        Math.Abs(p.point.x - transform.position.x) > 0.4f)
                        return;
                }

                Fall();
                break;
            case "Line":
                _canJump = true;
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        switch (collider.gameObject.tag)
        {
            case "Hole":
                _instantDeath = true;
                Destroy(gameObject,5f);
                Shadow.enabled = false;
                SoundHole.Play();
                SoundHoleCry.PlayDelayed(0.05f);
                transform.position = new Vector3(collider.gameObject.transform.position.x, transform.position.y, 0f);
                PlayDead();
                break;
            case "Banana":
                SoundBanana.Play();
                Fall();
                Destroy(collider.gameObject);
                break;
        }
    }

    public void Fall()
    {
        _bananaFall = true;
        Destroy(gameObject, 5f);
        _animation.Play("Fall");
        SoundBanana.Play();
        SoundBananaSmash.PlayDelayed(0.45f);
        gameObject.layer = GameSettings.FallenLineLayers[_line];
        Shadow.enabled = false;
        PlayDead();
    }

    void PlayDead()
    {
        if (_dead)
            return;
        _dead = true;
        if (IamDead != null)
            IamDead(gameObject);
    }

    private Vector2[] _forcesOrder = 
    {
        Vector2.up,
        Vector2.right
    };

    void SpawnKishki()
    {
        var rnd = GameSettings.Rnd;
        var rndStart = rnd.Next(0, Kishki.Length);
        var j = 0;
        for (var i = rndStart; i < Kishki.Length + rndStart; i++)
        {
            var ii = i >= Kishki.Length ? i - Kishki.Length : i;
            var k = Instantiate(Kishki[ii], transform.position, Quaternion.identity);
            k.gameObject.layer = GameSettings.KishkiLayers[_line];
            k.AddTorque(2f);
            k.AddForce(new Vector2((float)rnd.NextDouble(), Math.Min(1f,(float)rnd.NextDouble() + 0.5f)) * 500f);
            j++;
            if (j >= _forcesOrder.Length)
                j = 0;
        }
        var go = Instantiate(Blood, new Vector3(transform.position.x, GameSettings.Ground[_line]), Quaternion.identity);
        Destroy(go,5f);
    }
}
