using UnityEngine;

public class Stair : InteractiveObject
{

    public GameObject LeftWaypoint;
    public GameObject RightWaypoint;
    public override void interact(Character character)
    {
        
        if (character.CurrentWaypoint == LeftWaypoint)
        {
            character.moveToWaypoint(RightWaypoint, Utility.Direction.Right, Utility.MoveMode.Teleport);
        }
        else if(character.CurrentWaypoint == RightWaypoint)
        {
            character.moveToWaypoint(LeftWaypoint, Utility.Direction.Left, Utility.MoveMode.Teleport);
        }
    }
}