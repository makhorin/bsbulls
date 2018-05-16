using UnityEngine;

public class UpDownScroller : MonoBehaviour {

    public float UpDownSpeed = 0f;
    public float UpDownDelta = 0f;
    private float _startY;

    private int _speedSign = 1;

    void Start()
    {
        _startY = transform.position.y;
    }

    void Update ()
    {
        transform.Translate(0f, _speedSign * UpDownSpeed, 0f);
        var delta = Mathf.Abs(transform.position.y - _startY);
        if (delta > UpDownDelta)
            _speedSign = -_speedSign;
    }
}
