using UnityEngine;

public class BullsController : MonoBehaviour
{
    public GameObject[] BullPatterns;
    public GirlController Girl;

    public GameSettings Settings;
    private GameObject _frontBull;
    private float _lastFrontBull;

    private int _bullsSinceLastGirl;

    void Start()
    {
        for (var i = 0; i < 3; i++)
        {
            var xPos = Settings.LeftBorder + Settings.BullMinOffset +
                       (float)GameSettings.Rnd.NextDouble() * (Settings.BullMaxOffset - Settings.BullMinOffset);
            var go = Instantiate(BullPatterns[GameSettings.Rnd.Next(0, BullPatterns.Length)], new Vector3(xPos, Settings.Ground[i] + 1f), Quaternion.identity);
            go.AddComponent<BackBull>().SetSettings(Settings, i, xPos);
        }
    }

    void Update()
    {
        GenerateFrontBulls();
        RemoveFarObjects();
    }

    private void RemoveFarObjects()
    {
        if (_frontBull != null && _frontBull.transform.position.x < Settings.LeftBorder - 5f)
        {
            Destroy(_frontBull);
            _frontBull = null;
        }
    }


    private void GenerateFrontBulls()
    {
        if (BullPatterns.Length < 1 || Time.time - _lastFrontBull < Settings.FrontBullCooldownS)
            return;

        var chance = GameSettings.Rnd.NextDouble();

        if (chance < 1f - Settings.FrontBullChance)
            return;
        var line = Settings.GetRandomLine();
        var go = Instantiate(BullPatterns[GameSettings.Rnd.Next(0, BullPatterns.Length)], new Vector3(Settings.RightBorder, Settings.Ground[line] + 1f), Quaternion.identity);
        var fBull = go.AddComponent<FrontBull>();
        fBull.SetSettings(Settings, line);
        _frontBull = go;
        _lastFrontBull = Time.time;

        _bullsSinceLastGirl++;

        if (_bullsSinceLastGirl >= Settings.BullsPerGirl)
        {
            var grl = Instantiate(Girl, new Vector2(Settings.LeftBorder, Settings.GirlY), Quaternion.identity);
            grl.TargetBull = fBull;
            grl.GetComponent<EnviromentScroller>().SetSettings(Settings);
            _bullsSinceLastGirl = 0;
        }
    }
}
