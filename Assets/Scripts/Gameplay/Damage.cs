using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB.Gameplay
{

    public class DamageConfig
    {
        public Damage.Type Type { get; set; }

        public Damage.SubType SubType { get; set; }

        public float Total { get; set; }
        
        public float PerSecond { get; set; }
        
        public float Duration { get; set; }
        
        public float InternalRange { get; set; }

        public float ExternalRange { get; set; }
        
    }

    public class Damage
    {
        public enum Type { Physical, Elemental, Pure }
        public enum SubType { None, Crushing, Slashing, Piercing, Burning, Freezing, Shocking, Corrosion }

        Type type;
        SubType subType;

        float total; // The total amount of delivered damage
        float perSecond; // How much damage per seconds?
        float duration; // Duration in seconds ( only available to perSecond )

        float internalRange; // Inside this range the maximum damage is delivered
        float externalRange; // Outside this range no more damage is delivered

     
        public Damage(DamageConfig config)
        {
            type = config.Type;
            subType = config.SubType;
            total = config.Total;
            perSecond = config.PerSecond;
            duration = config.Duration;
            internalRange = config.InternalRange;
            externalRange = config.ExternalRange;

            if (externalRange < internalRange)
                externalRange = internalRange;

        }

        /**
         * At least we need to validate type and subtype.
         * */
        void Validate(DamageConfig config)
        {
            
            switch (config.Type)
            {
                case Damage.Type.Physical:
                    if (!(config.SubType == Damage.SubType.Crushing || config.SubType == Damage.SubType.Slashing || config.SubType == Damage.SubType.Piercing))
                        throw new System.Exception(string.Format("Physical damage must be Crushing, Slashing or Piercing."));
                    break;
                case Damage.Type.Elemental:
                    if (!(config.SubType == Damage.SubType.Burning || config.SubType == Damage.SubType.Freezing || 
                          config.SubType == Damage.SubType.Shocking || config.SubType == Damage.SubType.Corrosion))
                        throw new System.Exception(string.Format("Elemental damage must be Burning, Freezing, Shocking or Corrosion."));
                    break;
                case Damage.Type.Pure:
                    if (config.SubType != Damage.SubType.None)
                        throw new System.Exception(string.Format("Pure damage is pure."));
                    break;
            }

            
        }
    }

}
