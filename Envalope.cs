using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PC
{


    public class Envalope 
    {
        private List<Event> events;
        public GameObject source;
        public float pValue;
        public bool markEnvalopeForDeletion = false;

        public Envalope(Stimulus stim, Configuration config)
        {
            // assign the variables
            source = stim.m_source;
            events = new List<Event>();
            markEnvalopeForDeletion = false;
            Add(stim, config);
        }
        public void Add(Stimulus stim, Configuration config)
        {
            events.Add(new Event(stim, config));
            Process(); // add a new event with the given stimulus and configuration
            
        }
        public bool Exists(Stimulus stim)
        {
            return events.Exists(e => e.m_source == stim.m_source && e.m_type == stim.m_type); // if the stimuls exists in the event
        }
        public void Process()
        {
            pValue = 0f;
            foreach(Event E in events)
            {
                E.Process(); // process the event
                if(!E.markEventForDeletion) // if the event is not marked for deletion 
                {
                    pValue += E.eventPerceptionValue; // add to the perception value 

                }
            }
            events.RemoveAll(e => e.markEventForDeletion == true); // remove all events that are marked for deleition   
            if(events.Count == 0) // if there are no events left then mark the envalope for deletion
            {
                markEnvalopeForDeletion = true;
            }
        }
        public void Update(Stimulus stim)
        {
            Event foundEvent = events.Find(e => e.m_source == stim.m_source && e.m_type == stim.m_type);  //find matching events with the stimulus 
            if(foundEvent != null)
            {
                foundEvent.Update(); // if events can be found, update the envent and the process the envalope
                Process();
            }
        }
        public bool CanSeeTarget()
        {
            foreach(Event E in events)
            {
                if(E.m_type == Stimulus.StimulusTypes.VisualCanSee) // if the events stimulus is visual can see return true.
                {
                    return true;
                }
            }
            return false;
        }
    }
}