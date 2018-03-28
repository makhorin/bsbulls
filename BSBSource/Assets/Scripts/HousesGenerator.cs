using Assets;
using UnityEngine;

public class HousesGenerator : EnviromentGenerator
{

    public BldWithChance[] SpecialBuildings;
    private float _lastSpecBld;

    protected override void Start()
    {
        _lastSpecBld = Time.time;
        base.Start();
    }

    protected override EnviromentScroller SelectBld()
    {
        EnviromentScroller newEnv = null;

        if (Time.time - _lastSpecBld > GameSettings.SpecBldCooldownS)
        {
            var specBld = SpecialBuildings[GameSettings.Rnd.Next(0, SpecialBuildings.Length)];
            if (GameSettings.Rnd.NextDouble() > 1f - specBld.Chance && (
                    !specBld.IsStrip || (!GameController.GameStats.IsFrontBull && !GameController.GameStats.IsBackBull)))
            {
                newEnv = specBld.Bld;
                _lastSpecBld = Time.time;
            }
        }

        if (newEnv == null)
            newEnv = base.SelectBld();

        return newEnv;
    }
}
