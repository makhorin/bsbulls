using System.Collections;
using UnityEngine;

public class Black : MonoBehaviour {

    SpriteRenderer _sprite;
    float _alpha;
    float _timeToFinsh = 1f;
    bool _fading;

	void Start ()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _alpha = 0f;
    }

    public IEnumerator Fade()
    {
        _fading = true;
        while (_alpha < 1f && _fading)
        {
            _alpha += Time.deltaTime / _timeToFinsh;
            _sprite.color = new Color(1f, 1f, 1f, _alpha);
            yield return null;
        }
    }

    public IEnumerator Show()
    {
        _fading = false;
        while (_alpha > 0f && !_fading)
        {
            _alpha -= Time.deltaTime / _timeToFinsh;
            _sprite.color = new Color(1f, 1f, 1f, _alpha);
            yield return null;
        }
    }
}
