using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PC
{
    public class Event : Stimulus
    {
        public float eventPerceptionValue;
        public Configuration configuration;
        public float life;
        public float birth;
        public bool markEventForDeletion;

        public Event(Stimulus stim, Configuration config )
        {
            //initialise variables
            this.m_type = stim.m_type;
            this.m_source = stim.m_source;
            this.m_location = stim.m_location;
            this.m_Direction = stim.m_Direction;
            this.m_Radius = stim.m_Radius;
            this.configuration = config;
            this.birth = Time.time;
            this.markEventForDeletion = false;

        }
        public void Process()
        {
            life = Time.time - birth;
            eventPerceptionValue = configuration.GetPerception(life);
            if (eventPerceptionValue < 0f)
            {
                markEventForDeletion = true; // if the life of the stimulus is less than 0 then mark the event for deletion
            }
        }
        public void Update()
        {
            if(life > configuration.Decay)
            {
                birth = Time.time - configuration.Decay; // if the life is more htan the decay value, then assing the birth time.
            }
        }
    }
}