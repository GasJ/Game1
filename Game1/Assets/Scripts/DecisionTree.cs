using System;
using System.Collections.Generic;
using UnityEngine.Events;

public class DecisionTree
{
    /// <summary>
    /// Name of current node
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// List of child nodes
    /// </summary>
    public List<DecisionTree> Nodes { get; set; }
    /// <summary>
    /// The delegate to determine which child node should be entered
    /// </summary>
    public Func<string> MyPredicate { get; set; }
    /// <summary>
    /// The delegate of action to be executed of this node
    /// </summary>
    public Action MyAction { get; set; }

    private bool _isLeaf { get { return Nodes == null || Nodes.Count == 0; } }

    /// <summary>
    /// Start to process from this node
    /// </summary>
    public void act()
    {
        if (_isLeaf) MyAction();
        else this[MyPredicate()].act();
    }

    /// <summary>
    /// Get the child node with given name
    /// </summary>
    /// <param name="name">The name of child node</param>
    /// <returns>The child node with given name</returns>
    public DecisionTree this[string name]
    {
        get
        {
            return Nodes.Find((n) => { return n.Name == name; });
        }
        set
        {
            var match = Nodes.Find((n) => { return n.Name == name; });
            if (match == null)
            {
                var tree = value;
                tree.Name = name;
                Nodes.Add(tree);
            }
            else
            {
                match = value;
            }
        }
    }
}