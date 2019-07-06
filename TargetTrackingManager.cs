using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PC
{


    public class TargetTrackingManager : MonoBehaviour
    {
        public enum TResponse { DoNothing, TrackEnemy, AttackEnemy }; //enum for the AI's responce
        private const float PROCESS_TIME = 1f;
        [HideInInspector]
        public PManager perception_M;
        private List<Envalope> envalopes;

        [HideInInspector]
        public TResponse response;
        [HideInInspector]
        public GameObject Target;

        private void Awake()
        {
            //inisiating the variables
            perception_M = GameObject.FindGameObjectWithTag("GameController").GetComponent<PManager>();
            response = TResponse.DoNothing;
            Target = null;
            envalopes = new List<Envalope>();
        }
        private void Start()
        {
            perception_M.Register(this.gameObject);
            StartCoroutine(Process());
        }
        public void AcceptFilteredStimulus(Stimulus stim)
        {
            Debug.Log("type" + stim.m_type + "Source" + stim.m_source); // output the stimulus type and source
            foreach (Envalope E in envalopes)
            {
                if(E.source == stim.m_source)
                {
                    if(E.Exists(stim)) // if the stimulus exists in the current envalope envalope
                    {
                        E.Update(stim); // update the envalope and send in the currem stimulus
                    }
                    else
                    {
                        E.Add(stim, GetConfiguration(stim));//if the stimulus doesnt exist, add the stimulus into the envalope
                    }
                }
            }
            envalopes.Add(new Envalope(stim, GetConfiguration(stim))); // create and add an envalope into the envalopes List
        }
        private IEnumerator Process()
        {
            yield return new WaitForSeconds(PROCESS_TIME);
            foreach(Envalope E in envalopes)
            {
                E.Process();//process the envalope
            }
            envalopes.RemoveAll(e => e.markEnvalopeForDeletion == true);//deleate all the envalopes that are marked for deletion
            StartCoroutine(Process());
        }
        public TResponse GetResponse()
        {
            float highestPValue = 0f;
            Envalope temp = null;
            if(envalopes.Count == 0)
            {
                Target = null;
                return TResponse.DoNothing;//if no envalopes exist then there is not target and AI to do nothing
            }
            foreach(Envalope E in envalopes)
            {

                if(E.pValue >= highestPValue)
                {
                    temp = E;
                    highestPValue = E.pValue;
                }

            }
            if(temp.source.tag == "Bear")
            {
                Target = temp.source;
                if(temp.CanSeeTarget())//if the AI can see the bear tell the AI to attack the bear
                {
                    return TResponse.AttackEnemy;
                }
                else
                {
                    return TResponse.TrackEnemy;//if the AI cannot see the bear but is still reciving stimulus from the bear then track the bears location
                }
            }
            return TResponse.DoNothing;//if nothing matches do nothing
        }
        private Configuration GetConfiguration(Stimulus stim)
        {
            foreach(Configuration C in perception_M.configs)
            {
                if(C.type == stim.m_type)
                {
                    return C;
                }
            }
            return null;
        }

        
    }
}