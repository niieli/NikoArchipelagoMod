using Archipelago.MultiClient.Net.Enums;
using UnityEngine;

namespace NikoArchipelago;

public class APNotification
{
    public bool IsHint { get; }
    public string ItemName { get; }
    public string PlayerName { get; }
    public string SenderName { get; }
    public string LocationName { get; }
    public Sprite ItemIcon { get; }
    public Color BackgroundColor { get; }
    public Color TimerColor { get; }
    public string HintState { get; }
    public float Duration { get; }
    public ItemFlags ItemFlags { get; }

    public APNotification(
        bool isHint,
        string itemName,
        string playerName,
        string senderName,
        string locationName,
        ItemFlags itemFlags,
        float duration = 3f,
        Color? backgroundColor = null,
        Color? durationColor = null,
        Sprite icon = null,
        string hintState = null)
    {
        IsHint = isHint;
        ItemName = itemName;
        PlayerName = playerName;
        SenderName = senderName;
        LocationName = locationName;
        ItemFlags = itemFlags;
        ItemIcon = icon;
        if (backgroundColor != null)
            backgroundColor = new Color(backgroundColor.Value.r, backgroundColor.Value.g, backgroundColor.Value.b, 0.72f);
        BackgroundColor = backgroundColor ?? new Color(1, 0.54f, 0.76f, 0.72f);
        if (durationColor != null)
            durationColor = new Color(durationColor.Value.r, durationColor.Value.g, durationColor.Value.b, 0.79f);
        TimerColor = durationColor ?? new Color(0.486f, 0.60f, 1, 0.79f);
        HintState = hintState;
        Duration = duration;
    }
}