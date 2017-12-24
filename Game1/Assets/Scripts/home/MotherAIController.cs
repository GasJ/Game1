using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MotherAIController : AIController
{


    public GameObject upWaypoint;
    public GameObject downWaypoint;
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
                    Name="Catch",
                    MyAction = catchKid
                    //MyAction = () => { MyCharacter.moveToDeirection(Utility.Direction.Up); }
                },
            },
            MyPredicate = DecisionMake

        };



    }

    

    private bool _isUp = false;
    private void patrol()
    {
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

    private void chaseKid()
    {

        var dire = MyCharacter.CurrentWaypoint.GetComponent<Waypoint>().move(GlobalController.CurrentPlayer.CurrentWaypoint.GetComponent<Waypoint>());
        MyCharacter.moveToDeirection(dire);

    }

    private void catchKid()
    {
        gameObject.GetComponents<AudioSource>()[1].Play();
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
                    MyCharacter.MyActionController.notifyMovingFinished();
                }
            }
            );
            MyCharacter.MyActionController.notifyMovingFinished();
    }


    private bool isChase = false;
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

        if (GlobalController.GlobalGameEventHolder["Chaos"] > 10){
            GameObject.Find("Brother").GetComponent<BrotherAIController>().angry = true;
            return "Chase";
        }
            

        //if player go downstairs AI will chase the player
        if (GlobalController.CurrentPlayer.CurrentWaypoint == downStairs)
        {
            GameObject.Find("Brother").GetComponent<BrotherAIController>().angry = true;
            isChase = true;
        }

        if (GlobalController.CurrentPlayer.CurrentWaypoint == upStairs)
        {
            isChase = false;
        }
        if (isChase)
            return "Chase";

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
}
