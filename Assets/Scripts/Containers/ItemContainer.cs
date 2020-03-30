using OMTB.Collection;
using OMTB.Interface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace OMTB.Container
{
    /**
     * The only purpose of this class is to provide a protected data struct to store items.
     * */
    public abstract class ItemContainer : MonoBehaviour
    {
        /**
         * Called every time something changes in the inventory.
         * */
        public UnityAction OnChanged;

        /**
        * Internal data to manage items
        * */
        [System.Serializable]
        protected class Slot
        {
            public Item Item { get; set; }

            public int Quantity { get; set; } = 0;

            public Slot(Item item)
            {
                Item = item;
            }

            public Slot(Item item, int quantity)
            {
                Item = item;
                Quantity = quantity;
            }
        }

        public void SetOnChanged(UnityAction handle)
        {
            OnChanged += handle;
        }

        public void UnsetOnChanged(UnityAction handle)
        {
            OnChanged -= handle;
        }

    }

}
