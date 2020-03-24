using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB.Collection
{
    public class MeleeWeaponConfig : WeaponConfig
    {
        public float Speed { get; set; }

        public float Distance { get; set; }
    }

    public class MeleeWeapon : Weapon
    {
        float speed; // Attack speed
        public float Speed { get { return speed; } }

        float distance;
        public float Distance { get { return distance; } }

        protected override void Init(ItemConfig config)
        {
            base.Init(config);

            MeleeWeaponConfig c = config as MeleeWeaponConfig;
            speed = c.Speed;
            distance = c.Distance;
        }

    }
}

