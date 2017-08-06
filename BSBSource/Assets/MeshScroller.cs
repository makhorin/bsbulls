using UnityEngine;

public class MeshScroller : MonoBehaviour
{
    private Renderer _renderer;
    public float Multiplier;
    public string Layer;

    public GameSettings Setting;

    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _renderer.sortingLayerName = Layer;
    }
    void Update()
    {
        var speed = Setting.Speed;
        _renderer.material.mainTextureOffset = _renderer.material.mainTextureOffset + new Vector2(-speed * Multiplier, 0);
    }
}
