using System.Collections.Generic;
using HarmonyLib;

namespace NikoArchipelago.Patches;

public class LocalizationPatch
{
    [HarmonyPatch(typeof(LocalizationManager), "GetLocalizedValue")]
    public static class ConversationTextPatch
    {
        static bool Prefix(LocalizationManager __instance, string key, ref string __result)
        {
            var localizedTextField = AccessTools.Field(typeof(LocalizationManager), "localizedText");
            Dictionary<string, string> _localizedText = (Dictionary<string, string>)localizedTextField.GetValue(LocalizationManager.instance);
            var missingTextStringField = AccessTools.Field(typeof(LocalizationManager), "missingTextString");
            string _missingTextString = (string)missingTextStringField.GetValue(LocalizationManager.instance);
            
            if (_localizedText.ContainsKey(key))
            {
                __result = _localizedText[key];
            }
            else if (TrapManager.TrapConversations.TryGetValue(key, out var trapText))
            {
                __result = trapText;
            }
            else if (FishingPatch.FischerConversation.TryGetValue(key, out var fishText))
            {
                __result = fishText;
            }
            else
            {
                __result = _missingTextString;
            }

            return false; // Skip original method
        }
    }
}