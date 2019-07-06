using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GP
{
    public class Idle : Action
    {
        public Idle(string name, int cost, StateDrivenBrain brain, StateDrivenBrain.TacticalStates moveToState) : base(name, cost, brain, moveToState) { }
        public override ActionStates Initialise()
        {
            Debug.Log("Start Action : Idle");
            return ActionStates.Running;
        }
        public override ActionStates Update()
        {
            // Action still running
            return ActionStates.Running;
        }
        public override void CleanUp()
        {
            Debug.Log("End Action : Idle");
        }
    }
}
