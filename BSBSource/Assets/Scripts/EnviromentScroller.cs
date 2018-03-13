using Assets;
using UnityEngine;

public class EnviromentScroller : MonoBehaviour
{
    public SpriteRenderer Renderer;
    public float SpeedMultiplier = 1f;

    public float Width
    {
        get { return Renderer.sprite.textureRect.width / Renderer.sprite.pixelsPerUnit; }
    }

    public float Height
    {
        get { return Renderer.sprite.textureRect.height / Renderer.sprite.pixelsPerUnit; }
    }

    void Update ()
    {
        transform.Translate(-GameStats.Speed * SpeedMultiplier, 0f, 0f);
    }
}
