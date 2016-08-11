using UnityEngine;
using System.Collections;

public class SkyController : MonoBehaviour
{
    private AzureSky_Controller azureSkyController;
    private float azureTimeRatio;

    public float startTimeOfDay;
    public float endTimeOfDay;

    // Use this for initialization
    void Start ()
    {
        azureTimeRatio = ((endTimeOfDay - startTimeOfDay) / 24.0f);
        azureSkyController = FindObjectOfType(typeof(AzureSky_Controller)) as AzureSky_Controller;
        azureSkyController.SetTime(startTimeOfDay, 0);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnEnable()
    {
        EventManager.StartListening(EventName.RoundStart, StartSky);
        EventManager.StartListening(EventName.RoundEnd, EndSky);
    }

    void OnDisable()
    {
        EventManager.StopListening(EventName.RoundStart, StartSky);
        EventManager.StopListening(EventName.RoundEnd, EndSky);
    }

    void StartSky()
    {
        float roundLengthInMinutes = (FindObjectOfType(typeof(RoomManager)) as RoomManager).roundLengthInMinutes;
        azureSkyController.SetTime(startTimeOfDay, roundLengthInMinutes / azureTimeRatio);
    }

    void EndSky()
    {
        azureSkyController.SetTime(endTimeOfDay, 0);
    }
}
