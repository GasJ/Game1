using System.Collections.Generic;
using UnityEngine;

public class TestAIController : AIController
{
    public GameObject upWaypoint;
    public GameObject downWaypoint;

    public GameObject[] waypointList;

    public bool isPursuing;

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
                    Nodes =new List<DecisionTree>
                    {
                        new DecisionTree
                        {
                            Name ="Up",
                            MyAction = () => { MyCharacter.moveToDeirection(Utility.Direction.Up); }
                        },
                        new DecisionTree
                        {
                            Name="Down",
                            MyAction = () => { MyCharacter.moveToDeirection(Utility.Direction.Down); }
                        },
                        new DecisionTree
                        {
                            Name ="Left",
                            MyAction = () => { MyCharacter.moveToDeirection(Utility.Direction.Left); }
                        },
                        new DecisionTree
                        {
                            Name ="Right",
                            MyAction = () => { MyCharacter.moveToDeirection(Utility.Direction.Right); }
                        },
                    },
                    MyPredicate = PatrolDirection
                },
                new DecisionTree
                {
                    Name="Check",
                    Nodes =new List<DecisionTree>
                    {
                        new DecisionTree
                        {
                            Name ="Up",
                            MyAction = () => { MyCharacter.moveToDeirection(Utility.Direction.Up); }
                        },
                        new DecisionTree
                        {
                            Name="Down",
                            MyAction = () => { MyCharacter.moveToDeirection(Utility.Direction.Down); }
                        },
                        new DecisionTree
                        {
                            Name ="Left",
                            MyAction = () => { MyCharacter.moveToDeirection(Utility.Direction.Left); }
                        },
                        new DecisionTree
                        {
                            Name ="Right",
                            MyAction = () => { MyCharacter.moveToDeirection(Utility.Direction.Right); }
                        },
                    },
                    MyPredicate = CheckDirection
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

    private void catchKid()
    {

        var dire = MyCharacter.CurrentWaypoint.GetComponent<Waypoint>().move(GlobalController.CurrentPlayer.CurrentWaypoint.GetComponent<Waypoint>());
        MyCharacter.moveToDeirection(dire);

    }

    private void catchKid_old()
    {
        GlobalController.CurrentPlayer.moveToWaypoint(GlobalController.CurrentPlayer.InitialWaypoint,
                        GlobalController.CurrentPlayer.Direction, Utility.MoveMode.Teleport);
        MyCharacter.MyActionController.notifyMovingFinished();
        GlobalController.CurrentPlayer.MyActionController.notifyMovingFinished();
        Utility.ShowDialog(
            "You get caught!\n"
            + "You were sent back to your room.",
            null,
            null
            );
    }

    private bool _isUp = false;
    private bool _isPatroling = true;
    private string PatrolDirection()
    {
        if (_isPatroling)
        {
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
                return "Up";
            }
            else
            {
                return "Down";
            }
        }
        else
        {
            if (MyCharacter.CurrentWaypoint == upWaypoint)
            {
                _isPatroling = true;
                _isUp = false;
                return "Down";
            }
            if (MyCharacter.CurrentWaypoint == waypointList[4])
            {
                return "Right";
            }
            if (MyCharacter.CurrentWaypoint == waypointList[5])
            {
                _isUp = false;
                return "Down";
            }
            if (!_isUp)
            {
                if (MyCharacter.CurrentWaypoint == waypointList[1])
                {
                    _isUp = true;
                    return "Down";
                }
                return "Right";

            }
            return "Down";
        }
    }

    private bool _isLeft = false;
    private string CheckDirection()
    {
        if (_isPatroling)
        {
            _isLeft = false;
        }
        _isPatroling = false;
        if (MyCharacter.CurrentWaypoint == waypointList[0] || MyCharacter.CurrentWaypoint == waypointList[1]
            || MyCharacter.CurrentWaypoint == waypointList[2] || MyCharacter.CurrentWaypoint == waypointList[3])
        {
            _isLeft = false;
            return "Left";
        }
        if (MyCharacter.CurrentWaypoint == waypointList[5])
            _isLeft = true;
        if (MyCharacter.CurrentWaypoint == waypointList[6])
        {
            //get the sink's open value to verify whether it is open
            InteractiveObject sink;
            GlobalController.InteractiveObjectDictionary.TryGetValue("Washroom/Double_sink", out sink);
            ((SwitchableObject)sink)._isOpen = false;

            //get the bathtub's open value to verify whether it is open
            InteractiveObject bath;
            GlobalController.InteractiveObjectDictionary.TryGetValue("Washroom/Bathtub", out bath);
            ((SwitchableObject)bath)._isOpen = false;
            return "Right";
        }
        if (_isLeft)
        {
            return "Left";
        }
        return "Up";
    }

    private string DecisionMake()
    {
        if(GlobalController.GlobalGameEventHolder["Chaos"]>3)
        return "Catch";

        //if you can catch the kid, or see the kid
        if (CanCatch())
        {
            return "Catch";
        }

        if (waterSound())
        {
            return "Check";
        }
        return "Patrol";
    }

    private bool waterSound()
    {
        //get the sink's open value to verify whether it is open
        InteractiveObject sink;
        GlobalController.InteractiveObjectDictionary.TryGetValue("Washroom/Double_sink", out sink);
        bool sinkOpen = ((SwitchableObject)sink)._isOpen;

        //get the bathtub's open value to verify whether it is open
        InteractiveObject bath;
        GlobalController.InteractiveObjectDictionary.TryGetValue("Washroom/Bathtub", out bath);
        bool bathOpen = ((SwitchableObject)bath)._isOpen;

        return sinkOpen && bathOpen;
    }

    bool CanCatch()
    {
        GameObject kidPosition = GlobalController.CurrentPlayer.CurrentWaypoint;
        //Utility.Direction kidDirect =  GlobalController.CurrentPlayer.Direction;
        //this is the point my character is on.
        GameObject myPoint = MyCharacter.CurrentWaypoint;
        Utility.Direction myDirect = MyCharacter.Direction;

        if (MyCharacter.CanMove)
        {
            GameObject destination = myPoint.GetComponent<Waypoint>().getDestination(myDirect);
            if (destination == kidPosition)
            {
                return true;
            }
        }
        return false;
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
