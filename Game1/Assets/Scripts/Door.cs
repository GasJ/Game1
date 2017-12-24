using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractiveObject
{
    public bool IsLocked = false;
    public string keyId;
    public GameObject LeftWaypoint;
    public GameObject RightWaypoint;

    public override void interact(Character character)
    {
        if (character is Player) interactWithPlayer(character as Player);
        if (character is AI) interactWithAI(character as AI);
    }
    private void interactWithAI(AI ai)
    {
        pass(ai);
    }

    private void interactWithPlayer(Player player)
    {
        bool hasKey = false;

        foreach (var item in player.ItemList)
        {
            if (item.Name == "key" && (item.Property as string) == keyId)
            {
                hasKey = true;
                break;
            }
        }

        if (IsLocked)
        {
            if (hasKey)
            {
                Utility.ShowDialog(
                            "The door has been locked.",
                            new Utility.ButtonDeclaration
                            {
                                Text = "Unlock",
                                OnClickHandler = () =>
                                {
                                    IsLocked = false;
                                    (this as InteractiveObject).interact(player);
                                }
                            },
                            null
                    );
            }
            else
            {
                Utility.ShowDialog("You don't have a key.", null, null);
            }
        }
        else // is unlocked
        {
            if (hasKey)
            {
                Utility.ShowDialog(
                        "The door has been unlocked.",
                        new Utility.ButtonDeclaration
                        {
                            Text = "Lock",
                            OnClickHandler = () =>
                            {
                                IsLocked = true;
                                (this as InteractiveObject).interact(player);
                            }
                        },
                        new Utility.ButtonDeclaration
                        {
                            Text = "Pass",
                            OnClickHandler = () =>
                            {
                                pass(player);
                            }
                        }
                    );
            }
            else
            {
                Utility.ShowDialog(
                        "The door has been unlocked.",
                        null,
                        new Utility.ButtonDeclaration
                        {
                            Text = "Pass",
                            OnClickHandler = () =>
                            {
                                pass(player);
                            }
                        }
                    );
            }
        }
    }

    private void pass(Character character)
    {
        gameObject.GetComponent<AudioSource>().Play();

        if (character.CurrentWaypoint == LeftWaypoint)
        {
            character.moveToWaypoint(RightWaypoint, Utility.Direction.Right, Utility.MoveMode.Normal);
        }
        else if(character.CurrentWaypoint == RightWaypoint)
        {
            character.moveToWaypoint(LeftWaypoint, Utility.Direction.Left, Utility.MoveMode.Normal);
        }
    }
}
