using UnityEngine;

public class PlayerController : ActionController
{
    private void Start()
    {
        addInputHandlers();
    }


    private void addInputHandlers()
    {
        GlobalController.CurrentGlobalController.leftButtonDownAction += () => { if (isMyTurn) MyCharacter.moveToDeirection(Utility.Direction.Left); };
        GlobalController.CurrentGlobalController.rightButtonDownAction += () => { if (isMyTurn) MyCharacter.moveToDeirection(Utility.Direction.Right); };
        GlobalController.CurrentGlobalController.upButtonDownAction += () => { if (isMyTurn) MyCharacter.moveToDeirection(Utility.Direction.Up); };
        GlobalController.CurrentGlobalController.downButtonDownAction += () => { if (isMyTurn) MyCharacter.moveToDeirection(Utility.Direction.Down); };
        GlobalController.CurrentGlobalController.mouseLeftUpAction = () =>
        {
            if (isMyTurn)
            {
                var colliders = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                if (colliders.Length > 0)
                {
                    foreach (var c in colliders)
                    {
                        MyCharacter.moveToDeirection(MyCharacter.CurrentWaypoint.GetComponent<Waypoint>().LinksToObject(c.gameObject));
                    }
                }
            }
        };
    }
}
