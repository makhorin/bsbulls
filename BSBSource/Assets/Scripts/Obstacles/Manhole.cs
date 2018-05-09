
using UnityEngine;

public class Manhole : Obstacle
{
    public ParticleSystem Fontain;
    public SpriteRenderer Splat;
    private bool _splatShown;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (_splatShown)
            return;

        switch (collider.gameObject.tag)
        {
            case "Runner":
                Splat.gameObject.SetActive(true);
                Fontain.Play();
                _splatShown = true;
                break;
        }
    }
}

