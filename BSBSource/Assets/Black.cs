using System.Collections;
using UnityEngine;

public class Black : MonoBehaviour {

    SpriteRenderer _sprite;
    float _alpha;

	void Start ()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _alpha = 0f;
    }

    public IEnumerator Fade()
    {
        yield return null;
        //var elapsed = _alpha * 2f;
        //while (elapsed < 2f)
        //{
        //    elapsed += Time.deltaTime;
        //    _alpha = elapsed / 2f;
        //    _sprite.color = new Color(1f, 1f, 1f, _alpha);
        //    yield return null;
        //}
    }

    public IEnumerator Show()
    {
        yield return null;
        //var elapsed = _alpha * 2f;
        //while (elapsed < 2f)
        //{
        //    elapsed += Time.deltaTime;
        //    _alpha = elapsed / 2f;
        //    _sprite.color = new Color(1f, 1f, 1f, _alpha);
        //    yield return null;
        //}
    }
}
