public class Washroom_Bathtub : InteractiveObject
{
    private bool _isOpen = false;

    public override void interact(Character character)
    {
        if (character is Player) interactWithPlayer(character as Player);
    }

    private void interactWithPlayer(Player player)
    {
        if (!_isOpen)
        {
            Utility.ShowDialog(
                "The shower is turned off.",
                new Utility.ButtonDeclaration
                {
                    Text = "Turn on",
                    OnClickHandler = () =>
                    {
                        _isOpen = true;
                        //(this as IInteractiveObject).interact(player);
                    }
                },
                null
                );
            player.MyActionController.notifyMovingFinished();
        }
        else
        {
            Utility.ShowDialog(
                "The shower is turned on.",
                new Utility.ButtonDeclaration
                {
                    Text = "Turn off",
                    OnClickHandler = () =>
                    {
                        _isOpen = false;
                        //(this as IInteractiveObject).interact(player);
                    }
                },
                null
                );
            player.MyActionController.notifyMovingFinished();
        }
    }
}
