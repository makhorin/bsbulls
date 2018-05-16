using UnityEngine;

public class ColorRandomizer : MonoBehaviour
{

    public Color[] Colors;

    void Start ()
    {
        if (Colors.Length < 1)
            return;
        var sp = GetComponent<SpriteRenderer>();
        sp.color = Colors[GameSettings.Rnd.Next(0, Colors.Length)];
    }
}
