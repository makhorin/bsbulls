using System;
using UnityEngine;

namespace Assets
{
    public class GameStats
    {
        public float GetRunTime()
        {
            return _gameStarted.HasValue ? (float)DateTime.Now.Subtract(_gameStarted.Value).TotalSeconds : 0f;
        }

        public float RunTime = 0f;

        public int Dead;
        public int CurrentRunners;

        public int Score
        {
            get
            {
                return (int)_score;
            }
        }

        float _score;
        internal void IncreaseScore()
        {
            _score += CurrentRunners / 20f;
        }

        float _stamina;
        public float Stamina
        {
            get
            {
                return _stamina;
            }
            set
            {
                if (value < 0)
                    _stamina = 0;
                else if (value > MaxStamina)
                    _stamina = MaxStamina;
                else
                    _stamina = value;
            }
        }

        private int? _maxScore;
        public int MaxScore
        {
            get
            {
                if (!_maxScore.HasValue)
                    _maxScore = PlayerPrefs.GetInt("maxscore", 0);
                return _maxScore.Value;
            }
            set
            {
                if (MaxScore > value)
                    return;
                _maxScore = value;
                PlayerPrefs.SetInt("maxscore", _maxScore.Value);
                PlayerPrefs.Save();
            }
        }
        public bool GameOver;

        private DateTime? _gameStarted;

        public void RegisterRunner(RunnerController runner)
        {
            CurrentRunners++;
            runner.IamDead += ManDied;
        }


        public void ManDied(GameObject obj)
        {
            Dead++;
            CurrentRunners--;

            if (CurrentRunners > 0)
                return;

            PlayerPrefs.SetInt("score", Score);
            PlayerPrefs.SetFloat("runTime", GetRunTime());
            PlayerPrefs.Save();

            GameOver = true;
        }

        public float MaxStamina;

        public void StartGame()
        {
            _gameStarted = DateTime.Now;
            Stamina = MaxStamina;
            _defaultSpeed = GameSettings.DefaultSpeed * 0.5f;
            _currentSpeed = _defaultSpeed;
            SpeedMultipier = 1f;
            _lastWaitingRunners = DateTime.Now;
            _lastPivBar = DateTime.Now;
            _lastStrip = DateTime.Now;
            GameSettings.CanStartGame = false;
        }

        public GameStats()
        {
            _stamina = GameSettings.MaxStamina;
            MaxStamina = GameSettings.MaxStamina;
        }

        private float _currentSpeed;
        private float _defaultSpeed;
        public float Speed
        {
            get { return _currentSpeed * SpeedMultipier; }
        }

        public bool IsRunning;

        public float SpeedMultipier;
        
        private float _deltaTime;
        public void HandleSpeed()
        {
            _deltaTime += (Time.deltaTime - _deltaTime) * 0.1f;

            if (_defaultSpeed < GameSettings.DefaultSpeed)
                _defaultSpeed += Time.deltaTime;

            if (_defaultSpeed > GameSettings.DefaultSpeed)
                _defaultSpeed = GameSettings.DefaultSpeed;

            _currentSpeed = _defaultSpeed * _deltaTime;

            IsRunning = false;

            if (InputHelper.RightDown() || InputHelper.LeftDown())
            {
                if (Stamina > 0f)
                {
                    SpeedMultipier = GameSettings.SpeedUpMultipier;
                    Stamina -= Time.deltaTime;
                    IsRunning = true;
                }
                else
                    SpeedMultipier = 1f;
            }
            else
            {
                SpeedMultipier = 1f;
                Stamina += 0.5f * Time.deltaTime;
            }
        }
        
        public bool ShakeIt { get; set; }

        public bool IsStrip { get; set; }
        public bool IsFrontBull { get; set; }
        public bool IsBackBull { get; set; }

        private DateTime _lastPivBar;
        public bool ShowPivBar
        {
            get
            {
                if (!_gameStarted.HasValue)
                    return false;
                if (DateTime.Now.Subtract(_lastPivBar).TotalSeconds < GameSettings.PivBarCooldown)
                    return false;

                if (GameSettings.Rnd.NextDouble() > GameSettings.PivBarChance)
                    return false;

                _lastPivBar = DateTime.Now;
                return true;
            }
        }

        private DateTime _lastStrip;
        public bool ShowStrip
        {
            get
            {
                if (!_gameStarted.HasValue)
                    return false;
                if (DateTime.Now.Subtract(_lastStrip).TotalSeconds < GameSettings.StripCooldown)
                    return false;

                if (GameSettings.Rnd.NextDouble() > GameSettings.StripChance)
                    return false;

                _lastStrip = DateTime.Now;
                return true;
            }
        }


        private DateTime _lastWaitingRunners;
        public bool ShowWaitingRunners
        {
            get
            {
                if (!_gameStarted.HasValue)
                    return false;

                if (DateTime.Now.Subtract(_lastWaitingRunners).TotalSeconds < GameSettings.WaitingRunnersCooldown)
                    return false;

                if (GameSettings.Rnd.NextDouble() > GameSettings.WaitingRunnersChance)
                    return false;

                _lastWaitingRunners = DateTime.Now;
                return true;
            }
        }
    }
}
