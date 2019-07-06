using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace GP
{

    public enum Wstates { idle,KnowledgeOfTree, KnowledgeOfNails, KnowledgeOfWeapon, KnowledgeOfHouse,KnowledgeOfTent, 
    HasWood, HasWeapon,HasNails, LocatedAtTree,LocatedAtNails,LocatedAtHouse, LocatedAtTent,LocatedAtBear, // enum with the list of world states that will effect the players action 
    LocatedAtWeapon, CanSeeBear,BearDead,HouseIsBuilt}
    public class Ws 
    {
        private Dictionary<Wstates, WoldType> states;


        public Ws()
        {
            states = new Dictionary<Wstates, WoldType>(); // create a new dictionary for the world states 
            foreach(Wstates w in Enum.GetValues(typeof(Wstates))) // for each of the world states add them in with there values
            {
                states.Add(w, new WoldType());
            }
        }
        public bool GetVal(Wstates w)
        {
            return states[w].Value; // return the current value of the world state 
        }
        public void SetVal(Wstates w, bool val)
        {
            states[w].Value = val; // set the value to the world state and assing it a true value into its is enabled boolean variable 
            states[w].IsEnabled = true;
        }
        public bool Equal(Ws WS)
        {
            foreach(Wstates w in Enum.GetValues(typeof(Wstates)))
            {
                if(states[w].Value != WS.states[w].Value)// cheack that the world state value is equal to its orginal value
                {
                    return false;
                }
            }
            return true;
        }
        public bool EnabledEqual(Ws WS)
        {
            foreach(Wstates w in Enum.GetValues(typeof(Wstates)))
            {
                if(WS.states[w].IsEnabled)// cheak that the world state is enabled variable is equal to is orginal value 
                {
                    if (states[w].Value != WS.states[w].Value)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public bool HasAchived(Ws GL)
        {
            return EnabledEqual(GL); // return if the world state is equal 
        }
        public bool PreConditionsMeet(Ws cond)
        {
            return EnabledEqual(cond); //return if the world state is equal
        }
        public Ws GetWSEffected(Ws effects)
        {
            Ws worldstate = new Ws();
            foreach(Wstates w in Enum.GetValues(typeof(Wstates))) // for each of the worldstates  assign the effed world states 
            {
                worldstate.states[w].Value = states[w].Value;
                worldstate.states[w].IsEnabled = states[w].IsEnabled;
            }
            foreach(Wstates w in Enum.GetValues(typeof(Wstates)))
            {
                if(effects.states[w].Value) // if the world state value is true assign the variables for the worldstate to true 
                {
                    worldstate.states[w].Value = true;
                    worldstate.states[w].IsEnabled = true;
                }
            }
            return worldstate;
        }
        public int GetNumMisMatchStates(Ws world)
        {
            int mis = 0;
            foreach(Wstates w in Enum.GetValues(typeof(Wstates)))
            {
                if(world.states[w].IsEnabled) // gain the number of states that do not match their original value
                {
                    if (states[w].Value != world.states[w].Value)
                    {
                        mis++;
                    }
                }
            }
            return mis;
        }
        public void Effect(Ws world)
        { // apply effects wot the worldstate passed in 
            foreach(Wstates W in Enum.GetValues(typeof(Wstates)))
            {
                if(world.states[W].Value)
                {
                    states[W].Value = true;

                }
            }
        }
        public void Display(string mess)
        {
            Console.WriteLine(mess);
            foreach(KeyValuePair<Wstates,WoldType>w in states)
            {
                Debug.Log("Key: " + w.ToString() + ":" + w.Value.Value); //display the world states
                Console.Write("Key:{0,15} ", w.Key.ToString());
                Console.WriteLine(":{0}  ", w.Value.Value);
            }
        }
    }
    public class WoldType
    {
        public bool Value;
        public bool IsEnabled;
        public WoldType()
        {
            Value = false;
            IsEnabled = false;
        }
    }
}