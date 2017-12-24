using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Balcony_Door : InteractiveObject
{
    public bool IsLocked = true;
    public string keyId;

    public override void interact(Character character)
    {
        if (character is Player) interactWithPlayer(character as Player);
    }

    private void interactWithPlayer(Player player)
    {
        int sheetCount=0;

        foreach (var item in player.ItemList)
        {
            if (item.Name == "Sheet" && (item.Property as string) == "Room_brother")
            {
                sheetCount++;
            }
            if (item.Name == "Sheet" && (item.Property as string) == "Room_myself")
            {
                sheetCount++;
            }
        }
Debug.Log(sheetCount);
        if (IsLocked)
        {
            switch(sheetCount){
                case 0:
                Utility.ShowDialog("You can not escape from balcony without anything.",null,null);
                    break;
                case 1:

                Utility.ShowDialog("You can not escape from balcony with only one sheet.",null,null);
                    break;
                case 2:
                Utility.ShowDialog(
                        "You can escape from balcony with two sheet.",
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
                    break;
                default:
                    break;
            }
        }
    }
}


