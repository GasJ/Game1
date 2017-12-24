using UnityEngine;

public interface IMovable : IDirectional
{
    GameObject CurrentWaypoint { get; }
    bool IsMoving { get; }
    float MovingSpeed { get; }
}
