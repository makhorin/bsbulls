using UnityEngine;
using System.Collections;

public class PerlinShake : MonoBehaviour {
    
    public float duration = 0.5f;
    public float speed = 1.0f;
    public float magnitude = 0.1f;

    public GameSettings Setting;
    
    // -------------------------------------------------------------------------
    public void PlayShake() {
        
        StopAllCoroutines();
        StartCoroutine("Shake");
    }
    
    // -------------------------------------------------------------------------
    void Update() {
        if (GameController.GameStats.ShakeIt) {
            GameController.GameStats.ShakeIt = false;
            PlayShake();
        }
    }
    
    // -------------------------------------------------------------------------
    IEnumerator Shake() {
        
        float elapsed = 0.0f;
        
        Vector3 originalCamPos = Camera.main.transform.position;
        float randomStart = Random.Range(-1000.0f, 1000.0f);

        var defShCnt = 2;
        for (var i = 0; i < defShCnt; i++)
        {
            while (elapsed < duration)
            {

                elapsed += Time.deltaTime;

                float percentComplete = elapsed / duration;

                // We want to reduce the shake from full power to 0 starting half way through
                float damper = 1.0f - Mathf.Clamp(2.0f * percentComplete - 1.0f, 0.0f, 1.0f);

                // Calculate the noise parameter starting randomly and going as fast as speed allows
                float alpha = randomStart + speed * percentComplete;

                // map noise to [-1, 1]
                float x = Util.Noise.GetNoise(alpha, 0.0f, 0.0f) * 2.0f - 1.0f;
                float y = Util.Noise.GetNoise(0.0f, alpha, 0.0f) * 2.0f - 1.0f;

                x *= magnitude * damper;
                y *= magnitude * damper;

                Camera.main.transform.position = new Vector3(x, y, originalCamPos.z);

                yield return null;
            }

            elapsed = 0.0f;
        }
        
        
        Camera.main.transform.position = originalCamPos;
    }
}
