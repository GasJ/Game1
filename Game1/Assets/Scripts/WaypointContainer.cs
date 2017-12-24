using System.Collections.Generic;
using UnityEngine;

public class WaypointContainer : MonoBehaviour
{
    /// <summary>
    /// The name of this waypoint container
    /// </summary>
    public string Name;

    /// <summary>
    /// The list of all waypoint connections of this waypoint container
    /// </summary>
    public List<WaypointConnection> WaypointConnections
    {
        get
        {
            return new List<WaypointConnection>(gameObject.GetComponents<WaypointConnection>());
        }
    }

    public WaypointConnection this[string nameConnectionTo]
    {
        get
        {
            return WaypointConnections.Find((c)=> { return c.ConnectedContainer.Name == nameConnectionTo; });
        }
    }

    void Start()
    {
        setDefaultName();
    }

    private void setDefaultName()
    {
        Name = gameObject.name;
    }
}