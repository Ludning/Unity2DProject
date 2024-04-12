using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBusManager : Manager<EventBusManager>
{
    private Dictionary<Type, Delegate> delegateDictionary = new Dictionary<Type, Delegate>();

    public void Publish<T>(T e)
    {
        if (delegateDictionary.TryGetValue(typeof(T), out var subscribers))
        {
            var subscriberList = subscribers as Action<T>;
            subscriberList?.Invoke(e);
        }
    }
    public void Subscribe<T>(Action<T> subscriber)
    {
        if (delegateDictionary.ContainsKey(typeof(T)))
        {
            delegateDictionary[typeof(T)] = Delegate.Combine(delegateDictionary[typeof(T)], subscriber);
        }
        else
        {
            delegateDictionary[typeof(T)] = subscriber;
        }
    }
    public void Unsubscribe<TEvent>(Action<TEvent> subscriber)
    {
        if (delegateDictionary.ContainsKey(typeof(TEvent)))
        {
            var currentDelegate = delegateDictionary[typeof(TEvent)];
            var newDelegate = Delegate.Remove(currentDelegate, subscriber);
            if (newDelegate == null)
            {
                delegateDictionary.Remove(typeof(TEvent));
            }
            else
            {
                delegateDictionary[typeof(TEvent)] = newDelegate;
            }
        }
    }
}
