using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class GameEvent
{
    public List<Waypoint> EventPosition { get; set; }
    public int Severity { get; set; }
    public string EventType { get; set; }
}

public class GameEventHolder
{
    private List<GameEvent> GameEventList { get; set; }

    public int TotalServerity
    {
        get
        {
            return calculateServerity(GameEventList);
        }
    }

    public GameEventHolder()
    {
        GameEventList = new List<GameEvent>();
    }

    public int this[string eventType]
    {
        get
        {
            var e = GameEventList.Find((ge) => { return ge.EventType == eventType; });
            return e == null ? 0 : e.Severity;
        }
        set
        {
            var e = GameEventList.Find((ge) => { return ge.EventType == eventType; });
            if (e == null)
            {
                GameEventList.Add(new GameEvent { EventType = eventType, Severity = value });
            }
            else
            {
                e.Severity = value;
            }
        }
    }

    private int calculateServerity(IEnumerable<GameEvent> eventList)
    {
        var serverity = 0;
        foreach (var evt in eventList)
        {
            serverity += evt.Severity;
        }
        return serverity;
    }
}
