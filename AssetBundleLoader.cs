using System.IO;
using System.Reflection;
using BepInEx;
using UnityEngine;

namespace NikoArchipelago;

public class AssetBundleLoader
{
    public static AssetBundle LoadEmbeddedAssetBundle(string bundleName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        string resourceName = $"NikoArchipelago.{bundleName}";
        using (Stream resourceStream = assembly.GetManifestResourceStream(resourceName))
        {
            if (resourceStream == null)
            {
                Debug.LogError($"AssetBundle {bundleName} not found in embedded resources.");
                return null;
            }
            byte[] bundleData = new byte[resourceStream.Length];
            resourceStream.Read(bundleData, 0, bundleData.Length);
            AssetBundle assetBundle = AssetBundle.LoadFromMemory(bundleData);
            if (assetBundle == null)
            {
                Debug.LogError("Failed to load AssetBundle from embedded resource.");
            }
            return assetBundle;
        }
    }
}