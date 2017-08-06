using System.Runtime.InteropServices;
using UnityEngine;

public class ToPointScroller : MonoBehaviour
{
    public Vector2 Point;
    public float Speed;
    private Animator _animator;
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    void Update ()
    {
        transform.position = Vector2.MoveTowards(transform.position, Point, Speed * Time.deltaTime);
        if (transform.position == (Vector3) Point)
        {
            enabled = false;
        }
        else
        {
            if(_animator != null)
                _animator.Play("LogoGirl");
        }
    }
}
