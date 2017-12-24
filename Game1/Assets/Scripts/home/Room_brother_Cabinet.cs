public class Room_brother_Cabinet : InteractiveObject{


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
                "There is a toy.",
                new Utility.ButtonDeclaration
                {
                    Text = "take",
                    OnClickHandler = () =>
                      {
                          _isObjectInside = false;
                          player.ItemList.Add(new Item { Name = "Toy", Property = "Room_brother" });
                          Utility.ShowDialog("You got a toy.\nThere will be a surprise if you go to the toilet.", null, null);
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
