using Assets;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static System.Random Rnd = new System.Random();
    public float[] Ground = new [] {-1.5f, 0f, 1.5f};
    public float Air = 2f;
    public float Center = 0f;
    public float RightBorder = 10f;
    public float LeftBorder = -10f;

    public float MinJumpHeight = 200f;
    public float RandomJumpMultipier = 100f;
    public float DefaultSpeed = 0.1f;
    public float DefaultTutorShowTime = 5f;
    public float CurrentSpeed;
    public float MaxSpeed = 0.1f;
    public float SecondsToReachMaxSpeed = 120f;
    public float SpeedUpMultipier = 2f;
    public float SpeedDownMultiplier = 0.5f;
    public float GroundObstaclesChance = 1f;
    public int MaxRunners = 30;
    public float RunnersRadius = 5f;
    public float ApproachKoef = 0.8f;
    public float Step = 0.01f;
    public float BullStep = 0.1f;

    public float FrontBullChance = 0.2f;

    public float FrontBullSpeedMultiplier = 1.5f;
    public float BackBullDashSpeedMultiplier = 1.3f;
    public float BackBullApproachKoef = 0.5f;
    public float BackBullDashChance = 0f;

    public float BullMinOffset = 0.5f;
    public float BullMaxOffset = 1f;

    public float ObstacleCooldownS = 3f;
    public float DashCooldownS = 3f;
    public float FrontBullCooldownS = 3f;

    public float SpecBldCooldownS = 3f;

    public float LastPressOffset = 0.1f;

    public float Speed
    {
        get { return CurrentSpeed * _speedMultipier; }
    }

    public bool IsRunning
    {
        get { return Speed > CurrentSpeed; }
    }

    public static bool ShakeIt { get; set; }

    public float GirlY = 4.3f;

    public int BullsPerGirl = 3;

    private static float _speedMultipier;

    void Start()
    {
        CurrentSpeed = DefaultSpeed;
        GameStarter.Settings = this;
    }


    void Update()
    {
        if (InputHelper.RightDown())
            _speedMultipier = SpeedUpMultipier;
        else if (InputHelper.LeftDown())
            _speedMultipier = SpeedDownMultiplier;
        else
            _speedMultipier = 1f;
    }

    public int GetRandomLine()
    {
        return Rnd.Next(0, 3);
    }

    public int[] LineLayers = new[] {8, 9, 10};
    public int[] FallenLineLayers = new[] { 13, 14, 15 };
    public int[] BullLineLayers = new[] { 16, 17, 18 };
    public int[] ObstacleLineLayers = new[] { 19, 20, 21 };
    public string[] BullSortingLayers = new[] { "Bull", "Bull2", "Bull3" };
    public string[] RunnersSortingLayers = new[] { "Player", "Player2", "Player3" };

    public string[] LayerNames = new[] { "Line1", "Line2", "Line3" };
}
