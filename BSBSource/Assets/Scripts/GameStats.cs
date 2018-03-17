using System;
using UnityEngine;

namespace Assets
{
    public class GameStats : MonoBehaviour
    {

        public static float GetRunTime()
        {
            return _gameStarted.HasValue ? (float)DateTime.Now.Subtract(_gameStarted.Value).TotalSeconds : 0f;
        }

        public static float RunTime = 0f;

        public static int Dead;
        public static int CurrentRunners;

        public static int Score
        {
            get
            {
                return (int)_score;
            }
        }
        static float _score;
        internal static void IncreaseScore()
        {
            _score += CurrentRunners / 20f;
        }

        static float _stamina;
        public static float Stamina
        {
            get
            {
                return _stamina;
            }
            set
            {
                if (value < 0)
                    _stamina = 0;
                else if (value > GameSettings.MaxStamina)
                    _stamina = GameSettings.MaxStamina;
                else
                    _stamina = value;
            }
        }


        private static bool _canStart;
        public static bool CanStartGame
        {
            get { return _canStart; }
            set
            {
                _canStart = value;
            }
        }

        public static int MaxDead;
        public static bool GameOver;

        private static DateTime? _gameStarted;

        public static void RegisterRunner(RunnerController runner)
        {
            CurrentRunners++;
            runner.IamDead += ManDied;
        }


        public static void ManDied(GameObject obj)
        {
            Dead++;
            CurrentRunners--;

            if (CurrentRunners > 0)
                return;
            var runTime = GetRunTime();
            RunTime = runTime;
            if (MaxDead < Dead)
            {
                PlayerPrefs.SetString("stats", Dead.ToString());
                PlayerPrefs.Save();
                MaxDead = Dead;
            }

            GameOver = true;
        }

        public static void StartGame()
        {
            _gameStarted = DateTime.Now;
        }


        public static void Reset()
        {
            _gameStarted = null;
            RunTime = 0f;
            Dead = 0;
            _score = 0f;
            _stamina = GameSettings.MaxStamina;
        }

        public static float Speed
        {
            get { return CurrentSpeed * _speedMultipier; }
        }

        public static bool IsRunning
        {
            get { return _isRunning; }
        }

        private static float _speedMultipier;
        private float _deltaTime;
        static bool _isRunning;
        void Update()
        {
            _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;

            CurrentSpeed = GameSettings.DefaultSpeed * _deltaTime;
            _isRunning = false;

            if (InputHelper.RightDown() || InputHelper.LeftDown())
            {
                if (Stamina > 0f)
                {
                    _speedMultipier = GameSettings.SpeedUpMultipier;
                    Stamina -= Time.deltaTime;
                    _isRunning = true;
                }
                else
                    _speedMultipier = 1f;
            }
            else
            {
                _speedMultipier = 1f;
                Stamina += 0.5f * Time.deltaTime;
            }
            HandleSlowMotion();
        }

        static bool _isSlowMotion;
        public static void ToggleSlowMotion()
        {
            _isSlowMotion = !_isSlowMotion;
        }

        void HandleSlowMotion()
        {
            if (!_isSlowMotion)
                return;

            CurrentSpeed /= 3f;      
        }

        public static float CurrentSpeed;
        public static bool ShakeIt { get; set; }

        public static bool IsStrip { get; set; }
        public static bool IsFrontBull { get; set; }
        public static bool IsBackBull { get; set; }
    }
}
