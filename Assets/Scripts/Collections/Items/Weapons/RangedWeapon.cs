using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB.Collection
{
    public class RangedWeaponConfig : ItemConfig
    {
        public List<RangedWeapon.AmmunitionData> Ammunitions { get; set; }

        public float FireRate { get; set; }
        public float FireRange { get; set; }
        public float ReloadTime { get; set; }
    }


    public class RangedWeapon : Weapon
    {
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


        List<AmmunitionData> ammunitions;
        

        float fireRate;
        public float FireRate { get { return fireRate; } }

        float fireRange;
        public float FireRange { get { return fireRange; } }

        float reloadTime;
        public float ReloadTime { get { return reloadTime; } }

        protected override void Init(Config config)
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
}
