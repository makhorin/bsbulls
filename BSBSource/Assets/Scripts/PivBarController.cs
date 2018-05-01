using UnityEngine;

public class PivBarController : HousesScroller {

    public RunnerController RunnerObj;
    public int MinTaps;
    private int? toGenerate;

    protected override void Start () {
        base.Start();
        TutorController.ShowTutor(KeyCode.LeftArrow, 3);
	}

    public override bool CanBeShown
    {
        get
        {
            return GameController.GameStats.ShowPivBar;
        }
    }
}
