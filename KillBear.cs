using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GP
{
    public class KillBear : Action
    {
        float timeToAction = 4f;
        public KillBear(string name, int cost, StateDrivenBrain brain, StateDrivenBrain.TacticalStates moveToState) : base(name, cost, brain, moveToState) { }
        public override ActionStates Initialise()
        {
            Debug.Log("Start Action : Kill");
            return ActionStates.Running;
        }
        public override ActionStates Update()
        {
            timeToAction -= Time.deltaTime; // decrease the time of action 
            if (timeToAction <= 0)// if the time of the action is equal to or less than 0  then return succes else return running 
            {
                Debug.Log("bearKilled");
                return ActionStates.Success;
            }
            else
            {
                Debug.Log("Attacking");
                return ActionStates.Running;
            }
        }
        public override void CleanUp()
        {
            Debug.Log("Bear Dead");
            brain.bear.SetActive(false);

        }
    }
}