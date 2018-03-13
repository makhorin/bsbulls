using Assets;
using System.Linq;
using UnityEngine;

public class KishkiController : MonoBehaviour
{
    float _destroyTime;
    Collider2D _collider;
    void Start () {
        _collider = GetComponent<Collider2D>();
    }
    
    void Update ()
    {
        _destroyTime += Time.deltaTime;
        if (_destroyTime > 2f)
            _collider.enabled = false;
        transform.Translate(-GameStats.Speed * 1.5f, 0f, 0f);
        var p = transform.position;
        if (p.x > GameSettings.RightBorder || p.x < GameSettings.LeftBorder || p.y > GameSettings.Air || p.y < GameSettings.Ground.Last())
            Destroy(gameObject);
    }
}
