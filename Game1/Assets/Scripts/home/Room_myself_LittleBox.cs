public class Room_myself_LittleBox : InteractiveObject
{
    private bool _isKeyInside = true;
    
    public override void interact(Character character)
    {
        if (character is Player) interactWithPlayer(character as Player);
    }

    private void interactWithPlayer(Player player)
    {
        if (_isKeyInside)
        {
            player.ItemList.Add(new Item { Name = "key", Property = "my_bedroom" });
            _isKeyInside = false;
            Utility.ShowDialog("You got a key.", null, null);
            player.MyActionController.notifyMovingFinished();
        }
        else
        {
            Utility.ShowDialog("Nothing is there.", null, null);
        }
    }
}
