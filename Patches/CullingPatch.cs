using HarmonyLib;
using Koenigz.PerfectCulling;
using NikoArchipelago.Archipelago;

namespace NikoArchipelago.Patches;

public class CullingPatch
{
    [HarmonyPatch(typeof(PerfectCullingUtil), "ToggleRenderer")]
    public static class NoMoreInvisibleTrees
    {
        static bool Prefix(UnityEngine.Renderer __0, bool __1, bool __2, UnityEngine.Rendering.ShadowCastingMode __3)
        {
            if (__0 != null) return true;
            // if (Plugin.DebugMode && scrTransitionManager.instance.state == scrTransitionManager.States.idle)
            //     Plugin.BepinLogger.LogError("No Renderer found! Preventing culling error...");
            return false;
        }
    }
}