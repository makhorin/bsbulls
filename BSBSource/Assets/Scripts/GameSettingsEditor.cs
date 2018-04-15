using UnityEngine;

public class GameSettingsEditor : MonoBehaviour
{
    public bool Override;
    public float MinJumpHeight = 370f;
    public float RandomJumpMultipier = 100f;
    public float DefaultSpeed = 0.1f;
    public float DefaultTutorShowTime = 1.5f;
    public float SpeedUpMultipier = 2f;
    public float SpeedDownMultiplier = 0.5f;
    public float GroundObstaclesChance = 0.1f;
    public int MaxRunners = 2;
    public float RunnersRadius = 5f;
    public float ApproachKoef = 0.8f;
    public float Step = 0.01f;
    public float BullStep = 0.1f;

    public float FrontBullChance = 0.1f;

    public float FrontBullSpeedMultiplier = 1.5f;
    public float BackBullDashSpeedMultiplier = 1.5f;
    public float BackBullApproachKoef = 1.2f;
    public float BackBullDashChance = 0.15f;

    public float BullMinOffset = 1.5f;
    public float BullMaxOffset = 2.5f;

    public float ObstacleCooldownS = 3f;
    public float DashCooldownS = 7f;
    public float FrontBullCooldownS = 10f;

    public float SpecBldCooldownS = 15f;

    public float LastPressOffset = 0.05f;

    public int BullsPerGirl = 5;

    public float DashDurationS = 2f;

    public float MaxStamina = 1f;

    void Start()
    {
        if (!Override)
            return;

        GameSettings.MinJumpHeight = MinJumpHeight;
        GameSettings.RandomJumpMultipier = RandomJumpMultipier;
        GameSettings.DefaultSpeed = DefaultSpeed;
        GameSettings.DefaultTutorShowTime = DefaultTutorShowTime;
        GameSettings.SpeedUpMultipier = SpeedUpMultipier;
        GameSettings.SpeedDownMultiplier = SpeedDownMultiplier;
        GameSettings.GroundObstaclesChance = GroundObstaclesChance;
        GameSettings.MaxRunners = MaxRunners;
        GameSettings.RunnersRadius = RunnersRadius;
        GameSettings.ApproachKoef = ApproachKoef;
        GameSettings.Step = Step;
        GameSettings.BullStep = BullStep;

        GameSettings.FrontBullChance = FrontBullChance;

        GameSettings.FrontBullSpeedMultiplier = FrontBullSpeedMultiplier;
        GameSettings.BackBullDashSpeedMultiplier = BackBullDashSpeedMultiplier;
        GameSettings.BackBullApproachKoef = BackBullApproachKoef;
        GameSettings.BackBullDashChance = BackBullDashChance;

        GameSettings.BullMinOffset = BullMinOffset;
        GameSettings.BullMaxOffset = BullMaxOffset;

        GameSettings.ObstacleCooldownS = ObstacleCooldownS;
        GameSettings.DashCooldownS = DashCooldownS;
        GameSettings.FrontBullCooldownS = FrontBullCooldownS;

        GameSettings.SpecBldCooldownS = SpecBldCooldownS;

        GameSettings.LastPressOffset = LastPressOffset;

        GameSettings.BullsPerGirl = BullsPerGirl;

        GameSettings.DashDurationS = DashDurationS;

        GameSettings.MaxStamina = MaxStamina;
    }
}
