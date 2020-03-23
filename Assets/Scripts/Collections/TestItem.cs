using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB.Collection
{
    public class TestItemConfig: ItemConfig
    {

    }

    public class TestItem : Item
    {
        protected override void Init(ItemConfig config)
        {
            base.Init(config);
        }
    }

}
