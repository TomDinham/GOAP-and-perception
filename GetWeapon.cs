using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GP
{
    public class GetWeapon : Action
    {

        public GetWeapon(string name, int cost, StateDrivenBrain brain, StateDrivenBrain.TacticalStates moveToState) : base(name, cost, brain, moveToState) { }
        public override ActionStates Initialise()
        {
            Debug.Log("Start Action : Goto Weapon");
            return ActionStates.Running;
        }
        public override ActionStates Update()
        {
            if (Vector3.Distance(brain.transform.position, destination.position) < 3f) // if the AI is less than 3 units away from the target location return success, else return running.
            {
                return ActionStates.Success;
            }
            else
            {
                return ActionStates.Running;
            }
        }
        public override void CleanUp()
        {
            Debug.Log("Have Weapon");
            brain.Weapon.SetActive(false);
        }
    }
}

