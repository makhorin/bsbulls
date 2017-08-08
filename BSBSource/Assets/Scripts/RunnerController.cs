using System;
using System.Linq;
using Assets;
using UnityEngine;

public class RunnerController : MonoBehaviour
{
    private GameSettings _settings;
    private float _translateX;
    private Rigidbody2D _rigidBody;
    private Animator _animation;
    private float _defaultSpeedMultiplier = 2;
    private float _maxSpeedMultiplier = 5;
    private float _minSpeedMultiplier = 1;
    private int _line;

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
    private bool _goingToStrip = false;
    private float _lastPressRight;
    private GameObject _stripBar;

    public SpriteRenderer Shadow;
    public GameObject Blood;

    public event Action<GameObject> IamDead;

    private bool _dead;

    public void SetSettings(GameSettings settings, int line, bool jumpOnStart)
    {
        _settings = settings;
        gameObject.layer = _settings.LineLayers[line];
        _line = line;
        foreach (var sp in GetComponentsInChildren<SpriteRenderer>())
            sp.sortingLayerName = _settings.RunnersSortingLayers[line];
    }

    void Awake()
    {
        GameStats.RegisterRunner(this);
        _rigidBody = GetComponent<Rigidbody2D>();
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
        if (p.x > _settings.RightBorder || p.x < _settings.LeftBorder || p.y > _settings.Air || p.y < _settings.Ground.Last())
        {
            PlayDead();
            enabled = false;
            Destroy(gameObject);
        }

        if (_instantDeath || GameStats.GameOver)
        {
            HandleDeath();
        }
        else if (_bananaFall)
        {
            HandleFall();
        }
        else if (_goingToStrip && _stripBar != null)
        {
            HandleStrip();
        }
        else
        {
            HandleJump();
            HandleRun();
        }
    }

    private void HandleStrip()
    {
        
        if (InputHelper.RightTap())
        {
            _lastPressRight = Time.time;
            HandleRun();
        }
        else if (Time.time - _lastPressRight <= _settings.LastPressOffset)
            HandleRun();
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, _stripBar.transform.position, _settings.DefaultSpeed);
            _animation.SetFloat("SpeedMultiplier", _defaultSpeedMultiplier);
            if (Math.Abs(Vector3.Distance(_stripBar.transform.position, transform.position)) < 0.1f)
            {
                PlayDead();
                Destroy(gameObject);  
            }
        }     
    }

    private void HandleRun()
    {
        if (Math.Abs(_translateX) < _settings.Step)
        {
            var centerDelta = transform.position.x - _settings.Center;
            _translateX = centerDelta * _settings.ApproachKoef;

            if (_settings.IsRunning)
                _translateX = Math.Min(_translateX, 0f);
            else if (Math.Abs(_settings.Speed - _settings.CurrentSpeed) < 0.01f)
                _translateX = 0f;

            _animation.SetFloat("SpeedMultiplier", _defaultSpeedMultiplier);
        }
        else
        {
            var sign = Math.Sign(_translateX);
            var step = sign * _settings.Step;
            _translateX -= step;
            transform.Translate(-step, 0f, 0f);

            if (_settings.IsRunning)
                _animation.SetFloat("SpeedMultiplier", _maxSpeedMultiplier);
            else
                _animation.SetFloat("SpeedMultiplier", _minSpeedMultiplier);
        }        
    }

    private void HandleJump()
    {
        if (!_canJump || !InputHelper.Up())
            return;
        Shadow.enabled = false;
        _canJump = false;
        var jumpKoef = (float)GameSettings.Rnd.NextDouble() * _settings.RandomJumpMultipier;
        var jump = _settings.MinJumpHeight + jumpKoef;

        _rigidBody.AddForce(transform.up * jump);
        _animation.Play("Jump");
    }

    private void HandleDeath()
    {
        foreach (var sp in GetComponentsInChildren<SpriteRenderer>())
        {
            sp.color = new Color(0, 0, 0, 0);
        }
        Destroy(GetComponent<Collider2D>());
        _rigidBody.constraints |= RigidbodyConstraints2D.FreezePositionY;
        Destroy(_rigidBody);
        SpawnKishki();
        enabled = false;
    }

    private void HandleFall()
    {
        transform.Translate(-_settings.Speed, 0f, 0f);
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
                    if(p.point.y < transform.position.y)
                        return;
                }
                Fall();
                break;
            case "Line":
                Shadow.enabled = true;
                _canJump = true;
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        switch (collider.gameObject.tag)
        {
            case "Strip":
                if (_goingToStrip)
                    return;
                _goingToStrip = true;
                _rigidBody.constraints |= RigidbodyConstraints2D.FreezePositionY;
                _stripBar = collider.gameObject.transform.FindChild("GPoint").gameObject;
                break;
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
        gameObject.layer = _settings.FallenLineLayers[_line];
        Shadow.enabled = false;
        PlayDead();
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        switch (collider.gameObject.tag)
        {
            case "Strip":
                _goingToStrip = false;
                _stripBar = null;
                _rigidBody.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
                break;

        }
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
        Vector2.down,
        Vector2.left,
        Vector2.right
    };

    void SpawnKishki()
    {
        var rndStart = GameSettings.Rnd.Next(0, Kishki.Length);
        var j = 0;
        for (var i = rndStart; i < Kishki.Length + rndStart; i++)
        {
            var ii = i >= Kishki.Length ? i - Kishki.Length : i;
            var k = Instantiate(Kishki[ii], transform.position, Quaternion.identity);
            k.gameObject.GetComponent<KishkiController>().SetSettings(_settings);
            k.AddForce(_forcesOrder[j]);
            j++;
            if (j >= _forcesOrder.Length)
                j = 0;
        }
        var go = Instantiate(Blood, new Vector3(transform.position.x, _settings.Ground[_line]), Quaternion.identity);
        go.GetComponent<EnviromentScroller>().SetSettings(_settings);
        Destroy(go,5f);
    }
}
