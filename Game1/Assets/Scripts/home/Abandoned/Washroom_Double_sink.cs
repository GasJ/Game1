public class Washroom_Double_sink : InteractiveObject
{
    private bool _isOpen =false;

    public override void interact(Character character)
    {
        if(character is Player) interactWithPlayer(character as Player);
    }

    private void interactWithPlayer(Player player)
    {
        if (!_isOpen)
        {
            Utility.ShowDialog(
                "The tap is turned off.",
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
                "The tap is turned on.",
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
