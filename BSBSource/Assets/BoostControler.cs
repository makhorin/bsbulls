using UnityEngine;

public class BoostControler : MonoBehaviour
{
    RectTransform _transform;
    float _defaultWidth;

    private void Awake()
    {
        _transform = GetComponent<RectTransform>();
        _defaultWidth = _transform.rect.width;
    }
    void Update ()
    {
        _transform.sizeDelta = new Vector2(GameController.GameStats.Stamina / GameController.GameStats.MaxStamina * _defaultWidth,
            _transform.sizeDelta.y);
	}
}
