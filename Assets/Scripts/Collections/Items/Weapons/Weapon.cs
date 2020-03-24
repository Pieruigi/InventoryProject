using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Gameplay;

namespace OMTB.Collection
{
    public enum Held { None, OneHand, TwoHands }

    public abstract class WeaponConfig: ItemConfig
    {
        
        public List<Damage> Damages { get; set; }
        
        public Held Held { get; set; }

        
    }

    public abstract class Weapon : Item
    {
        /**
         * None: you can not put the weapon in the equipment like a sword; imagine some king of magic scroll for exemple.
         * OneHand: can be holded in one hand
         * TwoHands: you need two hands
         * */
        

        List<Damage> damages; // Can deliver multiple damage
        public List<Damage> Damages { get { return damages; } }

        Held held = Held.None;
        public Held Held { get { return held; } }

        protected override void Init(ItemConfig config)
        {
            base.Init(config);
            WeaponConfig c = config as WeaponConfig;
            if (c.Damages != null)
                damages = c.Damages;
            else
                damages = new List<Damage>();
            held = c.Held;
        }

    }


}
