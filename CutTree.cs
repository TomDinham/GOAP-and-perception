using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GP
{


    public class CutTree : Action
    {
        float timeToAction = 5f;
        public CutTree(string name, int cost, StateDrivenBrain brain, StateDrivenBrain.TacticalStates moveToState) : base(name, cost, brain, moveToState) { }
        public override ActionStates Initialise()
        {
            Debug.Log("Start Action : Cutting tree");
            return ActionStates.Running;
        }
        public override ActionStates Update()
        {
            timeToAction -= Time.deltaTime; // decrease the time of action 
            if(timeToAction <= 0)// if the time of the action is equal to or less than 0  then return succes else return running 
            {
                Debug.Log("Cutting Tree");
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

            Debug.Log("Wood Added");
        }
    }
}