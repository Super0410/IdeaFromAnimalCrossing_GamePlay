using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperEvent
{
    public class EventSystem : Singleton<EventSystem>
    {
        private Dictionary<int, List<Action<EventBase>>> listenerDict = new Dictionary<int, List<Action<EventBase>>>();

        public void AddListener(int id, Action<EventBase> cb)
        {
            if (!listenerDict.ContainsKey(id))
                listenerDict.Add(id, new List<Action<EventBase>>());

            List<Action<EventBase>> l = listenerDict[id];
            if (l.Contains(cb))
                return;
            l.Add(cb);
        }

        public void RemoveListener(int id, Action<EventBase> cb)
        {
            if (!listenerDict.ContainsKey(id))
                return;

            List<Action<EventBase>> l = listenerDict[id];
            if (!l.Contains(cb))
                return;
            l.Remove(cb);
        }

        public void Notify(int id, EventBase e)
        {
            if (!listenerDict.ContainsKey(id))
                return;

            List<Action<EventBase>> l = listenerDict[id];
            for (int i = 0; i < l.Count; i++)
            {
                Action<EventBase> cb = l[i];
                cb?.Invoke(e);
            }
        }
    }
}
