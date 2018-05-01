using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class PivbarSpawner : MonoBehaviour
    {
        public RunnerController RunnerObj;

        private int _minTaps;
        private bool _spawning;
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

            if (_minTaps > 0)
            {
                if (InputHelper.LeftTap() || InputHelper.RightTap())
                    _minTaps--;
                return;
            }

            _spawning = true;
            StartCoroutine("SpawnCoroutine");
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
