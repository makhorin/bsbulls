using UnityEngine;

public class WaitingRunnerController : MonoBehaviour
{
    public bool ShouldJump;
    public RunnerController RunnerObj;
    private int? toGenerate;

    void Start ()
    {
        GetComponent<Animator>().SetFloat("AnimOffset", (float)GameSettings.Rnd.NextDouble());
    }
    
    void Update ()
    {
        if (GameController.GameStats.GameOver)
            return;

        if (transform.position.x <= GameSettings.LeftBorder)
            return;

        if (!toGenerate.HasValue)
            return;

        enabled = false;

        var go = Instantiate(RunnerObj, transform.position, Quaternion.identity);
        go.SetSettings(GameSettings.GetRandomLine(), true);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (toGenerate.HasValue || collider.gameObject.tag != "Runner")
            return;
        toGenerate = 1;
    }
}
