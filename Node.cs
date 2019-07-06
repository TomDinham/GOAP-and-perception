using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GP
{
    public class Node  // base class for the nodes 
    {
        public Ws world;
        public int g;
        public int h;
        public int f;
        public Action action;
        public Ws parentWorlds;

    }
}