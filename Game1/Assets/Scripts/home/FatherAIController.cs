using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FatherAIController : AIController
{
    public bool knowSchedule = false; //if the brother told father your plan, he will chase you right now.

    public bool goBack = false;
    public GameObject upWaypoint;
    public GameObject downWaypoint;
    public GameObject washroom;
    public GameObject littleSonRoom;
    public GameObject oldSonRoom;
    public GameObject downStairs;
    public GameObject upStairs;
    void Start()
    {
        _myDecisionTree = new DecisionTree
        {
            Name = "Root",
            Nodes = new List<DecisionTree>
            {
                new DecisionTree
                {
                    Name="Patrol",
                    MyAction = patrol
                },
                new DecisionTree
                {
                    Name="Chase",
                    MyAction = chaseKid
                    //MyAction = () => { MyCharacter.moveToDeirection(Utility.Direction.Up); }
                },
                new DecisionTree
                {
                    Name="Check",
                    Nodes = new List<DecisionTree>
                    {
                        new DecisionTree
                        {
                            Name="checkBrother",
                            MyAction=checkBrother
                        },
                        new DecisionTree
                        {
                            Name="checkWashroom",
                            MyAction=checkWashroom
                        },
                        new DecisionTree
                        {
                            Name="checkRoom_myself",
                            MyAction=checkRoom_myself
                        },
                        new DecisionTree
                        {
                            Name="checkUp",
                            MyAction=checkUp
                        }
                    },
                    MyPredicate = checkUpstairs
                },
                new DecisionTree
                {
                    Name="Catch",
                    MyAction = catchKid
                },
            },
            MyPredicate = DecisionMake

        };



    }

    private bool _isUp = false;
    private void patrol()
    {
        //Debug.Log("patrol");
        var dire= MyCharacter.CurrentWaypoint.GetComponent<Waypoint>().move(downWaypoint.GetComponent<Waypoint>());
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

    private void checkUp()
    {
        patrol();
    }

    private void chaseKid()
    {
        var dire = MyCharacter.CurrentWaypoint.GetComponent<Waypoint>().move(GlobalController.CurrentPlayer.CurrentWaypoint.GetComponent<Waypoint>());
        MyCharacter.moveToDeirection(dire);

    }

    private void catchKid()
    {   gameObject.GetComponents<AudioSource>()[1].Play();     
        GlobalController.CurrentPlayer.moveToWaypoint(GlobalController.CurrentPlayer.InitialWaypoint,
                        GlobalController.CurrentPlayer.Direction, Utility.MoveMode.Teleport);
        
        Utility.ShowDialog(
            "You get caught!\n"
            + "You were sent back to your room.",
            new Utility.ButtonDeclaration
            {
                Text = "Restart",
                OnClickHandler = () =>
                {
                    SceneManager.LoadScene("Welcome");
                    GlobalController.IsShowingPauseMenu = false;
                    MyCharacter.MyActionController.notifyMovingFinished();
                }
            },
            new Utility.ButtonDeclaration
            {
                Text = "Okay",
                OnClickHandler = () =>
                {
                    GameObject.Find("Brother").GetComponent<BrotherAIController>().catched = true;
                    if(isChase) isChase = false;
                    if(isCheck) isCheck = false;
                    goBack = true;
                    knowSchedule = false;
                    MyCharacter.MyActionController.notifyMovingFinished();
                }
            }
            );

            
        
    }


    private bool isChase = false;
    public bool isCheck = false;
    private string DecisionMake()
    {
        if(GameObject.Find("Brother").GetComponent<BrotherAIController>().catched)
        {
            isChase=false;
        }
        //if you can catch the kid, or see the kid
        if (CanCatch())
        {
            GameObject.Find("Brother").GetComponent<BrotherAIController>().catched = true;
            return "Catch";
        }


        if (GlobalController.GlobalGameEventHolder["Chaos"] > 2)
        {
            GameObject.Find("Brother").GetComponent<BrotherAIController>().angry = true;
            isCheck=true;
        }
        if(isCheck)
            return "Check";

        //if player go downstairs AI will chase the player
        if (GlobalController.CurrentPlayer.CurrentWaypoint == downStairs)
        {
            isChase = true;
        }
        if (GlobalController.CurrentPlayer.CurrentWaypoint == upStairs)
        {
            isChase = false;
        }

        if (knowSchedule)
        {
            isChase = true;
        }

        if (isChase){
            //GameObject.Find("Brother").GetComponent<BrotherAIController>().angry = true;
            return "Chase";
        }
            
        //if too much chaos upstairs AI will chase the player
        if (GlobalController.GlobalGameEventHolder["Chaos"] > 10)
        {
            //GameObject.Find("Brother").GetComponent<BrotherAIController>().angry = true;
            return "Chase";
        }

        return "Patrol";
    }

    bool CanCatch()
    {
        GameObject kidPosition = GlobalController.CurrentPlayer.CurrentWaypoint;
        //Utility.Direction kidDirect =  GlobalController.CurrentPlayer.Direction;
        //this is the point my character is on.
        GameObject myPoint = MyCharacter.CurrentWaypoint;
        Utility.Direction myDirect = MyCharacter.Direction;
        var dire = MyCharacter.CurrentWaypoint.GetComponent<Waypoint>().move(GlobalController.CurrentPlayer.CurrentWaypoint.GetComponent<Waypoint>());
        
        if (myPoint == kidPosition)
            return true;
        GameObject destination = myPoint.GetComponent<Waypoint>().getDestination(dire);
        if (destination == kidPosition)
            return true;
        return false;
    }


    private string checkUpstairs()
    {
        if(GameObject.Find("Brother").GetComponent<BrotherAIController>().crying == true){
            return "checkBrother";
        }
        if(GameObject.Find("Double_sink").GetComponent<SwitchableObject>()._isOpen == true
        && GameObject.Find("Bathtub").GetComponent<SwitchableObject>()._isOpen == true){
            return "checkWashroom";
        }
        if(GameObject.Find("TV_table").GetComponent<SwitchableObject>()._isOpen == true){
            return "checkRoom_myself";
        }
        return "checkUp";
    }

    private void checkRoom_myself()
    {
        var dire = MyCharacter.CurrentWaypoint.GetComponent<Waypoint>().move(oldSonRoom.GetComponent<Waypoint>());
        MyCharacter.moveToDeirection(dire);
        if (MyCharacter.CurrentWaypoint == oldSonRoom)
        {
            bool tvOpen = GameObject.Find("TV_table").GetComponent<SwitchableObject>()._isOpen;
            if (tvOpen)
            {
                GameObject.Find("TV_table").GetComponent<SwitchableObject>()._isOpen = false;
                GameObject.Find("Brother").GetComponent<BrotherAIController>().angry = false;
                GlobalController.GlobalGameEventHolder["Chaos"] -= 3;
            }
                isCheck = false;
        }
    }

    private void checkBrother()
    {
        var dire = MyCharacter.CurrentWaypoint.GetComponent<Waypoint>().move(littleSonRoom.GetComponent<Waypoint>());
        MyCharacter.moveToDeirection(dire);
        if (MyCharacter.CurrentWaypoint == littleSonRoom)
        {
            //get weather brother is crying 
            bool stillCry = GameObject.Find("Brother").GetComponent<BrotherAIController>().crying;
            if (stillCry)
            {
                Utility.ShowDialog("What the fuck did you do to your brother?\nYou will pay for what you've done\n - your father",
                        null,
                        null);
                GameObject.Find("Brother").GetComponent<BrotherAIController>().crying = false;
                GameObject.Find("Brother").GetComponent<BrotherAIController>().angry = false;
                GlobalController.GlobalGameEventHolder["Chaos"] -= 3;
            }
            isCheck = false;
            isChase = true;
        }
    }

    private void checkWashroom()
    {
        var dire = MyCharacter.CurrentWaypoint.GetComponent<Waypoint>().move(washroom.GetComponent<Waypoint>());
        MyCharacter.moveToDeirection(dire);
        if (MyCharacter.CurrentWaypoint == washroom)
        {
            //get the sink's open value to verify whether it is open
            InteractiveObject sink;
            GlobalController.InteractiveObjectDictionary.TryGetValue("Washroom/Double_sink", out sink);
            if (((SwitchableObject)sink)._isOpen)
            {
                ((SwitchableObject)sink)._isOpen = false;
                GlobalController.GlobalGameEventHolder["Chaos"] -= 2;
            }
            //get the bathtub's open value to verify whether it is open
            InteractiveObject bath;
            GlobalController.InteractiveObjectDictionary.TryGetValue("Washroom/Bathtub", out bath);

            if (((SwitchableObject)bath)._isOpen)
            {
                ((SwitchableObject)bath)._isOpen = false;
                GlobalController.GlobalGameEventHolder["Chaos"] -= 2;
            }
            isCheck = false;
            GameObject.Find("Brother").GetComponent<BrotherAIController>().angry = false;
        }
    }

    //_myDecisionTree = new DecisionTree
    //{
    //    TrueNode = new DecisionTree
    //    {
    //        MyAction = (obj) => { MyCharacter.moveToDeirection(Utility.Direction.Down); }
    //    },
    //    FalseNode = new DecisionTree
    //    {
    //        MyAction = (obj) => { MyCharacter.moveToDeirection(Utility.Direction.Up); }
    //    },
    //    MyPredicate = (obj) => { return MyCharacter.CurrentWaypoint == upWaypoint; }
    //};
}
