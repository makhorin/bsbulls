using UnityEngine;

public class EnviromentScroller : MonoBehaviour
{
    public SpriteRenderer Renderer;
    public float SpeedMultiplier = 1f;

    public float Width
    {
        get { return Renderer.sprite.textureRect.width * Renderer.transform.localScale.x / Renderer.sprite.pixelsPerUnit; }
    }

    public float Height
    {
        get { return Renderer.sprite.textureRect.height * Renderer.transform.localScale.y / Renderer.sprite.pixelsPerUnit; }
    }

    void Update ()
    {
        Move();
    }

    protected void Move()
    {
        transform.Translate(-GameController.GameStats.Speed * SpeedMultiplier, 0f, 0f);
    }
}
