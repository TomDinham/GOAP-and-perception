using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GP
{

    public class buildHouse : Action {

        // Use this for initialization
        float timeToAction = 4f;
        public buildHouse(string name, int cost, StateDrivenBrain brain, StateDrivenBrain.TacticalStates moveToState) : base(name, cost, brain, moveToState) { }
        public override ActionStates Initialise()
        {
            Debug.Log("Start Action : Building house");
            return ActionStates.Running;
        }
        public override ActionStates Update()
        {
            timeToAction -= Time.deltaTime; // decrease the time of action 
            if (timeToAction <= 0)// if the time of the action is equal to or less than 0  then return succes else return running 
            {
                Debug.Log("Adding Items");
                return ActionStates.Success;
            }
            else
            {
                //Debug.Log(timeToAction);
                return ActionStates.Running;
            }
        }
        public override void CleanUp()
        {

            Debug.Log("Hosue Built");
            brain.House.SetActive(true);
        }
    }
}
