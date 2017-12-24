using UnityEngine;

public class Washroom_Toilet : InteractiveObject {

    public bool toyFlushed = false;
    
    public override void interact(Character character)
    {
        if (character is Player) interactWithPlayer(character as Player);
    }

    private void interactWithPlayer(Player player)
    {
        bool hasToy = false;
        foreach (var item in player.ItemList)
        {
            if (item.Name == "Toy" )
            {
                hasToy = true;
                break;
            }
        }
        if (hasToy)
        {
            Utility.ShowDialog(
                "This is a toilet.\n" + "Would you like to flush the toy down the toilet?",
                new Utility.ButtonDeclaration
                {
                    Text = "Yes",
                    OnClickHandler = () =>
                    {
                        gameObject.GetComponent<AudioSource>().Play();
                        player.ItemList.RemoveAll((i) => { return i.Name == "Toy"; });
                        toyFlushed = true;
                        GameObject.Find("Brother").GetComponent<BrotherAIController>().relationship-=100;
                        Utility.ShowDialog("The toy has been flush down the toilet.\n"+
                        "Your brother begin to cry.", 
                        new Utility.ButtonDeclaration
                        {
                            Text = "Great.",
                            OnClickHandler = () =>
                            {
                                player.MyActionController.notifyMovingFinished();
                            }
                        },
                     new Utility.ButtonDeclaration
                        {
                            Text = "Oh, Sorry.",
                            OnClickHandler = () =>
                            {
                                GameObject.Find("Brother").GetComponent<BrotherAIController>().relationship = 20;
                                player.MyActionController.notifyMovingFinished();
                            }
                        });
                        
                    }
                },
                null);
        }
        else
        {
            Utility.ShowDialog("This is a toilet.",null,null);
        }
    }
}
