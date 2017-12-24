public class Room_myself_Bed : InteractiveObject{


    private bool _isObjectInside = true;


    public override void interact(Character character)
    {
        if (character is Player) interactWithPlayer(character as Player);
    }

    private void interactWithPlayer(Player player)
    {
        if (_isObjectInside)
        {
            Utility.ShowDialog(
                "You may take your sheet with you, it is helpful.\n"+
                "Maybe you can use it escape from balcony.\n"+
                "But you need one more.",
                new Utility.ButtonDeclaration
                {
                    Text = "take",
                    OnClickHandler = () =>
                      {
                          _isObjectInside = false;
                          player.ItemList.Add(new Item { Name = "Sheet", Property = "Room_myself" });
                          Utility.ShowDialog("You got your sheet.", null, null);
                          player.MyActionController.notifyMovingFinished();
                      }
                },
                null
                );
        }
        else
        {
            Utility.ShowDialog("Nothing is there.", null, null);
        }
    }
}
