using KinematicCharacterController.Core;
using NikoArchipelago.Archipelago;

namespace NikoArchipelago.Stuff;

public static class MovementSpeed
{
    public static bool IsSpeedOn = true;
    private const float DiveSpeed = 16f;
    private const float JumpSpeed = 13f;
    private const float MaxAirMoveSpeed = 8f;
    private const float MaxStableMoveSpeed = 8f;
    private const float MaxWaterMoveSpeed = 5f;
    private const float DiveCancelHopSpeed = 11f;
    public static void MovementSpeedMultiplier()
    {
        if (!ArchipelagoClient.IsValidScene()) return;

        float boost = ArchipelagoClient.SpeedBoostAmount;
        const float speedFactor = 0.4f;
        const float jumpFactor = 0.08f;
        const float diveFactor = 0.25f;

        var speedMultiplier = 1f + boost * speedFactor;
        var jumpMultiplier = 1f + boost * jumpFactor;
        var diveMultiplier = 1f + boost * diveFactor;

        MyCharacterController.instance.DiveSpeed = DiveSpeed * diveMultiplier;
        MyCharacterController.instance.MaxAirMoveSpeed = MaxAirMoveSpeed * speedMultiplier;
        MyCharacterController.instance.JumpSpeed = JumpSpeed * jumpMultiplier;
        MyCharacterController.instance.DiveCancelHopSpeed = DiveCancelHopSpeed * jumpMultiplier;
        MyCharacterController.instance.MaxStableMoveSpeed = MaxStableMoveSpeed * speedMultiplier;
        MyCharacterController.instance.MaxWaterMoveSpeed = MaxWaterMoveSpeed * speedMultiplier;

        Plugin.BepinLogger.LogInfo($"Speed Boost Applied: {speedMultiplier}x, Jump Boost: {jumpMultiplier}x, Dive Boost: {diveMultiplier}x, Boost Amount: {boost}");
    }
}