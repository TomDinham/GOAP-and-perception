using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PC
{
    public class Configuration 
    {
        public Stimulus.StimulusTypes type;
        public float peak;
        public float attack;
        public float decay;
        public float sustainPercentage;
        public float sustain;
        public float release;
        public float modifier;
        public float aggressive;
        public float threatening;
        public float interesting;
        public Stimulus.StimulusTypes Type { get { return type; } }
        public float Decay { get { return decay; } }
       
        public Configuration(Stimulus.StimulusTypes type, float peak,float attack,
            float decay, float sustainPercentage,float sustain, float release, float modifier,
            float aggressive, float threatening, float interesting)
        {
            //initilise the variables
            this.type = type;
            this.peak = peak;
            this.attack = attack;
            this.decay = decay;
            this.sustainPercentage = sustainPercentage;
            this.sustain = sustain;
            this.release = release;
            this.modifier = modifier;
            this.aggressive = aggressive;
            this.threatening = threatening;
            this.interesting = interesting;

        }
        public float GetPerception(float life) //  compair the percention to configure its life time graph 
        {
            if(life < attack)
            {
                return GetGraphY(life, 0f, attack, 0f, peak);

            }
            else if (life < decay)
            {
                return (GetGraphY(life, attack, decay, peak, (peak * sustainPercentage)));
            }
            else if(life <sustain)
            {
                return (peak * sustainPercentage);
                
            }
            else if( life < release)
            {
                return GetGraphY(life, sustain, release, (peak * sustainPercentage), 0f);
            }
            else { return -1; }
        }
        private float GetGraphY(float x,float x1,float x2, float y1, float y2)
        {
            float graph= (y2 - y1) / (x2 - x1); // calculate the graph for the life time and drop of times for the graph
            return graph *(x-x1)+y1;
        }
        public static List<Configuration>GetConfiguration()
        { // create a list of configurations and add audio and visual stimulus to this list
            List<Configuration> configs = new List<Configuration>();
            configs.Add(new Configuration(Stimulus.StimulusTypes.VisualCanSee, 100f, 2f, 3f, 0.9f, 15f, 18f, 1f, 1f, 0.625f, 0.25f)); 
            configs.Add(new Configuration(Stimulus.StimulusTypes.AudioMovement, 50f, 1.5f, 2.5f, 0.9f, 7f, 10f, 1f, 1f, 0.625f, 0.25f));
            return configs;
        }
    }
}