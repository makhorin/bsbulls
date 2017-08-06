
using UnityEngine;

public class Manhole : Obstacle
{
    public SpriteRenderer[] Splats;
    private bool _splatShown;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (_splatShown)
            return;

        switch (collider.gameObject.tag)
        {
            case "Runner":
                foreach (var s in Splats)
                {
                    s.enabled = true;
                }
                _splatShown = true;
                break;
        }
    }
}

