using UnityEngine;

public class PivBarController : HousesScroller {

    public RunnerController RunnerObj;

    private int? toGenerate;

    public override bool CanBeShown
    {
        get
        {
            return GameController.GameStats.ShowPivBar;
        }
    }
}
