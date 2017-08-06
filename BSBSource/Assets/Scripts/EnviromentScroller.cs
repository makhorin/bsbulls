using UnityEngine;

public class EnviromentScroller : MonoBehaviour
{
    private GameSettings _settings;
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

    public virtual void SetSettings(GameSettings settings)
    {
        _settings = settings;
    }

    void Update ()
    {
        transform.Translate(-_settings.Speed * SpeedMultiplier, 0f, 0f);
    }
}
