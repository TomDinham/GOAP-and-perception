using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GP
{


    public class PickupNails : Action
    {
        float timeToAction = 1f;
        public PickupNails(string name, int cost, StateDrivenBrain brain, StateDrivenBrain.TacticalStates moveToState) : base(name, cost, brain, moveToState) { }
        public override ActionStates Initialise()
        {
            Debug.Log("Start Action : Picking up nails");
            return ActionStates.Running;
        }
        public override ActionStates Update()
        {
            timeToAction -= Time.deltaTime;  // decrease the time of action 
            if (timeToAction <= 0) // if the time of the action is less than or equal to 0 then return success 
            {
                Debug.Log("Getting Nail");
                return ActionStates.Success;
            }
            else
            {
                
                return ActionStates.Running; //  if the action has not finnished return that it is still running
            }
        }
        public override void CleanUp()
        {

            Debug.Log("Nails Added"); 
            brain.Nails.SetActive(false);
        }
    }
}
