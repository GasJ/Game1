using UnityEngine;

public class Garage_car : InteractiveObject{


	private bool _isObjectInside = true;


	public override void interact(Character character)
	{
		if (character is Player) interactWithPlayer(character as Player);
	}

	private void interactWithPlayer(Player player)
	{

		Utility.ShowDialog("You cannot get in the car without the key!", null, null);
	}
}

