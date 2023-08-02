using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scriptables
{
    public class Stats : ScriptableObject
    {
        public new string level;
        public int numberOfLegs;
        public int partsPerLeg;
        public float newLegRadius;
        public float LegDistance;
        public float legMinHeight;
        public float legMaxHeight;
        public float legHeight;
        public float minRotSpeed;
        public float maxRotSpeed;
        public float minOscillationSpeed;
        public float maxOscillationSpeed;
        
        public virtual void Activate() {}
    } 
}

