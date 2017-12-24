using System;
using UnityEngine.Events;

public class DecisionBinaryTree
{
    public DecisionBinaryTree TrueNode { get; set; }
    public DecisionBinaryTree FalseNode { get; set; }
    public Predicate<object> MyPredicate { get; set; }
    public object PredicateParameter { private get; set; }
    public UnityAction<object> MyAction { get; set; }
    public object ActionParameter { private get; set; }

    private bool _isLeaf { get { return TrueNode == null && FalseNode == null; } }

    public void act()
    {
        if (_isLeaf) MyAction(ActionParameter);
        else (MyPredicate(PredicateParameter) ? TrueNode : FalseNode).act();
    }
}
