using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GP
{


    public class Goal //  base class for the goal 
    {

        public Ws condition;
        public int m_priority;
        public Goal(int priority)
        {
            m_priority = priority;
            condition = new Ws();
        }

    }
}