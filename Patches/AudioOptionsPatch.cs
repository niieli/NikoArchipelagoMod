using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using HarmonyLib;
using SimpleJSON;
using UnityEngine;

namespace NikoArchipelago.Patches;

public class AudioOptionsPatch
{
    [HarmonyPatch(typeof(DecimalOption), nameof(DecimalOption.Serialize))]
    public static class DecimalOptionPostfixSerializePatch
    {
        private static void Postfix(DecimalOption __instance, ref JSONNode __result)
        {
            var invariant = __instance.Value.ToString("F2");

            //Locale uses `,`, so wrap it up in ' "" ' . I am so done with this...
            if (__instance.Value.ToString().Contains(","))
            {
                __result = new JSONString(invariant);
            }
            else
            {
                __result = new JSONNumber(__instance.Value);
            }
        }
    }
}