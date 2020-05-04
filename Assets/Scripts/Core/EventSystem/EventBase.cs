using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperEvent
{
    public class EventBase
    {
        public const int eventId = 10086;
    }

    public class Event_UpdatePathFinding : EventBase
    {
        public new const int eventId = EventId.UpdatePathFinding;
    }
}
