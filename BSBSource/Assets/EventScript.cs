using UnityEngine;

public class EventScript : MonoBehaviour {

    public ParticleSystem[] Particles;

    public void PlayParticle()
    {
        foreach(var p in Particles)
            p.Play();
    }
}
