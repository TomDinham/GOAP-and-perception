using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PC
{

    public class PManager : MonoBehaviour
    {
        private List<GameObject> registrants;
        private List<Stimulus> stimBuffer;
        private List<GameObject> characters;
        [HideInInspector]
        public List<Configuration> configs;

        void Awake()
        {
            //initilise the variables
            registrants = new List<GameObject>();
            stimBuffer = new List<Stimulus>();
            characters = GetAllCharacters();
            configs = Configuration.GetConfiguration();
        }
        void Start()
        {
            //on the start of the program run the process coroutine
            StartCoroutine(Process());
        }
        private List<GameObject>GetAllCharacters()
        {
            GameObject[] T_bear = GameObject.FindGameObjectsWithTag("Bear"); // find the bear objects
            List<GameObject> characters = new List<GameObject>(T_bear); //  add the bears into a list called characters
            characters.AddRange(T_bear);
            return characters;
        }
        public void Register(GameObject NPC)
        {
            registrants.Add(NPC); //  add the NPC to the registance list
        }
        public void AcceptStim(Stimulus stim)
        {
            stimBuffer.Add(stim); // ddd the stimulus to the list
        }
        IEnumerator Process()
        {
            yield return new WaitForSeconds(1.0f);
            GenerateVisualStimulations(); 
            ProcessStimBuffer();
            StartCoroutine(Process());
        }
        private void ProcessStimBuffer()
        {
            TargetTrackingManager TM;
            foreach(GameObject GO in registrants)
            {
                TM = GO.GetComponent< TargetTrackingManager>();
                foreach(Stimulus stim in stimBuffer)
                {
                    if(stim.m_type == Stimulus.StimulusTypes.AudioMovement)
                    {
                        float range = Mathf.Abs((stim.m_location - GO.transform.position).magnitude);
                        if(range < stim.m_Radius)// if the simulus is in range of the AI accept the stimulus
                        {
                            TM.AcceptFilteredStimulus(stim); 
                        }
                    }
                    if(stim.m_type == Stimulus.StimulusTypes.VisualCanSee)
                    {
                        TM.AcceptFilteredStimulus(stim);// if the AI can see the enemy the accept the stimulus
                    }
                }
            }
            stimBuffer.Clear();// clear the stimulation list 
        }
        private void GenerateVisualStimulations()
        {
            Vector3 direction;
            foreach(GameObject GO in registrants)
            {
                foreach(GameObject G in characters)
                {
                    if(CanSeeCharacter(GO.transform,G.transform,out direction) && GO != G) //  if the AI can see the enemy and it is not the NPC then add the stimulus to the list
                    {
                        stimBuffer.Add(new Stimulus(Stimulus.StimulusTypes.VisualCanSee, G, G.transform.position, direction, 0f, GO));
                    }
                }
            }
        }
        private static bool CanSeeCharacter(Transform reg, Transform Character, out Vector3 direction)
        {
            float regHeight = reg.GetComponent<CharacterController>().height; // assing the AIs height 
            Senses senses = reg.GetComponent<Senses>();//reference the sense's script 
            direction = Character.position - reg.position; // set the direction of the enemy 

            float distToTarget = Vector3.Distance(reg.position, Character.position); // set the distance to the the enemy 
            if(senses.sightRange > distToTarget) // if the sight range is more than the distance
            {
                float angle = Vector3.Angle(direction, reg.forward); // obtain the angle to the enemey 
                angle = System.Math.Abs(angle); // set the angle 
                if (angle < (senses.viewingAngle / 2)) //if the angle is less than the sight range 
                {
                     // set up the variable to save the data collected when the raycast hits
                    RaycastHit HitData;
                     // set up the layers to detect
                    LayerMask playerMask = 1 << 8;
                    LayerMask coverMask = 1 << 9;
                    LayerMask aiMask = 1 << 10;
                    LayerMask mask = coverMask | playerMask | aiMask;

                    float targetHeight = (Character.GetComponent<CharacterController>().height / 1.25f); // set the targets height
                    Vector3 regEyePos = new Vector3(reg.position.x, reg.position.y + regHeight, reg.position.z); // set the AIs eye position 
                    Vector3 targetBodyPos = new Vector3(Character.transform.position.x, Character.transform.position.y + targetHeight, Character.transform.position.z); // set the targets body position
                    Vector3 rayDirection = (targetBodyPos - regEyePos).normalized; // set the raycast direction 
                    bool hit = Physics.Raycast(regEyePos, rayDirection, out HitData, senses.sightRange, mask.value);// shoot a raycast and of hits set the hit variable to true
                    Debug.DrawRay(regEyePos, rayDirection * senses.sightRange, Color.cyan);
                    if(hit)
                    {
                        if (HitData.collider.tag == "Bear") // if the raycast hit the bear return true
                        {
                            return true;
                        }
                       
                    }
                }
                
            }
            return false;
        }
        
    }
}