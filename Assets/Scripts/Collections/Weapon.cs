using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Gameplay;

namespace OMTB.Collection
{

    public abstract class WeaponConfig: ItemConfig
    {
        
        public List<Damage> Damages { get; set; }
        
        public Weapon.Held Held { get; set; }

        
    }

    public abstract class Weapon : Item
    {
        /**
         * None: you can not put the weapon in the equipment like a sword; imagine some king of magic scroll for exemple.
         * OneHand: can be holded in one hand
         * TwoHands: you need two hands
         * */
        public enum Held { None, OneHand, TwoHands }

        List<Damage> damageList; // Can deliver multiple damage

        Held held = Held.None;

        protected override void Init(ItemConfig config)
        {
            base.Init(config);
            WeaponConfig c = config as WeaponConfig;
            if (c.Damages != null)
                damageList = c.Damages;
            else
                damageList = new List<Damage>();
            held = c.Held;
        }

    }


    public class MeleeWeaponConfig : WeaponConfig
    {
        public float Speed { get; set; }
    
        public float Distance { get; set; }
    }

    public class MeleeWeapon: Weapon
    {
        float speed; // Attack speed

        float distance;

        protected override void Init(ItemConfig config)
        {
            base.Init(config);

            MeleeWeaponConfig c = config as MeleeWeaponConfig;
            speed = c.Speed;
            distance = c.Distance;
        }

    }

    public class RangedWeaponConfig: ItemConfig
    {
        public List<AmmunitionData> Ammunitions { get; set; }

        public float FireRate { get; set; }
        public float FireRange { get; set; }
        public float ReloadTime { get; set; }
    }


    public class RangedWeapon: Weapon
    {

        List<AmmunitionData> ammunitions;

        float fireRate;

        float fireRange;

        float reloadTime;

        protected override void Init(ItemConfig config)
        {
            base.Init(config);

            RangedWeaponConfig c = config as RangedWeaponConfig;
            if (c.Ammunitions != null)
                ammunitions = c.Ammunitions;
            else
                ammunitions = new List<AmmunitionData>();

            fireRate = c.FireRate;
            fireRange = c.FireRange;
            reloadTime = c.ReloadTime;
        }
    }

    public class AmmunitionData
    {
        Ammunition ammunition;
        int magazineCapacity;

        public AmmunitionData(Ammunition ammunition, int magazineCapacity)
        {
            this.ammunition = ammunition;
            this.magazineCapacity = magazineCapacity;
        }
    }
}
