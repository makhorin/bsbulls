using UnityEngine;

public class EnviromentScroller : MonoBehaviour
{
    public float SpeedMultiplier = 1f;

    void Update ()
    {
        Move();
    }

    protected void Move()
    {
        transform.Translate(-GameController.GameStats.Speed * SpeedMultiplier, 0f, 0f);
    }
}
