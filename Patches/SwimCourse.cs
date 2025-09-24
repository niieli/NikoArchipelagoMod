using System.Collections;
using KinematicCharacterController.Core;
using NikoArchipelago.Archipelago;
using UnityEngine;
using UnityEngine.Serialization;

namespace NikoArchipelago.Patches;

public class SwimCourse : MonoBehaviour
{
    public static bool NoticeUp;
    private void Update()
    {
        if (ArchipelagoClient.SwimmingAcquired) return;
        if (!MyCharacterController.instance.isTouchingWater) return;
        StartCoroutine(Notice());
        MyCharacterController.instance.BackToDaisy();
    }
    
    private IEnumerator Notice()
    {
        if (NoticeUp || !SavedData.Instance.Notices) yield break;
        var t = Instantiate(Assets.NoticeSwimCourse, Plugin.NotifcationCanvas.transform);
        var time = 0f;
        NoticeUp = true;
        while (time < 60f)
        {
            time += Time.deltaTime;
            yield return null;
        }
        Destroy(t);
        NoticeUp = false;
    }
}