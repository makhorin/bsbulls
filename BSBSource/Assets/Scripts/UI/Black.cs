using System.Collections;
using UnityEngine;

public class Black : MonoBehaviour {

    SpriteRenderer _sprite;
    float _alpha;
    float _timeToFinsh = 1f;

	void Start ()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _alpha = 0f;
    }

    public IEnumerator Fade()
    {
        while (_alpha < 1f)
        {
            _alpha += Time.deltaTime / _timeToFinsh;
            _sprite.color = new Color(1f, 1f, 1f, _alpha);
            yield return null;
        }
    }

    public IEnumerator Show()
    {
        while (_alpha > 0f)
        {
            _alpha -= Time.deltaTime / _timeToFinsh;
            _sprite.color = new Color(1f, 1f, 1f, _alpha);
            yield return null;
        }
    }
}
