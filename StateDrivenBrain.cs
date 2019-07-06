using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GP;
using PC;


public class StateDrivenBrain : BasicAIController{
    private int healthLevel;
    protected float thinkInterval = 0.25f;
    [HideInInspector]
    public UnityEngine.AI.NavMeshAgent navmeshAgent;
    public enum TacticalStates { Goto, Animate, UseSmartObject };
    [HideInInspector]
    public FSM<TacticalStates> tacticalStateMachine;
    public bool tacticalStateActive = true;
    public bool displayFSMTransitions = false;
    private List<Action> actions;
    [HideInInspector]
    public Action currentAction;

    [HideInInspector]
    public Stack<Action> plan;
    public Transform hand;
    [HideInInspector]
    public Ws startWS;
    private List<Goal> goals;
    [HideInInspector]
    public Goal currentGoal;
    public GameObject House;
    public GameObject Nails;
    public GameObject Weapon,bear;
    private TargetTrackingManager ttm;
    protected void Awake()
    {
        ttm = GetComponent<TargetTrackingManager>();
        base.Awake();

        actions = new List<Action>();
        startWS = new Ws();
        //set the worlds states for the start of the game
        startWS.SetVal(Wstates.KnowledgeOfTree, true);
        startWS.SetVal(Wstates.KnowledgeOfNails, true);
        startWS.SetVal(Wstates.KnowledgeOfHouse, true);
        startWS.SetVal(Wstates.KnowledgeOfTent, true);
        startWS.SetVal(Wstates.KnowledgeOfWeapon, true);


        // creating all of the actions that the AI will need to complete its neccassary goal. each action will be initilised with the name, cost, object and type of action. 
        //Along with its preconditions and effects to the world state.

        //Action being created 
        Action GetWood = new GetWood("GetWood", 1, this, TacticalStates.Goto);
        GetWood.SetPreCondition(Wstates.KnowledgeOfTree, true);
        GetWood.SetEffect(Wstates.LocatedAtTree, true);
        GetWood.destination = GameObject.FindGameObjectWithTag("Tree").transform;
        actions.Add(GetWood);
        //Action being created 
        Action CutTree = new CutTree("CutTree", 1, this, TacticalStates.Goto);
        CutTree.SetPreCondition(Wstates.KnowledgeOfTree, true);
        CutTree.SetPreCondition(Wstates.LocatedAtTree, true);
        CutTree.SetEffect(Wstates.HasWood, true);
        CutTree.destination = GameObject.FindGameObjectWithTag("Tree").transform;
        actions.Add(CutTree);
        //Action being created 
        Action getNails = new GetNails("GoToNails", 1, this, TacticalStates.Goto);
        getNails.SetPreCondition(Wstates.HasWood, true);
        getNails.SetPreCondition(Wstates.KnowledgeOfNails, true);
        getNails.SetEffect(Wstates.LocatedAtNails, true);
        getNails.destination = GameObject.FindGameObjectWithTag("Nails").transform;
        actions.Add(getNails);
        //Action being created 
        Action pickUpNails = new PickupNails("Getting nails", 1,this, TacticalStates.Goto);
        pickUpNails.SetPreCondition(Wstates.KnowledgeOfNails, true);
        pickUpNails.SetPreCondition(Wstates.LocatedAtNails, true);
        pickUpNails.SetEffect(Wstates.HasNails, true);
        pickUpNails.destination = GameObject.FindGameObjectWithTag("Nails").transform;
        actions.Add(pickUpNails);
        //Action being created 
        Action goHouse = new GoHouse("GoTo House", 1, this, TacticalStates.Goto);
        goHouse.SetPreCondition(Wstates.KnowledgeOfHouse, true);
        goHouse.SetPreCondition(Wstates.HasNails, true);
        goHouse.SetPreCondition(Wstates.HasWood, true);
        goHouse.SetEffect(Wstates.LocatedAtHouse, true);
        goHouse.destination = GameObject.FindGameObjectWithTag("house").transform;
        actions.Add(goHouse);
        //Action being created 
        Action BuildHouse = new buildHouse("Build house", 1, this, TacticalStates.Goto);
        goHouse.SetPreCondition(Wstates.KnowledgeOfHouse, true);
        BuildHouse.SetPreCondition(Wstates.HasNails, true);
        BuildHouse.SetPreCondition(Wstates.HasWood, true);
        BuildHouse.SetPreCondition(Wstates.LocatedAtHouse, true);
        BuildHouse.SetEffect(Wstates.HouseIsBuilt, true);
        BuildHouse.destination = GameObject.FindGameObjectWithTag("house").transform;
        actions.Add(BuildHouse);
        //Action being created 
        Action GoToTent = new GoToTent("Tent", 1, this, TacticalStates.Goto);
        GoToTent.SetPreCondition(Wstates.KnowledgeOfTent,true);
        GoToTent.SetPreCondition(Wstates.HouseIsBuilt, true);
        GoToTent.SetEffect(Wstates.LocatedAtTent, true);
        GoToTent.destination = GameObject.FindGameObjectWithTag("Tent").transform;
        actions.Add(GoToTent);
        //Action being created 
        Action GoToWeapon = new GetWeapon("Weapon", 1, this, TacticalStates.Goto);
        GoToWeapon.SetPreCondition(Wstates.KnowledgeOfWeapon, true);
        GoToWeapon.SetPreCondition(Wstates.CanSeeBear, true);
        GoToWeapon.SetEffect(Wstates.HasWeapon, true);
        GoToWeapon.destination = GameObject.FindGameObjectWithTag("Weapon").transform;
        actions.Add(GoToWeapon);
        //Action being created 
        Action GoBear = new GoToBear("GoBear", 1, this, TacticalStates.Goto);
        GoBear.SetPreCondition(Wstates.HasWeapon, true);
        GoBear.SetPreCondition(Wstates.CanSeeBear, true);
        GoBear.SetEffect(Wstates.LocatedAtBear, true);
        GoBear.destination = GameObject.FindGameObjectWithTag("Bear").transform;
        actions.Add(GoBear);
        //Action being created 
        Action killBear = new KillBear("killing", 1, this, TacticalStates.Goto);
        killBear.SetPreCondition(Wstates.CanSeeBear, true);
        killBear.SetPreCondition(Wstates.LocatedAtBear, true);
        killBear.SetEffect(Wstates.BearDead, true);
        actions.Add(killBear);

        goals = new List<Goal>();
        
        //creating the goals for the AI, each goal will be initilised with a priotity and the world state conditions that will need to be met, for the AI to complete the goal
        //Goal being created 
        Goal BuildGoal = new Goal(10);
        BuildGoal.condition.SetVal(Wstates.LocatedAtTree, true);
        BuildGoal.condition.SetVal(Wstates.HasWood, true);
        BuildGoal.condition.SetVal(Wstates.LocatedAtNails, true);
        BuildGoal.condition.SetVal(Wstates.HasNails, true);
        BuildGoal.condition.SetVal(Wstates.LocatedAtHouse, true);
        BuildGoal.condition.SetVal(Wstates.HouseIsBuilt, true);
        BuildGoal.condition.SetVal(Wstates.LocatedAtTent, true);
        goals.Add(BuildGoal);
        //Goal being created 
        Goal KillBear = new Goal(9);
        KillBear.condition.SetVal(Wstates.CanSeeBear, true);
        KillBear.condition.SetVal(Wstates.HasWeapon, true);
        KillBear.condition.SetVal(Wstates.BearDead, true);
        goals.Add(KillBear);



        // Build the Finite State Machine
        tacticalStateMachine = new FSM<TacticalStates>(displayFSMTransitions);
        tacticalStateMachine.AddState(new Goto<TacticalStates>(TacticalStates.Goto, this, 0f)); 
        tacticalStateMachine.AddState(new Animate<TacticalStates>(TacticalStates.Animate, this, 0f)); 
        tacticalStateMachine.AddState(new UseSmartObject<TacticalStates>(TacticalStates.UseSmartObject, this, 0f)); 

        tacticalStateMachine.AddTransition(TacticalStates.Goto, TacticalStates.Animate);
        tacticalStateMachine.AddTransition(TacticalStates.Goto, TacticalStates.UseSmartObject);
        tacticalStateMachine.AddTransition(TacticalStates.Goto, TacticalStates.Goto);
        tacticalStateMachine.AddTransition(TacticalStates.Animate, TacticalStates.Goto);
        tacticalStateMachine.AddTransition(TacticalStates.Animate, TacticalStates.UseSmartObject);
        tacticalStateMachine.AddTransition(TacticalStates.Animate, TacticalStates.Animate);
        tacticalStateMachine.AddTransition(TacticalStates.UseSmartObject, TacticalStates.Goto);
        tacticalStateMachine.AddTransition(TacticalStates.UseSmartObject, TacticalStates.Animate);
        tacticalStateMachine.AddTransition(TacticalStates.UseSmartObject, TacticalStates.UseSmartObject);

    }


