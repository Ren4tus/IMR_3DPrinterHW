using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class FloatEvent : UnityEvent<float>
{
}

public class IntEvent : UnityEvent<int>
{
}

public class StringEvent : UnityEvent<string>
{
}


public static class EventManager
{
    private static Dictionary<string, UnityEvent> eventDictionary;
    private static Dictionary<string, FloatEvent> floatEventDictionary;
    private static Dictionary<string, IntEvent> intEventDictionary;
    private static Dictionary<string, StringEvent> stringEventDictionary;

    private static void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, UnityEvent>();
        }

        if (floatEventDictionary == null)
        {
            floatEventDictionary = new Dictionary<string, FloatEvent>();
        }

        if (intEventDictionary == null)
        {
            intEventDictionary = new Dictionary<string, IntEvent>();
        }

        if (stringEventDictionary == null)
        {
            stringEventDictionary = new Dictionary<string, StringEvent>();
        }
    }

    public static void StartListening(string eventName, UnityAction listener)
    {
        Init();
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

    public static void StopListening(string eventName, UnityAction listener)
    {
        Init();
        UnityEvent thisEvent = null;
        if (eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(string eventName)
    {
        Init();

        UnityEvent thisEvent = null;
        if (eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke();
        }
    }

}