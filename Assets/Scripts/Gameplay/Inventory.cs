﻿
using System;
using UnityEngine;
using OMTB.Collection;
using UnityEngine.Events;

namespace OMTB.Gameplay
{
    public class InventoryConfig
    {
        
        public int Rows { get; set; }

        public int Columns { get; set; }
    }
  
    public class Inventory: MonoBehaviour
    {
        /**
         * Internal data to manage items
         * */
        [System.Serializable]
        private class Slot
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

        /**
         * Called every time something changes in the inventory.
         * */
        public UnityAction OnChanged;


        public static Inventory Instance { get; private set; }
        
        public int Rows { get { return rows; } }
        public int Columns { get { return columns; } }


        #region PRIVATE
        Slot[] slots; // The item list
        int rows, columns;

        /**
         * This is form multislots items support. 
         * If an item take more than one slot to be stored, the there will be a root slot storing item data and al the other slots null. 
         * The root slot is the upper left one.
         * */
        int[] roots; // Used to manage multislots items; roots[i] = j means that slots[i] is null and belong to slots[j] which contains a big item

        #endregion

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;

                InventoryConfig config = GetConfiguration();
                Instance.rows = config.Rows;
                Instance.columns = config.Columns;
                Instance.slots = new Slot[rows*columns];
                Instance.roots = new int[rows * columns];
                // Init parents
                for (int i = 0; i < roots.Length; i++)
                    roots[i] = -1;

                // Get data from chache
                //TryLoadFromCache();

                // Set onSave handle
                //CacheManager.SetHandleOnSave(HW.Constants.InventoryFileName, HandleCacheManagerOnSave);

