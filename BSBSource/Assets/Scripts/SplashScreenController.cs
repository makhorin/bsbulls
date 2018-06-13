using System;
using UnityEngine;
using UnityEngine.UI;

public class SplashScreenController : MonoBehaviour
{
    public Transform Bull;
    public Transform BullPivot;
    public Text[] Text;
    public GameObject TapToStart;
    bool _textReady;
    bool _bullReady;
    float _textSeconds = 1f;
    float _textAlpha = 0f;
    float _bullTime = 0.2f;
    void Update ()
    {
        if (!_textReady)
        {
            _textAlpha += Time.deltaTime / _textSeconds;
            if (_textAlpha > 1f)
                _textReady = true;
            else
                foreach (var txt in Text)
                    txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, _textAlpha);
        }
        else
        {
            TapToStart.SetActive(true);
            GameSettings.CanStartGame = true;
            enabled = false;
        }
    }
}
