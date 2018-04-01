using System.Linq;
using UnityEngine;

public class KishkiController : EnviromentScroller
{
    public ParticleSystem Blood;
    public ParticleSystem Splash;
    public Rigidbody Rigidbody;
    public GameObject Splat;
    private bool _fallen;
    void Update ()
    {
        Move();
        var p = transform.position;
        if (p.x > GameSettings.RightBorder || p.x < GameSettings.LeftBorder || p.y > GameSettings.Air || p.y < GameSettings.Ground.Last())
            Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (_fallen)
            return;
        switch (collision.gameObject.tag)
        {
            case "Line":
                if (collision.gameObject.transform.position.y > transform.position.y)
                    return;
                Blood.Stop();
                Splash.Play();
                Renderer.enabled = false;
                Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
                transform.rotation = Quaternion.identity;
                Splat.SetActive(true);
                _fallen = true;
                break;
        }
    }
}
