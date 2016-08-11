using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public enum EventName
{
    ThumbsUp,
    RoundStart,
    RoundEnd,
    FireworksOver
};

public class EventManager : MonoBehaviour 
{
    private static Dictionary<EventName, UnityEvent> eventDictionary;

    #region Singleton
    private static EventManager instance;
    public static EventManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(EventManager)) as EventManager;

                if (instance == null)
                {
                    Debug.LogError("There needs to be one active EventManager script on a GameObject in your scene.");
                }
            }
            return instance;
        }
    }

    static void Init()
    {
        if (instance == null)
        {
            instance = FindObjectOfType(typeof(EventManager)) as EventManager;
        }

        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<EventName, UnityEvent>();
        }
    }
    #endregion

    void Awake()
    {
        Init();
    }

    public static void StartListening(EventName eventName, UnityAction listener)
    {
        if(instance == null)
        {
            Init();
        }
        UnityEvent thisEvent = null;
        if (eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(EventName eventName, UnityAction listener)
    {
        if (instance == null) return;
        UnityEvent thisEvent = null;
        if (eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(EventName eventName)
    {
        UnityEvent thisEvent = null;
        if (eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke();
        }
    }
}
