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
    }
}
