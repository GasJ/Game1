using UnityEngine;

public class Room_brother_playArea : InteractiveObject {


    public bool _isOpen = false;

    public override void interact(Character character)
    {
        if (character is Player) interactWithPlayer(character as Player);
    }

    private void interactWithPlayer(Player player)
    {
        if (!_isOpen)
        {
            Utility.ShowDialog(
                "Would you like to play with your brother?",
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
                        GameObject.Find("Brother").GetComponent<BrotherAIController>().play = true;
                        player.MyActionController.notifyMovingFinished();
                    }
                },
                null
                );
        }
        else
        {
            Utility.ShowDialog(
                "You're too tired to play again.",
                null,
                null
                );
        }
    }
}
