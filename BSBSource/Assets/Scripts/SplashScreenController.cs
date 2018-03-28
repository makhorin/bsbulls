using Assets;
using GooglePlayGames;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SplashScreenController : MonoBehaviour
{
    public DelayedObject[] DelayedObjects;
    private List<DelayedObject> _toCreate;
    void Start ()
    {
        Array.Sort(DelayedObjects, (p, n) => p.DelayS.CompareTo(n.DelayS));
        _toCreate = DelayedObjects.ToList();

#if UNITY_ANDROID
        
#endif
    }
    
    void Update ()
    {
        if (_toCreate.Count > 0)
        {
            var cr = _toCreate[0];
            if (cr.DelayS < Time.time)
            {
                cr.Obj.SetActive(true);
                _toCreate.Remove(cr);
            }
        }
        else
        {
            StartCoroutine("GPAuthenticate");
            enabled = false;
        }
    }

    void GPAuthenticate()
    {
        PlayGamesPlatform.Activate();
        if (!PlayGamesPlatform.Instance.localUser.authenticated &&
            Application.internetReachability != NetworkReachability.NotReachable)
            PlayGamesPlatform.Instance.localUser.Authenticate(OnAuthenticate);
    }

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
            PlayGamesPlatform.Instance.ReportScore(maxScore, 
                                                    "CgkIj9Sz_8UXEAIQAQ", 
                                                    OnScoreReported);
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
