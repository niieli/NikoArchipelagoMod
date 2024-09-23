using BepInEx;
using HarmonyLib;
using Mono.Cecil;
using UnityEngine;

namespace NikoArchipelago.Patches;

public class NotificationPatch
{
    [HarmonyPatch(typeof(scrNotificationDisplayer), "AnimatePopup")]
    public static class NotificationTextPatch
    {
        [HarmonyPostfix]
        public static void BypassLocalization(scrNotificationDisplayer __instance)
        {
            // Check if there are notifications in the queue
            if (__instance.notificationQueue.Count > 0)
            {
                // Get the latest notification from the queue
                Notification currentNotification = __instance.notificationQueue[__instance.notificationQueue.Count - 1];

                if (currentNotification != null && !string.IsNullOrEmpty(currentNotification.key))
                {
                    // Try to get the localized value from LocalizationManager
                    string localizedText = LocalizationManager.instance.GetLocalizedValue(currentNotification.key);

                    // Check if the localized text is valid (not null or empty)
                    if (localizedText != "!!What's my line?!!")
                    {
                        // Use localized text if it exists
                        __instance.textMesh.text = localizedText;
                    }
                    else
                    {
                        // Bypass localization and use the raw key as text
                        __instance.textMesh.text = currentNotification.key;
                    }
                }
            }
        }
    }
}