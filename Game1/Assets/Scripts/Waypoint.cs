using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public bool IsAbleToReach = true;
    public GameObject MyContainerGameObject;
    public int X;
    public int Y;
    public GameObject Left;
    public Utility.MoveMode LeftMoveMethod = Utility.MoveMode.Normal;
    public GameObject Right;
    public Utility.MoveMode RightMoveMethod = Utility.MoveMode.Normal;
    public GameObject Up;
    public Utility.MoveMode UpMoveMethod = Utility.MoveMode.Normal;
    public GameObject Down;
    public Utility.MoveMode DownMoveMethod = Utility.MoveMode.Normal;

    public WaypointContainer MyContainer
    {
        get
        {
            return MyContainerGameObject.GetComponent<WaypointContainer>();
        }
    }

    public delegate void PlayerMoveEventHandler(GameObject waypoint, Utility.MoveMode moveMethod, Utility.Direction movingDirection);


    void Start()
    {
        setDefaultCoordinate();
    }
                                                          
   /* a* pathfinding is not very suitable in our case,
    * since we have waypoints, and AI cannot only walk diagonally,
    * so we use search tree when the destination is located in a different room
    */

    public Utility.Direction move(Waypoint destination) // this method will only return the next direction that AI should move towards
    {
        //var direction = Utility.Direction.None;

        if(MyContainer.Name.Equals(destination.MyContainer.Name)){  // in the same room
            if (destination.X - this.X > 0) 
            {
                if (this.Right != null && this.Right.GetComponent<Waypoint>() != null)  //player at right and no barrier
                {
                    return Utility.Direction.Right;
                }
                else if(destination.Y - this.Y < 0 && this.Up != null && this.Up.GetComponent<Waypoint>() != null)  //try another path
                {
                    return Utility.Direction.Up;
                }
                else if (destination.Y - this.Y > 0 && this.Down != null && this.Down.GetComponent<Waypoint>() != null)
                {
                    return Utility.Direction.Down;
                }
                else
                {
                    if (this.Left!=null && this.Left.GetComponent<Waypoint>() != null)  // back up
                    { return Utility.Direction.Left; }
                    else if (this.Up != null && this.Up.GetComponent<Waypoint>() != null)
                    {return Utility.Direction.Up;}
                    else
                    {return Utility.Direction.Down;}

                }
            }
            if (destination.X - this.X < 0) 
            {
                if (this.Left != null && this.Left.GetComponent<Waypoint>() != null)    //player at left and no barrier
                {
                    return Utility.Direction.Left;
                }
                else if (destination.Y - this.Y < 0 && this.Up != null && this.Up.GetComponent<Waypoint>() != null) //try another path
                {
                    return Utility.Direction.Up;
                }
                else if (destination.Y - this.Y > 0 && this.Down != null && this.Down.GetComponent<Waypoint>() != null)
                {
                    return Utility.Direction.Down;
                }
                else
                {
                    if (this.Right != null && this.Right.GetComponent<Waypoint>() != null)  // back up
                    { return Utility.Direction.Right; }
                    else if (this.Up != null && this.Up.GetComponent<Waypoint>() != null)
                    { return Utility.Direction.Up; }
                    else
                    { return Utility.Direction.Down; }
                }
            }

            if (destination.Y - this.Y < 0) 
            {
                if (this.Up != null && this.Up.GetComponent<Waypoint>() != null)    //player at up and no barrier
                {
                    return Utility.Direction.Up;
                }
                else if (destination.X - this.X > 0 && this.Right != null && this.Right.GetComponent<Waypoint>() != null)   //try another path
                {
                    return Utility.Direction.Right;
                }
                else if (destination.X - this.X < 0 && this.Left != null && this.Left.GetComponent<Waypoint>() != null)
                {
                    return Utility.Direction.Left;
                }
                else
                {
                    if (this.Down != null && this.Down.GetComponent<Waypoint>() != null)  // back up
                    { return Utility.Direction.Down; }
                    else if (this.Left != null && this.Left.GetComponent<Waypoint>() != null)
                    { return Utility.Direction.Left; }
                    else
                    { return Utility.Direction.Right; }
                }
            }
            if(destination.Y - this.Y > 0)
            {
                if (this.Down != null && this.Down.GetComponent<Waypoint>() != null)    //player at down and no barrier
                {
                    return Utility.Direction.Down;
                }
                else if (destination.X - this.X > 0 && this.Right != null && this.Right.GetComponent<Waypoint>() != null)   // try another path
                {
                    return Utility.Direction.Right;
                }
                else if (destination.X - this.X < 0 && this.Left != null && this.Left.GetComponent<Waypoint>() != null)   // try another path
                {
                    return Utility.Direction.Left;
                }
                else
                {
                    if (this.Up != null && this.Up.GetComponent<Waypoint>() != null)  // back up
                    { return Utility.Direction.Up; }
                    else if (this.Left != null && this.Left.GetComponent<Waypoint>() != null)
                    { return Utility.Direction.Left; }
                    else
                    { return Utility.Direction.Right; }
                }
            }
            else{
                    return Utility.Direction.None;  //should never reach here
            }
        }

        else{    //different room

            if (moveHelper(MyContainer, destination, 0, null).MyWaypoint == this) //case1: AI already stands right beside the door,just go through the door.
            { 
                return moveHelper(MyContainer, destination, 0, null).Direction;
            }
            else{
                return move(moveHelper(MyContainer, destination, 0, null).MyWaypoint);   //case2: AI is not at the door, walk towards the door first.
            }
        }

        //return direction;
    }

    public WaypointConnection moveHelper(WaypointContainer startRoom, Waypoint destinationHelper, int count, WaypointContainer comeFrom){  //this method return which door(WaypointConnection) AI should move towards
        WaypointConnection result = null;

        foreach (var conn in startRoom.WaypointConnections)
        {
            if (conn.ConnectedContainer == destinationHelper.MyContainer)
            {
                return conn;
            }
            else
            {
                if (count < 5 && conn.ConnectedContainer != comeFrom)
                {
                    var temp = moveHelper(conn.ConnectedContainer, destinationHelper, count + 1, startRoom);
					//result = temp != null ? conn : null; 
					if (temp == null) {
						result = null;
					}
					else 
					{
						return conn;
					}
                }
            }
        }

        return result;    //shuold never reach here
    }

    public GameObject getDestination(Utility.Direction direction)
    {
        switch (direction)
        {
            case Utility.Direction.Left:
                return Left;
            case Utility.Direction.Right:
                return Right;
            case Utility.Direction.Up:
                return Up;
            case Utility.Direction.Down:
                return Down;
            default:
                return null;
        }
    }

    public Utility.MoveMode getMoveMode(Utility.Direction direction)
    {
        switch (direction)
        {
            case Utility.Direction.Left:
                return LeftMoveMethod;
            case Utility.Direction.Right:
                return RightMoveMethod;
            case Utility.Direction.Up:
                return UpMoveMethod;
            case Utility.Direction.Down:
                return DownMoveMethod;
            default:
                return Utility.MoveMode.Teleport;
        }
    }

    public Utility.Direction LinksToObject(GameObject obj)
    {
        if (Left == obj)
        {
            return Utility.Direction.Left;
        }
        else if (Right == obj)
        {
            return Utility.Direction.Right;
        }
        else if (Up == obj)
        {
            return Utility.Direction.Up;
        }
        else if (Down == obj)
        {
            return Utility.Direction.Down;
        }
        else
        {
            return Utility.Direction.None;
        }
    }

    private void setDefaultCoordinate()
    {
        try
        {
            var tempList = gameObject.name.Split('_');
            Y = int.Parse(tempList[1]);
            X = int.Parse(tempList[2]);
        }
        catch
        {

        }
    }
}
