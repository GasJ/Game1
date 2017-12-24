using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BrotherAIController : AIController
{
    public bool crying = false; //whether the brother is crying or not
    public bool sugar = false; //can be changed after the player made a dicision
    public bool play = false; //can be changed after the player made a dicision
    public bool talking = false;

    public string talkContent;

    public bool catched = false; //the player is catched or not.
    public bool angry = false; //parents are angry
    public bool upstair = false; //does any parent is upstair?
    
    public int relationship = 60;
    public GameObject upWaypoint;
    public GameObject downWaypoint;
    public GameObject playArea;
    void Start()
    {
        NewMethod();
    }

    private void NewMethod()
    {
        _myDecisionTree = new DecisionTree
        {
            Name = "Root",
            MyPredicate = FirstDecision,  //prepare
            Nodes = new List<DecisionTree>
            {
                //A.
                new DecisionTree
                {
                         Name = "kidCatched",
                        Nodes = new List<DecisionTree>{
                            new DecisionTree
                                {
                                    Name = "goodFriends",
                                    MyAction = notHappy //prepare
                                },
                                new DecisionTree
                                {
                                    Name = "notFriends",
                                     MyAction = soHappy //prepare
                                },
                            },
                            MyPredicate = areWeFriends //prepare
                },
                //B.
                new DecisionTree
                {
                    //if the brothe is not angry yet (the relationship is >= 50)
                    Name="notYetAngry",
                    MyPredicate = whetherTalk, //prepare
                    //its nodes
                    Nodes = new List<DecisionTree>{
                        new DecisionTree
                        {
                            Name = "needToTalk",
                            MyAction = talkToKid
                        },

                        new DecisionTree
                        {
                            Name = "noNeedToTalk",
                            MyPredicate = canWePlayTogether, //prepare
                            Nodes = new List<DecisionTree>{
                                        new DecisionTree
                                        {
                                            Name = "playTogether",
                                            MyAction = playWithKid //prepare
                                        },
                                        new DecisionTree
                                        {
                                            //cannot play together
                                            Name = "isSugar",
                                            MyPredicate = doYouHaveSugar,      //prepare
                                            Nodes = new List<DecisionTree>{
                                                new DecisionTree{
                                                    Name = "getSugar",
                                                    MyAction = getSugarFromKid //prepare
                                                },
                                                new DecisionTree{
                                                    Name = "randomAction",
                                                    MyAction = randomAction //prepare
                                                },
                                            },
                                        },
                                },
                    },
                    },
                },

                //C.
                new DecisionTree
                {
                    //if the brother is engaged (the relationship is < 50)
                    Name="Engaged",
                    MyPredicate = isParentsAngry, //prepare
                    //its nodes
                    Nodes = new List<DecisionTree>{
                        //a.
                        new DecisionTree
                        {
                            Name = "parentsNotAngry",
                            MyPredicate = areToysGone, //prepare
                            Nodes = new List<DecisionTree>{
                                //1.
                                new DecisionTree
                                {
                                    Name = "toysGone",
                                    MyAction = cryChaos //prepare
                                },
                                //2.
                                new DecisionTree
                                {
                                    Name = "toysNotGone",
                                    MyAction = randomDirtyTrick //prepare
                                },
                            },
                        },

                        //b.
                        new DecisionTree
                        {
                            Name = "parentsAngry",
                            MyPredicate = isParentsUpstair, //prepare
                            Nodes = new List<DecisionTree>{
                                //1.
                                new DecisionTree
                                {
                                    /* 
                                    find the parent who is on upstair,
                                    and let this parent go down
                                     */
                                    Name = "tellParentDownstair",
                                    MyAction = tellParent //prepare
                                },
                                //2.
                                new DecisionTree
                                {
                                    /* 
                                    find the parent who is going to go upstair,
                                    and cancle his movement
                                     */
                                    Name = "prenventParentUpstair",
                                    MyAction = downstair //prepare
                                },
                            },
                        },
                    },
                },
                new DecisionTree
                {
                    Name ="talk",
                    MyAction = talkToKid
                }
            }
    };
    }
    /*
    private string isKidCatched()
    {
        if(catched){
             return "kidCatched";
         }
         return "kidNotCatched";
    }
    */

    private bool _isUp = false;
    
    private void patrol()
    {
        /* patrol move. */
        var dire = MyCharacter.CurrentWaypoint.GetComponent<Waypoint>().move(downWaypoint.GetComponent<Waypoint>());

        if (MyCharacter.CurrentWaypoint == upWaypoint)
        {
            _isUp = false;
        }
        if (MyCharacter.CurrentWaypoint == downWaypoint)
        {
            _isUp = true;
        }

        if (_isUp)
        {
            dire = MyCharacter.CurrentWaypoint.GetComponent<Waypoint>().move(upWaypoint.GetComponent<Waypoint>());
        }
        else
        {
            dire = MyCharacter.CurrentWaypoint.GetComponent<Waypoint>().move(downWaypoint.GetComponent<Waypoint>());
        }
        MyCharacter.moveToDeirection(dire);

    }

    private void talkHelper(){
        Player play = GameObject.Find("Player").GetComponent<Player>();
        int relationship = GameObject.Find("Brother").GetComponent<BrotherAIController>().relationship;
        if(relationship < 60) {
            Utility.ShowDialog(
                "Oh you.\n" + "I don't wanna talk to you.\n" + "You are the worst brother in this world.",
                new Utility.ButtonDeclaration
                {
                    Text = "You too. And also the worst little pig.",
                    OnClickHandler = () =>
                    {
                        talkContent = "curse";
                        play.MyActionController.notifyMovingFinished();
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
                                talkContent = "normal";
                                play.MyActionController.notifyMovingFinished();
                                Utility.ShowDialog("You brother just goes away.", null, null);
                            }
                         },
                         new Utility.ButtonDeclaration{
                             Text = "You are my only friend in this home.",
                             OnClickHandler = () =>{
                                talkContent = "comfort";
                                play.MyActionController.notifyMovingFinished();
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
                        talkContent = "curse";
                        Utility.ShowDialog("You brother hates you now.", null, null);
                        play.MyActionController.notifyMovingFinished();
                    }
                },
                new Utility.ButtonDeclaration
                {
                    Text = "Nothing.",
                    OnClickHandler = () =>
                    {
                        Utility.ShowDialog("Haha, funny..", 
                            new Utility.ButtonDeclaration{
                                Text = "Haha...",
                                OnClickHandler = () =>
                                {
                                    talkContent = "normal";
                                    play.MyActionController.notifyMovingFinished();
                                    Utility.ShowDialog("You brother just goes away.", null, null);
                                }
                            },
                            new Utility.ButtonDeclaration{
                                Text = "Would you like to hang out?",
                                OnClickHandler = () =>
                                {
                                    talkContent = "comfort";
                                    play.MyActionController.notifyMovingFinished();
                                    Utility.ShowDialog("Good. But you can't get out from our house now..", null, null);
                                }
                            });
                    }
                });
    }
    private void talkToKid(){
         Debug.Log("talk");
         talkHelper();
        switch(talkContent){
            case "curse":
            //the conversation part ends at the player cursed the brother
            //relationship decreases.
                relationship -= 20;
                break;
            case "comfort":
            //the conversation part ends at the player comforted the brother
            //relationship increase.
                relationship += 20;
                break;
            default:
            //a normal result
            //relationship does not change.
                break;
        }
        talking = false;
        MyCharacter.MyActionController.notifyMovingFinished();
    }

    private void randomDirtyTrick(){

         Debug.Log("randomTrick");
        /* 
        get a random int x (0-100)
        if x > 50:
        //go to downstair
        downstair();
        return;

        //if x < 50:
        open the kid's music
        [
            if it is hard to get to the kid's room
            just try to use a remote controller.
        ]
        */
        relationship += 20;
        System.Random ran = new System.Random();
        if(ran.Next(0, 1000) >= 500){
            Debug.Log("tell parent");
            tellParent();
            return;
        }

        Debug.Log("open music");
        RemoteOpenMusic();
    }

    private void RemoteOpenMusic(){
        if(GameObject.Find("TV_table").GetComponent<SwitchableObject>()._isOpen == false){
            Utility.ShowDialog(
             "I will do something to disturb you.\nBut I won't tell you what I didi.\n - your bro.",
             null,
             null
         );
            GameObject.Find("TV_table").GetComponent<SwitchableObject>()._isOpen = true;
            GlobalController.GlobalGameEventHolder["Chaos"] += 3;
        }
        MyCharacter.MyActionController.notifyMovingFinished();
    }

    private void cryChaos(){
        /* 
            add chaos to the global control
         */

         Debug.Log("cry");
        gameObject.GetComponents<AudioSource>()[1].Play();
         if(crying==false){
            gameObject.GetComponent<AudioSource>().Play();
            crying = true;
             Utility.ShowDialog(
             "I will cry, and you will die.\n - your bro, who hates you.",
             null,
             null
         );
            GlobalController.GlobalGameEventHolder["Chaos"] += 3;
        }

        MyCharacter.MyActionController.notifyMovingFinished();

        /* 
            need to change parents' AI to react to the brother's crying
         */
    }

    private void randomAction(){
        /* 
        get a random int x (0,150)
        if x <50:
            rest - waiting at the current waypoint
        else if x > 100:
            play viedeo games
        else:
            move
         */
        Debug.Log("randomAction");
         System.Random ran = new System.Random();
         int x = ran.Next(0, 1500);
        if(x < 500){
            MyCharacter.MyActionController.notifyMovingFinished();
            return; // waiting at the current waypoint
        }
        if(x > 1000){
            /* 
                determine whether the brother is around the play area
                if yes:
                    play..
                if no:
                    go to the play area
             */
        var dire = MyCharacter.CurrentWaypoint.GetComponent<Waypoint>().move(playArea.GetComponent<Waypoint>());
        MyCharacter.moveToDeirection(dire);
        }
        patrol();
    }

    private void getSugarFromKid(){

         Debug.Log("getSugar");
        relationship += 20;
        sugar = false;
        MyCharacter.MyActionController.notifyMovingFinished();
        /* 
        set the sugar false.
         */
    }

    private void playWithKid(){

         Debug.Log("play");
        relationship += 30;
        play = false;
        MyCharacter.MyActionController.notifyMovingFinished();
        /* 
        set the playTogether false.
         */
    }

    private void soHappy(){

         Debug.Log("sohappy");
         catched = false;

         Utility.ShowDialog(
             "I am so happy to watch you are catched. - hate you, Bro.",
             new Utility.ButtonDeclaration
             {
                 Text = "...",
                    OnClickHandler = () =>
                    {
                    }
             },
             null);

        
        MyCharacter.MyActionController.notifyMovingFinished();
         
        /* 
            active the 
            HAPPY
            animator of the brother
         */

         
    }

    private void notHappy(){

         Debug.Log("nothappy");

         Utility.ShowDialog(
             "Oh, I wanna help you.\n" + 
             "But my useless developer cannot finish every part of the game." +
             "Love you, Bro.",
             null,
             null);
        catched = false;
        MyCharacter.MyActionController.notifyMovingFinished();
        /* 
            active the 
            UNHAPPY
            animator of the brother
         */
    }

    private void tellParent(){

         Debug.Log("tellparent");
         Utility.ShowDialog(
             "I will tell father everything of your schedule.",
             null,
             null
         );
         GameObject.Find("Father").GetComponent<FatherAIController>().knowSchedule = true;
         MyCharacter.MyActionController.notifyMovingFinished();
        /* 
            go to find parent,
            and then send message to parent
         */
         // get parent's waypoint 

    }

    private void downstair(){

        Debug.Log("downstair");
        if(crying == false)
        {
            GameObject.Find("Father").GetComponent<FatherAIController>().isCheck=false;
            GameObject.Find("Father").GetComponent<FatherAIController>().knowSchedule = false;
        }
        relationship += 20;

        MyCharacter.MyActionController.notifyMovingFinished();
        /* 
            go to find parent,
            and then parent will not go downstair
         */
    }

    private string whetherTalk(){
        
        /* 
            if talking is activate.
            return "needToTalk";

            else
            return "noNeedToTalk";
         */
        if(talking){
            return "needToTalk";
        }

         return "noNeedToTalk";
     }

    private string canWePlayTogether(){
        /* 
            if playTogether is activate.
            return "playTogether";

            else
            return "isSugar"; //to determine whether the brother would like to give the bro sugar
         */
        if(play){
            return "playTogether";
        }

        return "isSugar";
    }

    private string areWeFriends(){
        if(relationship >= 90){
            return "goodFriends";
        }

        return "notFriends";
    }

    private string areToysGone(){
        /* 
        determine whether the toys are gone

        if(gone){
           return "toysGone";
        }

        return "toysNotGone";
         */
        if (GameObject.Find("Toilet").GetComponent<Washroom_Toilet>().toyFlushed && relationship < 30)
        {
            relationship += 30;
            return "toysGone";
        }

       return "toysNotGone";
    }
    
     private string doYouHaveSugar(){
         /*
         sugar will be actived when the kid click the button
         give the bro sugar 
         if (sugar){
             return  "getSugar";
         }

         return "randomAction";
         
          */
          if(sugar){
              return  "getSugar";
          }
        return "randomAction";
     }

     private string isParentsUpstair(){
         /* 
         get parents' waypoint

         if(upstair){

             return "tellParentDownstair";
         }

         return "prenventParentUpstair";
          */
          if(upstair){
              return "tellParentDownstair";
          }

          return "prenventParentUpstair";
     }

    private string isParentsAngry()
    {
        /* 
        get parents' state
        if(angry){
            return "parentsAngry";
        }

        return "parentsNotAngry";
         */
         if(angry){
             return "parentsAngry";
         }
         return "parentsNotAngry";
    }

    private string FirstDecision ()
    {
        string shouldInteractive = "cannot interactive";
        InteractiveObject brother;
        GlobalController.InteractiveObjectDictionary.TryGetValue("Brother", out brother); 
        if(brother != null) shouldInteractive = "it should can interactive";
        Debug.Log(shouldInteractive);
        if(GlobalController.CurrentPlayer.CurrentWaypoint == MyCharacter.CurrentWaypoint)
        {
            return "talk";
        }
        if(catched){
            return "kidCatched";
        }

        //if the brother is angry (relationship is greater than 50)
        if (relationship > 50)
        {
            return "notYetAngry";
        }

        return "Engaged";
    }
    
}