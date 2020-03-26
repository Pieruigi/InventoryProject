using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB.Collection
{
    public enum ArmorBodyPart { Head, Chest, Gloves, Pants, Boots }

    public class ArmorConfig: ItemConfig
    {
        public ArmorBodyPart BodyPart { get; set; }
    }

    public class Armor : Item
    {
        [SerializeField]
        [ReadOnly]
        ArmorBodyPart bodyPart = ArmorBodyPart.Chest;

        protected override void Init(Config config)
        {
            base.Init(config);

            ArmorConfig c = config as ArmorConfig;
            bodyPart = c.BodyPart;
        }
    }

}
