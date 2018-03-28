using Assets;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float Width;
    
    public void SetSettings(int line)
    {
        gameObject.layer = GameSettings.ObstacleLineLayers[line];
    }

    void Update()
    {
        transform.Translate(-GameController.GameStats.Speed, 0f, 0f);
    }
}

