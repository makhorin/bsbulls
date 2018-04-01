public class GameSettings
{
    public static System.Random Rnd = new System.Random();
    public static float[] Ground = new [] {-1f, -4f, -7f};
    public static float Air = 5f;
    public static float Center = 0f;
    public static float RightBorder = 10.5f;
    public static float LeftBorder = -10.5f;

    public static float MinJumpHeight = 370f;
    public static float RandomJumpMultipier = 100f;
    public static float DefaultSpeed = 0.1f;
    public static float DefaultTutorShowTime = 1.5f;
    public static float MaxSpeed = 0.1f;
    public static float SecondsToReachMaxSpeed = 10f;
    public static float SpeedUpMultipier = 2f;
    public static float SpeedDownMultiplier = 0.5f;
    public static float GroundObstaclesChance = 0.1f;
    public static int MaxRunners = 2;
    public static float RunnersRadius = 5f;
    public static float ApproachKoef = 0.8f;
    public static float Step = 0.01f;
    public static float BullStep = 0.1f;

    public static float FrontBullChance = 0.1f;

    public static float FrontBullSpeedMultiplier = 1.5f;
    public static float BackBullDashSpeedMultiplier = 1.5f;
    public static float BackBullApproachKoef = 1.2f;
    public static float BackBullDashChance = 0.15f;

    public static float BullMinOffset = 2.5f;
    public static float BullMaxOffset = 3.5f;

    public static float ObstacleCooldownS = 3f;
    public static float DashCooldownS = 7f;
    public static float FrontBullCooldownS = 10f;

    public static float SpecBldCooldownS = 15f;

    public static float LastPressOffset = 0.05f;

    public static float DashDurationS = 2f;

    public static float GirlY = 2f;

    public static int BullsPerGirl = 5;

    public static float MaxStamina = 1f;

    public static int GetRandomLine()
    {
        return Rnd.Next(0, 3);
    }

    public static int[] LineLayers = new[] {8, 9, 10};
    public static int[] FallenLineLayers = new[] { 13, 14, 15 };
    public static int[] BullLineLayers = new[] { 16, 17, 18 };
    public static int[] ObstacleLineLayers = new[] { 19, 20, 21 };
    public static string[] BullSortingLayers = new[] { "Bull", "Bull2", "Bull3" };
    public static string[] RunnersSortingLayers = new[] { "Player", "Player2", "Player3" };
    public static int[] KishkiLayers = new[] { 25, 28, 29 };
    public static string[] LayerNames = new[] { "Line1", "Line2", "Line3" };
}
