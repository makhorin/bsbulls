using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets
{
    public static class GameStats
    {

        public static float GetRunTime()
        {
            return _gameStarted.HasValue ? (float)DateTime.Now.Subtract(_gameStarted.Value).TotalSeconds : 0f;
        }

        public static float RunTime = 0f;

        public static int Dead;
        public static int CurrentRunners;

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

        static GameStats()
        {
            var prev = PlayerPrefs.GetString("stats","0");
            int.TryParse(prev, out MaxDead);
        }

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
        }
    }

    public class Stats
    {
        public int Score;
        public float Seconds;
    }
}
