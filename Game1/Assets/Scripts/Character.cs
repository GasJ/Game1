using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour, IMovable, IAnimationUpdatable
{
    private Vector3 _targetPosition;

    public GameObject InitialWaypoint;
    public GameObject GlobalControllerGameObject;

    /// <summary>
    /// Indicate if this character can move
    /// </summary>
    public bool CanMove 
    {
        get 
        {
            if (this is Player)
                return !IsMoving && !GlobalController.IsShowingDialog && !GlobalController.IsShowingPauseMenu;
            else
                return !IsMoving;
        }
    }
    /// <summary>
    /// The current waypoint that this character stands on
    /// </summary>
    public GameObject CurrentWaypoint { get; private set; }
    /// <summary>
    /// Indicate if this character is moving
    /// </summary>
    public bool IsMoving { get; private set; }
    /// <summary>
    /// The moving speed of this character
    /// </summary>
    public float MovingSpeed { get; protected set; }
    /// <summary>
    /// The direction that this character faces to
    /// </summary>
    public Utility.Direction Direction { get; private set; }
    /// <summary>
    /// The action controller of this character
    /// </summary>
    public ActionController MyActionController
    {
        get
        {
            return gameObject.GetComponent<ActionController>();
        }
    }

    // Use this for initialization
    public void Start () {
        MovingSpeed = 2f;
        moveToInitialWaypoint();
	}

    // Update is called once per frame
    public void Update () {
        handleCharacterMoving();
    }

    /// <summary>
    /// Move the character to a specifc waypoint
    /// </summary>
    /// <param name="destination"></param>
    /// <param name="movingDirection"></param>
    public void moveToWaypoint(GameObject destination, Utility.Direction movingDirection, Utility.MoveMode moveMode)
    {
        if (CanMove)
        {
            Direction = movingDirection;

            CurrentWaypoint = destination;
            var waypointPosition = destination.transform.position;
            _targetPosition = waypointPosition;

            switch (moveMode)
            {
                case Utility.MoveMode.Normal:
                    IsMoving = true;
                    break;
                case Utility.MoveMode.Teleport:
                    gameObject.transform.position = _targetPosition;
                    if(this is Player) moveMainCameraToMe();
                    MyActionController.notifyMovingFinished();
                    break;
                default:
                    break;
            }

            updateAnimation(); 
        }
    }

    /// <summary>
    /// Move the character to a direction
    /// </summary>
    /// <param name="wayPoint"></param>
    public void moveToDeirection(Utility.Direction movingDirection)
    {
        if (CanMove)
        {
            var destination = CurrentWaypoint.GetComponent<Waypoint>().getDestination(movingDirection);

            if (destination != null)
            {
                if (destination.GetComponent<InteractiveObject>() != null)
                {
                    //Debug.Log("we are interacting with " + destination.GetComponent<InteractiveObject>().registerName);
                    Direction = movingDirection;
                    updateAnimation();
                    destination.GetComponent<InteractiveObject>().interact(this);
                }
                else if (destination.GetComponent<Waypoint>() != null)
                {
                    moveToWaypoint(
                            destination,
                            movingDirection,
                            CurrentWaypoint.GetComponent<Waypoint>().getMoveMode(movingDirection)
                        );
                }
            } 
        }
    }

    /// <summary>
    /// Update the character's animation
    /// </summary>
    public virtual void updateAnimation() { }

    private void moveToInitialWaypoint()
    {
        moveToWaypoint(InitialWaypoint, Utility.Direction.Up, Utility.MoveMode.Teleport);
    }

    /// <summary>
    /// Move the main camera to the character's current position
    /// </summary>
    private void moveMainCameraToMe()
    {
        GlobalController.CurrentGlobalController.moveCameraTo(gameObject);
    }

    /// <summary>
    /// Move the main camera to the character's current position with a given speed
    /// </summary>
    /// <param name="movingSpeed">the moving speed</param>
    private void moveMainCameraToMe(float movingSpeed)
    {
        GlobalController.CurrentGlobalController.moveCameraTo(gameObject, movingSpeed);
    }

    private void handleCharacterMoving()
    {
        if (IsMoving)
        {
            var currentPosition = gameObject.transform.position;
            if (Vector3.Distance(currentPosition, _targetPosition) >= 0.1f)
            {
                gameObject.transform.position
                    = Vector3.MoveTowards(currentPosition, _targetPosition, MovingSpeed * Time.deltaTime);
                if(this is Player) moveMainCameraToMe();
            }
            else
            {
                gameObject.transform.position = _targetPosition;
                if(this is Player) moveMainCameraToMe();
                IsMoving = false;
                updateAnimation();
                MyActionController.notifyMovingFinished();
            }
        }
    }
}
