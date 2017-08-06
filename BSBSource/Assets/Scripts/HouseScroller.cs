public class HouseScroller : EnviromentScroller
{
    public override void SetSettings(GameSettings settings)
    {
        base.SetSettings(settings);
        foreach (var cntrl in GetComponentsInChildren<WaitingRunnerController>())
        {
            cntrl.SetSettings(settings);
        }
    }
}