    private Goal GetGoal()
    {
        //retun the goal based upon with goal has the highest priority
        Goal TempG = null;
        foreach(Goal G in goals)
        {
            if(TempG == null)
            {
                TempG = G;
            }
            else
            {
                if(TempG.m_priority < G.m_priority)
                {
                    TempG = G;
                }
            }
        }
        return TempG;
    }
    public void GenerateAStarPlan()
    {
       //generate the plan for the ai to complet its goals.
        Debug.Log("Generating Plan");
        AStar aStar = new AStar(actions);
        currentGoal = GetGoal();
        plan = aStar.GetPlan(startWS, currentGoal);
        if(plan == null) // if the plan is not assigned, set the goal to its defult goal 
        {
            Debug.Log("Assigning Defult Goal");
            currentGoal = goals[1]; 
            plan = aStar.GetPlan(startWS, currentGoal);
            foreach(Action a in plan)
            {
                Debug.Log("Action: " + a.Name); // ouput the ations within the plan
            }
            return;
        }
        foreach(Action a in plan)
        {
            Debug.Log("Action: " + a.Name); // output the actions within the plan 
        }
        currentAction = plan.Pop();
        tacticalStateMachine.SetInitialState(currentAction.MoveToState);

    }

    public void Start() {
        base.Start();
       
        navmeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        // Generate the plan
        GenerateAStarPlan();
    }

