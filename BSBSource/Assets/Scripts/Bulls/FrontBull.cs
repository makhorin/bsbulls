using UnityEngine;

public class FrontBull : MonoBehaviour
{
    void Start()
    {
       transform.Rotate(Vector3.up, 180);
        foreach (var audio in GetComponents<AudioSource>())
        {
            if(audio.clip.name == "BULL")
                audio.Play((ulong)GameSettings.Rnd.Next(0, 2));
        }
    }

    public void SetSettings(int line)
    {
        gameObject.layer = GameSettings.BullLineLayers[line];
        foreach (var sp in GetComponentsInChildren<SpriteRenderer>())
            sp.sortingLayerName = GameSettings.BullSortingLayers[line];
        foreach (var sp in GetComponentsInChildren<ParticleSystem>())
            sp.GetComponent<Renderer>().sortingLayerName = GameSettings.BullSortingLayers[line];
    }

    void Update ()
    {
        transform.Translate(GameSettings.FrontBullSpeedMultiplier * GameController.GameStats.Speed, 0f,0f);
    }

    public void LaserHit()
    {
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Animator>().Play("Explosion");
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
