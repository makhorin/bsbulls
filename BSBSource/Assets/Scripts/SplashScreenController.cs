//using GooglePlayGames;
using System;
using UnityEngine;
using UnityEngine.UI;

public class SplashScreenController : MonoBehaviour
{
    public Transform Bull;
    public Transform BullPivot;
    public Text[] Text;
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
        else if (!_bullReady)
        {
            _bullTime -= Time.deltaTime;
            Bull.RotateAround(BullPivot.position, new Vector3(0, 0, 1), Time.deltaTime * 240);
            if (_bullTime <= 0f)
                _bullReady = true;
        }
        else
        {
            GameSettings.CanStartGame = true;
            //StartCoroutine("GPAuthenticate");
            enabled = false;
        }
    }

    //void GPAuthenticate()
    //{
    //    PlayGamesPlatform.Activate();
    //    if (!PlayGamesPlatform.Instance.localUser.authenticated &&
    //        Application.internetReachability != NetworkReachability.NotReachable)
    //        PlayGamesPlatform.Instance.localUser.Authenticate(OnAuthenticate);
    //}

    private void OnAuthenticate(bool isAuthenticated)
    {
        try
        {
            if (!isAuthenticated)
            {
                Debug.LogWarning("Auth error");
                return;
            }

            var maxScore = PlayerPrefs.GetInt("maxscore", 0);
            Debug.Log("Authenticated");
#if UNITY_ANDROID
            //PlayGamesPlatform.Instance.ReportScore(maxScore, 
            //                                        "CgkIj9Sz_8UXEAIQAQ", 
            //                                        OnScoreReported);
#endif
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    private void OnScoreReported(bool obj)
    {
      
    }
}