    public void Update()
    {
        base.Update();
        if (tacticalStateActive & plan != null && tacticalStateMachine.CurrentState != null) {
            tacticalStateMachine.CurrentState.Act();
            CheckPlan();
            tacticalStateMachine.Check();
        }
        if (ttm.GetResponse() == TargetTrackingManager.TResponse.AttackEnemy) // ifthe AI can see the bear, manipulate the world state and re generate the plan 
        {

            startWS.SetVal(Wstates.CanSeeBear, true);
            startWS.SetVal(Wstates.KnowledgeOfTree, false);
           
            GenerateAStarPlan();
        }
      
    }

    private void CheckPlan()
    {
        if(tacticalStateMachine.CurrentState.StateFinished)
        {
            if (plan.Count == 0)
            {
                currentGoal.m_priority = 0; // if the current goal has finnished then set its priority to 0 and generate a new plan 
                GenerateAStarPlan();
            }
            if(tacticalStateMachine.currentState.ActionStatus == ActionStates.Failed)
            {
                GenerateAStarPlan(); // if the action failed in the plan then regenerate the plan 
            }
        }
        
    }


    // Transition methods for FSM. Note it is possible to transist to the same state.
    // The States themselves determine when they should transist based on their interactions with the coresponding Action.
    // The State to transist to is determined by the State of the next Action in the Stack.
    public bool GuardGotoToAnimate(State<TacticalStates> currentState) {
        return (currentState.StateFinished && plan.Peek().MoveToState == TacticalStates.Animate);
    }
    public bool GuardGotoToUseSmartObject(State<TacticalStates> currentState) {
        return (currentState.StateFinished && plan.Peek().MoveToState == TacticalStates.UseSmartObject);
    }
    public bool GuardGotoToGoto(State<TacticalStates> currentState) {
        return (currentState.StateFinished && plan.Peek().MoveToState == TacticalStates.Goto);
    }
    public bool GuardAnimateToGoto(State<TacticalStates> currentState) {
        return (currentState.StateFinished && plan.Peek().MoveToState == TacticalStates.Goto);
    }
    public bool GuardAnimateToUseSmartObject(State<TacticalStates> currentState) {
        return (currentState.StateFinished && plan.Peek().MoveToState == TacticalStates.UseSmartObject);
    }
    public bool GuardAnimateToAnimate(State<TacticalStates> currentState) {
        return (currentState.StateFinished && plan.Peek().MoveToState == TacticalStates.Animate);
    }
    public bool GuardUseSmartObjectToGoto(State<TacticalStates> currentState) {
        return (currentState.StateFinished && plan.Peek().MoveToState == TacticalStates.Goto);
    }
    public bool GuardUseSmartObjectToAnimate(State<TacticalStates> currentState) {
        return (currentState.StateFinished && plan.Peek().MoveToState == TacticalStates.Animate);
    }
    public bool GuardUseSmartObjectToUseSmartObject(State<TacticalStates> currentState) {
        return (currentState.StateFinished && plan.Peek().MoveToState == TacticalStates.UseSmartObject);
    }


    // Ensure current tactical state is notified when a trigger is entered
    protected virtual void OnTriggerEnter(Collider collider) {
        tacticalStateMachine.CurrentState.OnStateTriggerEnter(collider);
    }

  

   
}
