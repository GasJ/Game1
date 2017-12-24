using UnityEngine;

public abstract class InteractiveObject : MonoBehaviour
{
    /// <summary>
    /// The name registered to interactive object dictionary in global controller
    /// </summary>
    public string registerName;

    /// <summary>
    /// The action to do when a character interact with this interactive object
    /// </summary>
    /// <param name="character"></param>
    public abstract void interact(Character character);

    protected void Start()
    {
        if (registerName != null && registerName != "") 
        {
            GlobalController.InteractiveObjectDictionary.Add(registerName, this);
        }
    }
}