public class AIController : ActionController
{
    protected DecisionTree _myDecisionTree { get; set; }

    protected override void act()
    {
        base.act();
        _myDecisionTree.act();
    }
}
