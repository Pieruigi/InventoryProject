using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.ComponentModel;

namespace OMTB.Collection
{

    public class ItemConfig
    {
        public string Name { get; set; }
        public string ShortDescription { get; set; }
       
        public int MaxQuantity { get; set; } = -1;

        public int MaxQuantityPerSlot { get; set; } = 1;

        public bool MultipleSlots { get; set; }

        public Vector2 Shape { get; set; } // Rows x columns

    }

    public abstract class Item : ScriptableObject
    {
        [SerializeField]
        [ReadOnly]
        private string _name;
        public string Name
        {
            get { return _name; }
        }

        [SerializeField]
        [ReadOnly]
        private string shortDesc;
        public string ShortDescription { get { return shortDesc; } }

        [SerializeField]
        [ReadOnly]
        private string longDesc;
        public string LongDescripion { get { return longDesc; } }

        [SerializeField]
        [ReadOnly]
        private int maxQuantity; // How many items in the whole inventory? Negative means infinite
        public int MaxQuantity { get { return maxQuantity; } }
        public bool IsInfiniteQuantity { get { return maxQuantity < 0; } }

        [SerializeField]
        [ReadOnly]
        private int maxQuantityPerSlot; // Negative means infinite
        public int MaxQuantityPerSlot { get { return maxQuantityPerSlot; } }
        public bool IsInfiniteQuantityPerSlot { get { return maxQuantityPerSlot < 0; } }

        [SerializeField]
        [ReadOnly]
        private bool multipleSlots = false;
        public bool MultipleSlots { get { return multipleSlots; } }

        [SerializeField]
        [ReadOnly]
        private Vector2 slotShape = Vector2.one;
        public Vector2 SlotShape { get { return slotShape; } } // Rows x columns

        [SerializeField]
        [ReadOnly]
        private Sprite icon; // The icon representing the item 

        [SerializeField]
        [ReadOnly]
        GameObject asset; // The prefab you are going to load into the scene


        public static Item Create(System.Type type, ItemConfig config)
        {
            Item i = ScriptableObject.CreateInstance(type) as Item;
            i.Init(config);
            return i;
        }
                
        protected virtual void Init(ItemConfig config)
        {
            Debug.Log("Parent Init");
            _name = config.Name;
            maxQuantity = config.MaxQuantity;
            if (maxQuantity == 0)
                maxQuantity = -1;
            maxQuantityPerSlot = config.MaxQuantityPerSlot;
            if (maxQuantityPerSlot == 0)
                maxQuantityPerSlot = -1;

            shortDesc = config.ShortDescription;
            
            multipleSlots = config.MultipleSlots;
            slotShape = config.Shape;
        }

    }

}

