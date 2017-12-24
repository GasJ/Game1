using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrotherInteractive : InteractiveObject {
    public override void interact(Character character)
    {
        Debug.Log("try to interact with bro.");
        if (character is Player) interactWithPlayer(character as Player);
    }

/*     private void interactWithPlayer(Player player)
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
                        GlobalController.GlobalGameEventHolder["Chaos"] += 8;
                        player.ItemList.RemoveAll((i) => { return i.Name == "Toy"; });
                        player.MyActionController.notifyMovingFinished();
                        Utility.ShowDialog("The toy has been flush down the toilet.", null, null);
                        toyFlushed = true;
                    }
                },
                null);
        }
        else
        {
            Utility.ShowDialog("This is a toilet.",null,null);
        }
    } */

        private void interactWithPlayer(Player player)
    {
        Debug.Log("we are supposed to talk with each other.");
        GameObject.Find("Brother").GetComponent<BrotherAIController>().talking = true;
        //Utility.ShowDialog("you can talk to your brother.", null, null);
        //player.MyActionController.notifyMovingFinished();
         int relationship = GameObject.Find("Brother").GetComponent<BrotherAIController>().relationship;
        if(relationship < 60) {
            Utility.ShowDialog(
                "Oh you.\n" + "I don't wanna talk to you.\n" + "You are the worst brother in this world.",
                new Utility.ButtonDeclaration
                {
                    Text = "You too. And also the worst little pig.",
                    OnClickHandler = () =>
                    {
                        GameObject.Find("Brother").GetComponent<BrotherAIController>().talkContent = "curse";
                        player.MyActionController.notifyMovingFinished();
                        Utility.ShowDialog("You brother hates you now.", null, null);
                    }
                },
                new Utility.ButtonDeclaration
                {
                    Text = "Come on, bro. I like you.",
                    OnClickHandler = () =>
                    {
                        Utility.ShowDialog("Okay, but I don't like you..", 
                         new Utility.ButtonDeclaration{
                            Text = "Don't be a gerk.",
                            OnClickHandler = () =>{
                                GameObject.Find("Brother").GetComponent<BrotherAIController>().talkContent = "normal";
                                player.MyActionController.notifyMovingFinished();
                                Utility.ShowDialog("You brother just goes away.", null, null);
                            }
                         },
                         new Utility.ButtonDeclaration{
                             Text = "You are my only friend in this home.",
                             OnClickHandler = () =>{
                                GameObject.Find("Brother").GetComponent<BrotherAIController>().talkContent = "comfort";
                                player.MyActionController.notifyMovingFinished();
                                Utility.ShowDialog("Your brother likes you a little more.", null, null);
                            }
                         });
                    }
                });
        }

        //relationship > 60
        Utility.ShowDialog(
                "Hello. Brother, What's up?",
                new Utility.ButtonDeclaration
                {
                    Text = "Go away.",
                    OnClickHandler = () =>
                    {
                        GameObject.Find("Brother").GetComponent<BrotherAIController>().talkContent = "curse";
                        player.MyActionController.notifyMovingFinished();
                        Utility.ShowDialog("You brother hates you now.", null, null);
                    }
                },
                new Utility.ButtonDeclaration
                {
                    Text = "Nothing can be up in this home.",
                    OnClickHandler = () =>
                    {
                        Utility.ShowDialog("Haha, funny..", 
                            new Utility.ButtonDeclaration{
                                Text = "Haha...",
                                OnClickHandler = () =>
                                {
                                    GameObject.Find("Brother").GetComponent<BrotherAIController>().talkContent = "normal";
                                    player.MyActionController.notifyMovingFinished();
                                    Utility.ShowDialog("You brother just goes away.", null, null);
                                }
                            },
                            new Utility.ButtonDeclaration{
                                Text = "Would you like to hang out?",
                                OnClickHandler = () =>
                                {
                                    GameObject.Find("Brother").GetComponent<BrotherAIController>().talkContent = "comfort";
                                    player.MyActionController.notifyMovingFinished();
                                    Utility.ShowDialog("Good. But you can't get out from our house now..", null, null);
                                }
                            });
                    }
                }); 
    }

}