using UnityEngine;

public class Room_brother_Bed : InteractiveObject
{

    public bool _isOpen = false;

    private bool _isObjectInside = true;

    public override void interact(Character character)
    {
        if (character is Player) interactWithPlayer(character as Player);
    }

    private void interactWithPlayer(Player player)
    {
        if (!_isOpen)
        {
            Utility.ShowDialog(
                "You find some candies on the table.\n" + "Would you like to share with your brother?",
                new Utility.ButtonDeclaration
                {
                    Text = "Yes.",
                    OnClickHandler = () =>
                    {

                        Utility.ShowDialog(
                            "Your brother feels happy with you.",
                            null,
                            null
                            );
                        _isOpen = true;
                        //(this as IInteractiveObject).interact(player);
                        GameObject.Find("Brother").GetComponent<BrotherAIController>().sugar = true;
                        player.MyActionController.notifyMovingFinished();
                    }
                },
                null
                );
        }
        else
        {
            if (_isObjectInside)
            {
                if (GameObject.Find("Brother").GetComponent<BrotherAIController>().relationship > 90)
                {
                    Utility.ShowDialog(
                                    "You may take your brother's sheet with you,\n" +
                                    "because he is your best friend.\n",
                                    new Utility.ButtonDeclaration
                                    {
                                        Text = "take",
                                        OnClickHandler = () =>
                                          {
                                              _isObjectInside = false;
                                              player.ItemList.Add(new Item { Name = "Sheet", Property = "Room_brother" });
                                              Utility.ShowDialog("You got his sheet.", null, null);
                                              player.MyActionController.notifyMovingFinished();
                                          }
                                    },
                                    null
                                    );

                }
                else
                {
                    Utility.ShowDialog(
                    "Although there is sheet on bed,\n" +
                    "you could not take it.",
                    null,
                    null
                    );
                }
            }
            else
            {
                Utility.ShowDialog(
               "Nothing is there.",
               null,
               null
               );

            }
        }
    }
}
