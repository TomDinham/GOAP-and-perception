using UnityEngine;
using System.Collections;

    // Flexible state that can manage Actions that interact with GameObjects and complete on end of animation -(if required).
    public class UseSmartObject<T> : AIState<T> {

        public UseSmartObject(T stateName, StateDrivenBrain controller, float minDuration) : base(stateName, controller, minDuration) { }


        public override void OnEnter() {
            base.OnEnter();
            actionStatus = brain.currentAction.Initialise();
        }

        public override void Act() {
            if (!animationFinished) {
               actionStatus = brain.currentAction.Update();
            }
            if (actionStatus == GP.ActionStates.Success) {
                stateFinished = true;
            }
        }

        public override void OnLeave() {
            base.OnLeave();

            brain.currentAction.CleanUp();
            // On successful completion of the task the effects are applied to the agent's WS
            if (actionStatus == GP.ActionStates.Success) {
                brain.startWS.Effect(brain.currentAction.effects);
            }
            brain.currentAction = brain.plan.Pop();
        }

        public override void OnAnimationEnded() {
            base.OnAnimationEnded();
            stateFinished = true;
            if(actionStatus == GP.ActionStates.Running){
                actionStatus = GP.ActionStates.Failed;
            }

        }
    }
