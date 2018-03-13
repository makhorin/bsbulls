using Assets;
using UnityEngine;

public class MeshScroller : MonoBehaviour
{
    private Renderer _renderer;
    public float Multiplier;
    public string Layer;

    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _renderer.sortingLayerName = Layer;
    }
    void Update()
    {
        var speed = GameStats.Speed;
        _renderer.material.mainTextureOffset = _renderer.material.mainTextureOffset + new Vector2(-speed * Multiplier, 0);
    }
}
