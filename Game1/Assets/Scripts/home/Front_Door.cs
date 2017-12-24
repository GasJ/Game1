using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Front_Door : InteractiveObject
{
    public bool IsLocked = true;
    public string keyId;

    public override void interact(Character character)
    {
        if (character is Player) interactWithPlayer(character as Player);
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
                Utility.ShowDialog(
                        "The door has been unlocked.",
                        null,
                        new Utility.ButtonDeclaration
                        {
                            Text = "Escape",
                            OnClickHandler = () =>
                            {
                                gameObject.GetComponent<AudioSource>().Play();
                                SceneManager.LoadScene("Finish");
                                GlobalController.IsShowingPauseMenu = false;
                            }
                        }
                    );
            }
        }
    }


