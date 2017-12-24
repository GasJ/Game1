using UnityEngine;

public class ActionController : MonoBehaviour
{
    protected bool isMyTurn = false;

    /// <summary>
    /// The character that this action controller works with
    /// </summary>
    public Character MyCharacter
    {
        get
        {
            return gameObject.GetComponent<Character>();
        }
    }

    /// <summary>
    /// Start to act
    /// </summary>
    public void startTurn()
    {
        isMyTurn = true;
        act();
    }

    /// <summary>
    /// Notify the global controller that the turn has been finished
    /// </summary>
    public void notifyMovingFinished()
    {
        isMyTurn = false;
        GlobalController.NotifyTurnFinished(this);
    }

    /// <summary>
    /// The action that this action controller would do
    /// </summary>
    protected virtual void act() { }
}