                DontDestroyOnLoad(gameObject);
            }
            else
                GameObject.Destroy(gameObject);
        }

        #region GET
        /**
         * Returns true if there is at least a slot containig the given item
        * */
        public bool Exists(Item item)
        {
            ValidateItem(item);

            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i] != null && slots[i].Item == item)
                    return true;
            }

            return false;
        }

        public int GetQuantity(Item item)
        {
            ValidateItem(item);

            int ret = 0;
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i] != null && slots[i].Item == item)
                    ret += slots[i].Quantity;
            }

            return ret;
        }

        public int GetQuantity(int index)
        {
            ValidateIndex(index);

            if (IsEmpty(index))
                return 0;
            
            if (roots[index] >= 0) // I know slots[index] is not empty thanks to IsEmpty()
                return slots[roots[index]].Quantity; // Gets the root
            
            return slots[index].Quantity;
        }

        public bool IsEmpty(int index)
        {
            ValidateIndex(index);

            return ( slots[index] == null && roots[index] == -1);
        }

        public Item GetItem(int index)
        {
            ValidateIndex(index);

            if (IsEmpty(index))
                return null;

            if (roots[index] >= 0) // I know slots[index] is not empty thanks to IsEmpty()
                return slots[roots[index]].Item; // Gets the root

            return slots[index].Item;
        }



        #endregion

        #region ADD
        /** TESTED
         * Adds an item given its quantity.
         * Returns the quantity added.
         * */
        public int Insert(Item item, int quantity)
        {
            ValidateItem(item);

            ValidateQuantity(quantity);

            int added = InternalInsert(item, quantity);

            if(added > 0)
                OnChanged?.Invoke(); // Send the event only if changes happened

            return added;

        }

        /**
         * Adds an item given its quantity to a specific slot. 
         * Returns the quantity actually added.
         * */
        public int Insert(int index, Item item, int quantity)
        {
            ValidateIndex(index);

            ValidateItem(item);

            ValidateQuantity(quantity);

            int added = InternalInsert(index, item, quantity);
            
            if (added > 0)
                OnChanged?.Invoke(); // Send the event only if changes happened

            return added;
        }

        #endregion


        #region REMOVE
        /**
         * Removes a specific quantity of an item starting from the first slot.
         * */
        public int Remove(Item item, int quantity)
        {
            ValidateItem(item);

            ValidateQuantity(quantity);

            int count = InternalRemove(item, quantity);

           
            if(count > 0)
                OnChanged?.Invoke();

            return count;
        }

        /**
        * Removes a specific quantity of an item from a single slot.
        * */
        public int Remove(int index, int quantity)
        {
            ValidateIndex(index);

            ValidateQuantity(quantity);


            int count = InternalRemove(index, quantity);


            if (count > 0)
                OnChanged?.Invoke();

            return count;
        }


        #endregion




        #region CACHING
        /**
         * Aggiorna i dati in cache prima che questa venga salvata su file.
         * */
        private void HandleCacheManagerOnSave()
        {

            //InventoryData data = new InventoryData();
            //// Creo struttura dati per cache
            //foreach (ItemData iData in itemDataList)
            //{
            //    data.AddOrUpdate(iData.Item.Name, iData.Count);
            //    //InventoryData.ItemData d = new InventoryData.ItemData(iData.Item.Name, iData.Count);
            //    //d.assetName = iData.Item.Name;
            //    //d.count = iData.Count;
            //    //data.items.Add(d);
            //}

            //// Salvo struttura dati in cache
            //CacheManager.SetData(Constants.InventoryFileName, data);

        }
        /**
         * Prova a caricare i dati dalla cache.
         * */
        //private void TryLoadFromCache()
        //{

        //    InventoryData data = CacheManager<InventoryData>.GetData(Constants.InventoryFileName);
        //    if (data == null)
        //        return;


        //    List<string> itemNames = data.GetAll();

        //    // Carico la collection di items da resources
        //    List<Item> items = new List<Item>(Resources.LoadAll<Item>(Constants.ItemResourcePath));

        //    // Carico la cache nella struttura dati dell'inventario
        //    //foreach(InventoryData.ItemData iid in data.items)
        //    foreach (string itemName in itemNames)
        //    {
        //        // Cerco l'item nella collection
        //        Item item = items.Find(i => i.name == itemName);

        //        if (item == null)
        //        {
        //            Debug.LogError("File di salvataggio corrotto:" + Constants.InventoryFileName);
        //            return;
        //        }

        //        // Aggiungo un nuovo item all'inventario
        //        itemDataList.Add(new ItemData(item, data.Count(itemName)));

        //    }
        //}
        #endregion

        #region INTERNAL

        int InternalInsert(Item item, int quantity)
        {
            if (!item.MultipleSlots)
                return InternalInsertSingle(item, quantity);
            else
                return InternalInsertMultiple(item, quantity);
            
        }

        int InternalInsert(int index, Item item, int quantity)
        {
            // We can not exceed the maximum capacity for this item
            if (!item.IsInfiniteQuantity)
            {
                int c = GetQuantity(item);
                if (c + quantity > item.MaxQuantity)
                    quantity = item.MaxQuantity - c;
            }

            // Max quantity for this item has already been reached
            if (quantity == 0)
                return 0;

            if (!item.MultipleSlots)
                return InternalInsertSingle(index, item, quantity);
            else
                return InternalInsertMultiple(index, item, quantity);
        }

        int InternalInsertSingle(Item item, int quantity)
        {
            Debug.Log(string.Format("Adding single slot item: {0} ({1}).", item, quantity));
            int tmp = quantity;
            
            // Look for room to add the item
            for (int i = 0; i < slots.Length && quantity > 0; i++)
            {
                quantity -= InternalInsertSingle(i, item, quantity);

            }

            return tmp - quantity;
        }

        int InternalInsertSingle(int index, Item item, int quantity)
        {
            Debug.Log(string.Format("Single item add: {0}, index: {1}, quantity: {2}", item, index, quantity));

            if (!IsEmpty(index) && GetItem(index) != item)
                return 0;

            //// Check the slot is not occupied by another item
            //if (slots[index] != null && slots[index].Item != item)
            //    return 0;

            //// Check if the slot beholds to a multiple slots holding a different item
            //if (slots[index] == null && roots[index] >= 0 && slots[roots[index]].Item != item)
            //    return 0;

            // Ok can be added
            if (slots[index] == null)
                slots[index] = new Slot(item);

            if (!item.IsInfiniteQuantityPerSlot && slots[index].Quantity + quantity > item.MaxQuantityPerSlot)
                quantity = item.MaxQuantityPerSlot - slots[index].Quantity;

            slots[index].Quantity += quantity;

            return quantity;
        }

        /**
         * The root slot is int the upper left.
         * */
        int InternalInsertMultiple(int index, Item item, int quantity)
        {
            //// Check if there is a different item
            //if (slots[index] != null && slots[index].Item != item)
            //    return 0;

            //// Check if the slot beholds to a multiple slots holding a different item
            //if (slots[index] == null && roots[index] >= 0 && slots[roots[index]].Item != item)
            //    return 0;

            if (!IsEmpty(index) && GetItem(index) != item)
                return 0;

            if (index + item.SlotShape.x > rows || index + item.SlotShape.y > columns ) // It doesn't fit
            {
                return 0;
            }

            // I need to check if the slots starting from index and matching the item width are free NO E' DA RIFARE
            for(int i=1; i<item.SlotShape.y; i++)
            {
                if (!IsEmpty(index+i) && GetItem(index+i) != item)
                    return 0;
            }
            // As above but for the heigh
            for (int i = 1; i < item.SlotShape.x; i++)
            {
                if (!IsEmpty(index + i*columns) && GetItem(index + i*columns) != item)
                    return 0;
            }

            // I can use the simple slot insert code
            int count = InternalInsertSingle(index, item, quantity);
            if (count == 0)
                return 0;

            // Set roots
            for (int i = 1; i < item.SlotShape.y; i++)
            {
                roots[index + i] = index;
            }
            for (int i = 1; i < item.SlotShape.x; i++)
            {
                roots[index + i*columns] = index;
            }

            return count;
        }

        int InternalInsertMultiple(Item item, int quantity)
        {
            return 0;
        }
        
        int InternalRemove(Item item, int quantity)
        {
            if (!item.MultipleSlots)
                return InternalRemoveSingle(item, quantity);
            else
                return InternalRemoveMulti(item, quantity);
        }

        int InternalRemove(int index, int quantity)
        {
            if (slots[index] == null)
                return 0;

            if (!slots[index].Item.MultipleSlots)
                return InternalRemoveSingle(index, quantity);
            else
                return InternalRemoveMulti(index, quantity);

        }

        int InternalRemoveSingle(Item item, int quantity)
        {
            int tmp = quantity;
            for (int i = 0; i < slots.Length && quantity > 0; i++)
            {
                if (slots[i] != null && slots[i].Item == item)
                {
                    quantity -= InternalRemoveSingle(i, quantity);
                }
            }

            return tmp - quantity;
        }

        int InternalRemoveSingle(int index, int quantity)
        {
            
            if(slots[index].Quantity <= quantity)
            {
                quantity = slots[index].Quantity;
                slots[index] = null;
            }
            else
            {
                slots[index].Quantity -= quantity;
            }

            return quantity;
        }



        int InternalRemoveMulti(int index, int quantity)
        {
            return 0;
        }

        int InternalRemoveMulti(Item item, int quantity)
        {
            return 0;
        }
        #endregion 

        #region VALIDATE
        void ValidateIndex(int index)
        {
            if (index < 0)
                throw new Exception(string.Format("Index is less than zero: {0}.", index));

            if (index >= slots.Length)
                throw new Exception(string.Format("Index is too higher: {0}, slots.length: {1}.", index, slots.Length));
        }

        void ValidateItem(Item item)
        {
            if (item == null)
                throw new Exception(string.Format("Item is null: {0}.", item));
        }

        void ValidateQuantity(int quantity)
        {
            if (quantity <= 0)
                throw new Exception(string.Format("Quantity is less or equal to zero: {0}.", quantity));
        }

        #endregion

        #region CONFIGURATION
        InventoryConfig GetConfiguration()
        {
            return new InventoryConfig() { Rows = 5, Columns = 8 };
        }
        #endregion

    }
}
