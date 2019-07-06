using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GP {
    
    public abstract class Action: GOAPAction { // base class for all the actions 
     

        protected StateDrivenBrain brain;
        public Transform destination;
        public GameObject interactObject;
        protected Vector3 moveRotation;
        protected Quaternion lookAtRotation;
        protected float angleToTurn;
        protected float angleOffset;
        public Action(string name, int cost, StateDrivenBrain brain, StateDrivenBrain.TacticalStates moveToState):base(name,cost,moveToState) {
            this.brain = brain;
            angleOffset = 0f;
        }
 
        protected bool FaceTarget(Vector3 target)
        {
            // turn the AI to face towards the target 
            if (target != null) {
                TurnToFace(target);
               
                moveRotation = Quaternion.Slerp(brain.gameObject.transform.rotation, lookAtRotation, brain.turnSpeed * Time.deltaTime).eulerAngles;
                
                moveRotation.x = 0;
                moveRotation.z = 0;
                
                angleToTurn = lookAtRotation.eulerAngles.y - brain.gameObject.transform.rotation.eulerAngles.y;
                
                if (Mathf.Abs(angleToTurn) < 5.0f) {
                
                    brain.animator.SetFloat("Rotation", 0f);
                    return true;
                }
                else {
                   
                    if (angleToTurn < 0f) {
                        brain.animator.SetFloat("Rotation", -brain.turnSpeed);
                    }
                    else {
                        brain.animator.SetFloat("Rotation", brain.turnSpeed);
                    }
                }
            }
            else {
                brain.animator.SetFloat("Rotation", 0f);
            }
            return false;
        }
       
        protected void TurnToFace(Vector3 target) {
            if (target != null) {
               
                Vector3 lookRotation = Quaternion.LookRotation(target - brain.gameObject.transform.position).eulerAngles;
                lookRotation.x = 0;
                lookRotation.z = 0;
              
                lookAtRotation = Quaternion.Euler(lookRotation);
            }
        }
    }
}