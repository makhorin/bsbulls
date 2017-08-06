using System.Linq;
using UnityEngine;

public class KishkiController : MonoBehaviour
{

    private GameSettings _settings;
    public void SetSettings(GameSettings settings)
    {
        _settings = settings;
    }

    void Start () {
        
    }
    
    void Update ()
    {
        transform.Translate(-_settings.Speed * 1.5f, 0f, 0f);
        var p = transform.position;
        if (p.x > _settings.RightBorder || p.x < _settings.LeftBorder || p.y > _settings.Air || p.y < _settings.Ground.Last())
            Destroy(gameObject);
    }
}
