using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.ComponentModel;

namespace OMTB.Collection
{

    public class ItemConfig: Config
    {
        public string Name { get; set; }
       
        public Category Category { get; set; }

        public string ShortDescription { get; set; }
       
        public int MaxQuantity { get; set; } = -1;

        public int MaxQuantityPerSlot { get; set; } = -1;

        
        public Vector2 SlotShape { get; set; } = Vector2.one; // Columns x rows

    }

    public abstract class Item : Asset
    {
        [SerializeField]
        //[ReadOnly]
        private string _name;
        public string Name { get { return _name; } }

        [SerializeField]
        //[ReadOnly]
        Category category; // Used as filter; can be null.
        public Category Category { get { return Category; } }
        

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
        private int maxQuantityPerSlot = -1; // Negative means infinite
        public int MaxQuantityPerSlot { get { return maxQuantityPerSlot; } }
        public bool IsInfiniteQuantityPerSlot { get { return maxQuantityPerSlot < 0; } }

        [SerializeField]
        //[ReadOnly]
        
        public bool TakesMoreSlots { get { return slotShape != Vector2.one; } }

        [SerializeField]
        //[ReadOnly]
        private Vector2 slotShape = Vector2.one;
        public Vector2 SlotShape { get { return slotShape; } } // Columns x rows

        [SerializeField]
        //[ReadOnly]
        private Sprite icon; // The icon representing the item 
        public Sprite Icon { get { return icon; } }

        [SerializeField]
        [ReadOnly]
        GameObject asset; // The prefab you are going to load into the scene


        //public static Item Create(System.Type type, ItemConfig config)
        //{
        //    Item i = ScriptableObject.CreateInstance(type) as Item;
        //    i.Init(config);
        //    return i;
        //}
                
        protected override void Init(Config config)
        {
            Debug.Log("Parent Init");
            ItemConfig c = config as ItemConfig;
            _name = c.Name;
            maxQuantity = c.MaxQuantity;
            if (maxQuantity == 0)
                maxQuantity = -1;
            maxQuantityPerSlot = c.MaxQuantityPerSlot;
            if (maxQuantityPerSlot == 0)
                maxQuantityPerSlot = -1;

            shortDesc = c.ShortDescription;

            slotShape = c.SlotShape;
            if (slotShape.x == 0)
                slotShape.x = 1;
            if (slotShape.y == 0)
                slotShape.y = 1;

        }

    }

}

