using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Gameplay;

namespace OMTB.Collection
{
    public enum WeaponGrip { None, OneHand, TwoHands }

    public abstract class WeaponConfig: ItemConfig
    {
        
        public List<Damage> Damages { get; set; }
        
        public WeaponGrip Grip { get; set; }

        
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

        [SerializeField]
        [ReadOnly]
        WeaponGrip grip = WeaponGrip.None;
        public WeaponGrip Grip { get { return grip; } }

        protected override void Init(Config config)
        {
            base.Init(config);
            WeaponConfig c = config as WeaponConfig;
            if (c.Damages != null)
                damages = c.Damages;
            else
                damages = new List<Damage>();
            grip = c.Grip;
        }

    }


}
