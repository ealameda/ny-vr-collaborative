using UnityEngine;
using System.Collections;

public class ThumbsUpIndicator : Photon.MonoBehaviour
{
    public void ThumbsUp()
    {
        EventManager.TriggerEvent(EventName.ThumbsUp);
    }

    public void ThumbsNotUp()
    {
        EventManager.TriggerEvent(EventName.ThumbsUp);
    }
}
