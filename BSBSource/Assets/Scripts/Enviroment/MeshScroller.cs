using UnityEngine;

public class MeshScroller : MonoBehaviour
{
    private Renderer _renderer;
    public float Multiplier;
    public string Layer;
    public int Order;

    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _renderer.sortingLayerName = Layer;
        _renderer.sortingOrder = Order;
    }
    void Update()
    {
        var speed = GameController.GameStats.Speed;
        _renderer.material.mainTextureOffset = new Vector2(_renderer.material.mainTextureOffset.x - speed * Multiplier, 0);
    }
}
