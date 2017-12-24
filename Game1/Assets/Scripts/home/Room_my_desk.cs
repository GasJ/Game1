using UnityEngine;

public class Room_my_desk : InteractiveObject{


	private bool _isObjectInside = true;


	public override void interact(Character character)
	{
		if (character is Player) interactWithPlayer(character as Player);
	}

	private void interactWithPlayer(Player player)
	{
		
			Utility.ShowDialog("You cannot turn on you pc without password", null, null);
	}
}

