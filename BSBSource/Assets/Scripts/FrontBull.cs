using UnityEngine;

public class FrontBull : MonoBehaviour
{
    private GameSettings _settings;

    void Start()
    {
       transform.Rotate(Vector3.up, 180);
        foreach (var audio in GetComponents<AudioSource>())
        {
            if(audio.clip.name == "BULL")
                audio.Play((ulong)GameSettings.Rnd.Next(0, 2));
        }
    }

    public void SetSettings(GameSettings settings, int line)
    {
        _settings = settings;
        gameObject.layer = _settings.BullLineLayers[line];
        foreach (var sp in GetComponentsInChildren<SpriteRenderer>())
            sp.sortingLayerName = _settings.BullSortingLayers[line];
        foreach (var sp in GetComponentsInChildren<ParticleSystem>())
            sp.GetComponent<Renderer>().sortingLayerName = _settings.BullSortingLayers[line];
    }

    void Update ()
    {
        transform.Translate(_settings.FrontBullSpeedMultiplier * Mathf.Max(_settings.Speed,_settings.DefaultSpeed), 0f,0f);
    }

    public void LaserHit()
    {
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponentInChildren<Animator>().Play("Bull");
        foreach (var ps in GetComponentsInChildren<ParticleSystem>())
        {
            if (ps.gameObject.tag == "BloodParticles")
                ps.Play();
            else
                ps.Stop();
        }
        foreach (var audio in GetComponents<AudioSource>())
        {
            if (audio.clip.name == "Explosion")
                audio.Play((ulong)GameSettings.Rnd.Next(0, 2));
        }
        Destroy(gameObject, 1f);
    }
}
