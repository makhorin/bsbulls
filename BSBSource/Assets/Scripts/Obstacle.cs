using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private GameSettings _settings;
    public float Width;
    

    public void SetSettings(GameSettings settings, int line)
    {
        _settings = settings;
        gameObject.layer = _settings.ObstacleLineLayers[line];
    }

    void Update()
    {
        transform.Translate(-_settings.Speed, 0f, 0f);
    }
}

