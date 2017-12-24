using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAIController2 : AIController
{

    public GameObject upWaypoint;

    

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
                    Name="Check",
                    //MyAction = () => { MyCharacter.moveToDeirection(Utility.Direction.Down); }
                },
                new DecisionTree
                {
                    Name="Pursue",
                    //MyAction = () => { MyCharacter.moveToDeirection(Utility.Direction.Up); }
                },
            },
            MyPredicate = decisionMake

        };


        
    }

    private void patrol()
    {
        throw new NotImplementedException();
    }

    private string decisionMake()
    {
        return "Patrol";
    }

    
}
