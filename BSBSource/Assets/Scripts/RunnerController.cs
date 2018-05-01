﻿using System;
using System.Linq;
using Assets;
using UnityEngine;

public class RunnerController : MonoBehaviour
{
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

    private bool _canJump = false;
    private bool _bananaFall = false;
    private bool _instantDeath = false;
    private bool _jumpOnStart = false;
    

    public SpriteRenderer Shadow;
    public BloodSplat Blood;

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
            sp.sortingOrder = _line;

        _animation.SetFloat("RunOffset", (float)GameSettings.Rnd.NextDouble());

        if (_jumpOnStart)
            _animation.Play("Jump");
    }

    bool _isShocked;
    void Update ()
    {
        if (GameController.GameStats.IsStrip)
            RigidBody.constraints |= RigidbodyConstraints2D.FreezePositionY;
        else
            RigidBody.constraints &= ~RigidbodyConstraints2D.FreezePositionY;

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
            if (GameController.GameStats.IsBackBull && !_isShocked)
                _animation.Play("Shock");
            _isShocked = GameController.GameStats.IsBackBull;
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
         transform.position = Vector3.MoveTowards(transform.position, pos, GameController.GameStats.Speed);
        _animation.SetBool("IsRunning", false);
        if (Math.Abs(Vector3.Distance(pos, transform.position)) < 0.1f)
        {
            PlayDead();
            Destroy(gameObject);
        }
    }

    bool _isFastRunning;
    private void HandleRun()
    {
        if (GameController.GameStats.Speed == 0f)
            return;

        var centerDelta = transform.position.x - GameSettings.Center;

        if (centerDelta < -0.5)
        {
            if (GameController.GameStats.IsRunning)
                transform.Translate(GameSettings.Step * GameSettings.ApproachKoef, 0f, 0f);
        }
        else if (centerDelta > 1)
        {
            if (!GameController.GameStats.IsRunning)
                transform.Translate(-GameSettings.Step * GameSettings.ApproachKoef, 0f, 0f);
        }

        if (_isFastRunning != GameController.GameStats.IsRunning)
        {
            _isFastRunning = GameController.GameStats.IsRunning;
            if (_isFastRunning)
                _animation.Play("FastRun");
            else
                _animation.Play("Run");
        }

        _animation.SetBool("IsRunning", GameController.GameStats.IsRunning);
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

    private void OnCollisionStay2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Runner":
                if (collision.transform.position.y - collision.otherCollider.transform.position.y > 0.2f)
                    transform.Translate(-GameSettings.Step, 0f, 0f);
                else if (collision.transform.position.y - collision.otherCollider.transform.position.y < -0.2f)
                    transform.Translate(GameSettings.Step, 0f, 0f);
                break;
        }
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
        go.Blood.GetComponent<Renderer>().sortingLayerName = GameSettings.BullSortingLayers[_line];
        Destroy(go,5f);
    }
}
