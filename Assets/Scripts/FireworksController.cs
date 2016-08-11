using UnityEngine;
using System.Collections;

public class FireworksController : MonoBehaviour
{
    public GameObject fireworks;
    public float fireworksPlayTimeInSeconds;

    private bool fireworksActive = false;
    private float fireworksStartTime;

	void Update ()
    {
        if (fireworks.activeSelf && (Time.time - fireworksStartTime > fireworksPlayTimeInSeconds))
        {
            StopFireworks();
        }
    }

    void StartFireworks()
    {
        fireworks.SetActive(true);
        fireworksStartTime = Time.time;
    }

    void StopFireworks()
    {
        fireworks.SetActive(false);
        EventManager.TriggerEvent(EventName.FireworksOver);
    }

    void OnEnable()
    {
        EventManager.StartListening(EventName.RoundStart, StopFireworks);
        EventManager.StartListening(EventName.RoundEnd, StartFireworks);
    }

    void OnDisable()
    {
        EventManager.StartListening(EventName.RoundStart, StopFireworks);
        EventManager.StopListening(EventName.RoundEnd, StartFireworks);
    }
}
