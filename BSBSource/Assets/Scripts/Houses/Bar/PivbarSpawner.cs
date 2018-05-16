using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class PivbarSpawner : MonoBehaviour
    {
        public RunnerController RunnerObj;
        public GameObject Door;

        private int _minTaps;
        private bool _spawning;
        private bool _tutorShown;

        private void Start()
        {
            _minTaps = GameSettings.MinTapsForPivbar;
        }

        void Update()
        {
            if (GameController.GameStats.GameOver || _spawning)
                return;

            if (transform.position.x <= GameSettings.LeftBorder)
                return;

            if (transform.position.x >= GameSettings.RightBorder)
                return;

            if (!_tutorShown)
                TutorController.ShowTutor(KeyCode.LeftArrow, _minTaps);

            _tutorShown = true;
            if (_minTaps > 0)
            {
                if (InputHelper.LeftTap() || InputHelper.RightTap())
                    _minTaps--;
                return;
            }
            Door.SetActive(true);
            _spawning = true;
            StartCoroutine(SpawnCoroutine());
        }

        IEnumerator SpawnCoroutine()
        {
            for (int i = 0; i < GameSettings.PivBarSpawn; i++)
            {
                var go = Instantiate(RunnerObj, transform.position, Quaternion.identity);
                go.SetSettings(GameSettings.GetRandomLine(), false);
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
