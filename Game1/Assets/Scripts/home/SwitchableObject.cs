using UnityEngine;
public class SwitchableObject : InteractiveObject {


    public string objectname;
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
                objectname +" is turned off.",
                new Utility.ButtonDeclaration
                {
                    Text = "Turn on",
                    OnClickHandler = () =>
                    {
                        _isOpen = true;
                        gameObject.GetComponent<AudioSource>().Play();
                        //(this as IInteractiveObject).interact(player);
                        GlobalController.GlobalGameEventHolder["Chaos"] += 2;
                        player.MyActionController.notifyMovingFinished();
                    }
                },
                null
                );
        }
        else
        {
            Utility.ShowDialog(
                objectname +" is turned on.",
                new Utility.ButtonDeclaration
                {
                    Text = "Turn off",
                    OnClickHandler = () =>
                    {
                        _isOpen = false;
                        gameObject.GetComponent<AudioSource>().Stop();
                        //(this as IInteractiveObject).interact(player);
                        GlobalController.GlobalGameEventHolder["Chaos"] -= 2;
                        player.MyActionController.notifyMovingFinished();
                    }
                },
                null
                );
        }
    }
}
