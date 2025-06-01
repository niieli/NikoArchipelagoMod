using System.Collections;
using KinematicCharacterController.Core;
using NikoArchipelago.Archipelago;
using UnityEngine;

namespace NikoArchipelago.Patches;

public class SwimCourse : MonoBehaviour
{
    private bool _noticeUp;
    private void Update()
    {
        if (ArchipelagoClient.SwimmingAcquired) return;
        if (!MyCharacterController.instance.isTouchingWater) return;
        StartCoroutine(Notice());
        MyCharacterController.instance.BackToDaisy();
    }
    
    private IEnumerator Notice()
    {
        if (_noticeUp) yield break;
        var t = Instantiate(Plugin.NoticeSwimCourse, Plugin.NotifcationCanvas.transform);
        _noticeUp = true;
        yield return new WaitForSeconds(5f);
        Destroy(t);
        _noticeUp = false;
    }
}