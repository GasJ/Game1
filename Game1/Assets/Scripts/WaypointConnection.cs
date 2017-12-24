using UnityEngine;

public class WaypointConnection : MonoBehaviour
{
    public GameObject ConnectedContainerObject;
    public GameObject MyWaypointGameObject;
    /// <summary>
    /// The direction for character to move to get to the connnected waypoint container
    /// </summary>
    public Utility.Direction Direction;

    /// <summary>
    /// The waypoint container connected to
    /// </summary>
    public WaypointContainer ConnectedContainer
    {
        get
        {
            return ConnectedContainerObject.GetComponent<WaypointContainer>();
        }
    }
    /// <summary>
    /// The waypoint in the waypoint container to connect to the other waypoint container
    /// </summary>
    public Waypoint MyWaypoint
    {
        get
        {
            return MyWaypointGameObject.GetComponent<Waypoint>();
        }
    }

}