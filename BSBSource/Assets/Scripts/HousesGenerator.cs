using UnityEngine;

public class HousesGenerator : EnviromentGenerator
{

    public BldWithChance[] SpecialBuildings;
    private float _lastSpecBld;

    protected override EnviromentScroller SelectBld()
    {
        EnviromentScroller newEnv = null;

        if (Time.time - _lastSpecBld > GameSettings.SpecBldCooldownS)
        {
            var specBld = SpecialBuildings[GameSettings.Rnd.Next(0, SpecialBuildings.Length)];
            if (GameSettings.Rnd.NextDouble() > 1f - specBld.Chance)
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
