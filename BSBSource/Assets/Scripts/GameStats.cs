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

        private int _money;
        public int Money
        {
            get
            {
                return _money;
            }
            set
            {
                if (_money == value)
                    return;
                _money = value;
                PlayerPrefs.SetInt("money", _money);
                PlayerPrefs.Save();
            }
        }

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
            _currentSpeed = GameSettings.DefaultSpeed;
        }

        public GameStats()
        {
            _stamina = GameSettings.MaxStamina;
            _money = PlayerPrefs.GetInt("money", 0);
            MaxStamina = GameSettings.MaxStamina;
        }

        private float _currentSpeed;
        public float Speed
        {
            get { return _currentSpeed * SpeedMultipier; }
        }

        public bool IsRunning;

        public float SpeedMultipier;
        
        bool _isSlowMotion;
        public void ToggleSlowMotion()
        {
            _isSlowMotion = !_isSlowMotion;
        }

        private float _deltaTime;
        public void HandleSpeed()
        {
            _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;

            _currentSpeed = GameSettings.DefaultSpeed * _deltaTime;

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
            HandleSlowMotion();
        }

        void HandleSlowMotion()
        {
            if (!_isSlowMotion)
                return;

            _currentSpeed /= 3f;      
        }

        
        public bool ShakeIt { get; set; }

        public bool IsStrip { get; set; }
        public bool IsFrontBull { get; set; }
        public bool IsBackBull { get; set; }
    }
}
