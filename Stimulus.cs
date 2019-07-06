using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PC
{   
    public class Stimulus
    {
        public enum StimulusTypes { AudioMovement, VisualCanSee}
        public StimulusTypes m_type;
        public GameObject m_source;
        public Vector3 m_location;
        public Vector3 m_Direction;
        public float m_Radius;
        public GameObject m_secondarySource;

        public Stimulus(StimulusTypes type, GameObject source, Vector3 location, Vector3 direction, float radius, GameObject secondarySource)
        {
            //initilise the variables
            m_type = type;
            m_source = source;
            m_location = location;
            m_Direction = direction;
            m_Radius = radius;
            m_secondarySource = secondarySource;
        }
        public Stimulus() { }
    }
}