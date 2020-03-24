using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Collection;

namespace OMTB.Gameplay
{

    public class DamageConfig
    {
        public DamageType damageType;

        public float Total { get; set; }
        
        public float PerSecond { get; set; }
        
        public float Duration { get; set; }
        
        public float InternalRange { get; set; }

        public float ExternalRange { get; set; }
        
    }

    public class Damage
    {
        DamageType damageType;

        float total; // The total amount of delivered damage
        float perSecond; // How much damage per seconds?
        float duration; // Duration in seconds ( only available to perSecond )

        float internalRange; // Inside this range the maximum damage is delivered
        float externalRange; // Outside this range no more damage is delivered

     
        public Damage(DamageConfig config)
        {
            damageType = config.damageType;
            total = config.Total;
            perSecond = config.PerSecond;
            duration = config.Duration;
            internalRange = config.InternalRange;
            externalRange = config.ExternalRange;

            if (externalRange < internalRange)
                externalRange = internalRange;

        }

    }

}
