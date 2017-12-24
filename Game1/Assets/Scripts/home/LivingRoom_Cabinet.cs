public class LivingRoom_Cabinet : InteractiveObject
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
            player.ItemList.Add(new Item { Name = "key", Property = "livingRoom" });
            _isKeyInside = false;
            Utility.ShowDialog("You got a key.", null, null);
        }
        else
        {
            Utility.ShowDialog("Nothing is there.", null, null);
        }
    }
}
