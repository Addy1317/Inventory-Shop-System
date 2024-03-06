using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ShopAndInventory
{
    public class EventController<T>
    {
        public Action<T> baseEvent;
        public void InvokeEvent(T type) => baseEvent?.Invoke(type);
        public void AddEventListener(Action<T> listener) => baseEvent += listener;
        public void RemoveEventListener(Action<T> listener) => baseEvent -= listener;
    }

    public class EventController<T1, T2>
    {
        public Action<T1, T2> baseEvent;
        public void InvokeEvent(T1 type1, T2 type2) => baseEvent?.Invoke(type1, type2);
        public void AddEventListener(Action<T1, T2> listener) => baseEvent += listener;
        public void RemoveEventListener(Action<T1, T2> listener) => baseEvent -= listener;
    }
}