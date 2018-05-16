using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class ScoreController : MonoBehaviour
    {
        public Text TextField;

        private void Update()
        {
            if (GameController.GameStats == null)
                return;
            TextField.text = GameController.GameStats.Score.ToString();
        }
    }
}
