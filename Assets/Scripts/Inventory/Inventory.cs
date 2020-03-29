
using System;
using UnityEngine;
using OMTB.Collection;
using UnityEngine.Events;
using OMTB.Interface;

namespace OMTB.Gameplay
{
    public class ItemContainerConfig
    {
        
        public int Rows { get; set; }

        public int Columns { get; set; }
    }
  
    public class Inventory: MonoBehaviour, IBigSlotContainer
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

        //protected abstract ItemContainerConfig GetConfiguration();

        //public static Inventory Instance { get; private set; }
        [SerializeField]
        int columns, rows;

        public int Rows { get { return rows; } }
        public int Columns { get { return columns; } }

        public static Inventory Instance { get; private set; }

        #region PRIVATE

        Slot[] slots; // The item list

        

        /**
         * This is form multislots items support. 
         * If an item take more than one slot to be stored, the there will be a root slot storing item data and al the other slots null. 
         * The root slot is the upper left one.
         * */

        int[] roots; // Used to manage multislots items; roots[i] = j means that slots[i] is null and belong to slots[j] which contains a big item

        #endregion

        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = this;

                slots = new Slot[rows * columns];
                roots = new int[rows * columns];
                // Init parents
                for (int i = 0; i < roots.Length; i++)
                    roots[i] = -1;

                DontDestroyOnLoad(gameObject);
            }
            else
                GameObject.Destroy(gameObject);

            //ItemContainerConfig config = GetConfiguration();
            //rows = config.Rows;
            //columns = config.Columns;
   

                // Get data from chache
                //TryLoadFromCache();

                // Set onSave handle
                //CacheManager.SetHandleOnSave(HW.Constants.InventoryFileName, HandleCacheManagerOnSave);

        }

        public void SetOnChanged(UnityAction handle)
        {
            OnChanged += handle;
        }

        public void UnsetOnChanged(UnityAction handle)
        {
            OnChanged -= handle;
        }

        #region GET
        /**
         * Returns true if there is at least a slot containig the given item
        * */
        public bool Exists(Item item)
        {
         

            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i] != null && slots[i].Item == item)
                    return true;
            }

            return false;
        }

        public int GetQuantity(Item item)
        {
           

            int ret = 0;
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i] != null && slots[i].Item == item)
                    ret += slots[i].Quantity;
            }

            return ret;
        }

        /**
         * Returns the quantity of the store item. Works also for not root slots belonging to a big slot.
         * */
        public int GetQuantity(int index)
        {
          

            if (IsEmpty(index))
                return 0;
            
            if (roots[index] >= 0) // I know slots[index] is not empty thanks to IsEmpty()
                return slots[roots[index]].Quantity; // Gets the root
            
            return slots[index].Quantity;
        }

        /**
         * Returns the amount of free room allowed for the given item in the given slot.
         * */
        public virtual int GetFreeRoom(int index, Item item)
        {
           

            int tot = GetQuantity(item); // The total quantity of the item in the inventory

            if (!item.TakesMoreSlots)
            {
                return GetFreeRoom(index, item, tot);
            }
            else
            {
                int ret = 0;
                if (!IsRoot(index)) // It's not the root
                {
                    int tmp = GetRootIndex(index);
                    if(tmp >= 0) // Find the root
                    {
                        index = GetRootIndex(index);
                    }
                    else // It's empty
                    {
                        // If it doesn' fit then move back
                        index = FitBorders(index, item);

                    }
                }

                for (int i=0; i<item.SlotShape.y; i++) // Rows
                {
                    for(int j=0; j < item.SlotShape.x; j++) // Columns
                    {
                        int idx = index + i * columns + j;
                        Debug.Log("Checking:" + idx);
                        ret = GetFreeRoom(idx, item, tot);
                        if (ret <= 0)
                            return 0;
                    }
                }

                return ret;
            }
              
        }

 

        public bool IsEmpty(int index)
        {
           

            return ( slots[index] == null && roots[index] == -1);
        }

        public Item GetElement(int index)
        {
        
            if (IsEmpty(index))
                return null;

            if (roots[index] >= 0) // I know slots[index] is not empty thanks to IsEmpty()
                return slots[roots[index]].Item; // Gets the root

            return slots[index].Item;
        }

        /**
         * Returns true if the slot is the root of a multislots item.
         * */
        public bool IsRoot(int index)
        {
          
            if (IsEmpty(index))
                return false;

            if (!GetElement(index).TakesMoreSlots)
                return true;

            // Check if exists has root
            for(int i=0; i<roots.Length; i++)
            {
                if (roots[i] == index)
                    return true;
            }

            return false;
        }

        /**
         * Returns the root if the index passed as parameter is part of a multislots item or if is itself the roor, otherwise returns -1.
         * */
        public int GetRootIndex(int index)
        {
            if (IsEmpty(index))
                return -1;

            if (!GetElement(index).TakesMoreSlots)
                return index;

            return roots[index];
                
        }

        /**
         * Returns true if the slot belongs to a big slot and the vector will store the relative coordinates inside the big slot itself, otherwise
         * returns false. It works also for the big slot root.
         * */
        public bool TryGetCoordsInBigSlot(int index, out Vector2 coords)
        {
            
            coords = Vector2.zero;

            if (IsEmpty(index))
                return false;

            Item item = GetElement(index);

            if (!item.TakesMoreSlots)
                return false;

            if (IsRoot(index))
                return true;

            int rootIndex = GetRootIndex(index);

            int rootX = rootIndex % columns;
            int thisX = index % columns;

            coords.x = thisX - rootX;

            int rootY = rootIndex / columns;
            int thisY = index / columns;


            coords.y = thisY - rootY;

            return true;
        }

        /**
         * Returns true if the index refers to a slot belonging to a multiple slot and fill the array with all the slots, otherwise returns false.
         * */
        public bool TryGetBigSlotIndices(int index, out int[] indices)
        {
           
            indices = null;
            
            if (IsEmpty(index))
                return false;

            int root = -1;

            // Try to find the root if exists
            if (IsRoot(index))
                root = index;
            else
                root = GetRootIndex(index);

            if (root == -1) // Not a big slot
                return false;

            // Its a big slot, take the item
            Item item = slots[root].Item;

            // Init the array
            indices = new int[(int)item.SlotShape.x*(int)item.SlotShape.y];

            // Loop through all the slots beolonging to the big slot and add them to the index array
            for(int i=0; i<item.SlotShape.y; i++)
            {
                for(int j =0; j<item.SlotShape.x; j++)
                {
                    indices[i * (int)item.SlotShape.x + j] = root + i * columns + j;
                    Debug.Log(string.Format("Indices[{0}]: {1}", i * (int)item.SlotShape.x + j, root + i * columns + j));
                }
            }

            

            return true;
        }


        #endregion

        #region MOVE
        /**
         * Moves items from one slot to another and returns the actual move quantity
         * */
        public int Move(int srcIndex, int dstIndex, int quantity)
        {
            // Get the item
            Item item = GetElement(srcIndex);
            
            // Remove from source
            int removed = Remove(srcIndex, quantity);

            // Add to destination
            int added = Insert(dstIndex, item, quantity);

            // Add remaining items to source
            if (removed > added)
                Insert(srcIndex, item, removed - added);
       
            return added;
        }

        ///**
        // * Moves items from a given slot to a slot into another container and returns the actual move quantity
        // * */
        //public int Move(int srcIndex, int dstIndex, IItemContainer dstContainer, int quantity)
        //{
        //    if (dstContainer == null)
        //        throw new Exception(string.Format("Destination container is null: {0}.", dstContainer));

        //    // Get the item
        //    Item item = GetItem(srcIndex);

        
        //    // Remove from source
        //    int removed = Remove(srcIndex, quantity);

        //    // Add to destination
        //    int added = dstContainer.Insert(dstIndex, item, quantity);

        //    // Add remaining items to source
        //    if (removed > added)
        //        Insert(srcIndex, item, removed - added);

        //    return added;
        //}
        #endregion

        #region ADD
        /** TESTED
         * Adds an item given its quantity.
         * Returns the quantity added.
         * */
        public int Insert(Item item, int quantity)
        {
           

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

        private int GetFreeRoom(int index, Item item, int currentQuantity)
        {
            int q;
            if (IsEmpty(index)) // The slot is empty
            {
                Debug.Log("The slot is empty:"+index);
                q = item.MaxQuantityPerSlot;
            }
            else // The slot is not empty
            {
                if (GetElement(index) != item)// Different items
                    return 0;

                q = item.MaxQuantityPerSlot - GetQuantity(index); // The maximum amount I can actually add in the slot
            }

            if (item.MaxQuantity < 0) // Infinite
                return q;

            if (q > currentQuantity) // I can't add this quantity without exceeding the maximum amount
                return item.MaxQuantity - currentQuantity;
            else
                return q; // I fill the slot
        }

        int InternalInsert(Item item, int quantity)
        {
            Debug.Log("InternalInsert");
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

            if (!item.TakesMoreSlots)
                return InternalInsertSingle(item, quantity);
            else
                return InternalInsertMultiple(item, quantity);
            
        }

        int InternalInsert(int index, Item item, int quantity)
        {
            //Debug.Log("InternalInsert-index");
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


            if (!item.TakesMoreSlots)
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
            //Debug.Log(string.Format("Single item add: {0}, index: {1}, quantity: {2}", item, index, quantity));

            if (!IsEmpty(index) && GetElement(index) != item)
                return 0;

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
            index = FitBorders(index, item);
            //Debug.Log("InternalInsertMultiple");

            if (!IsEmpty(index) && GetElement(index) != item)
                return 0;

            if(!IsEmpty(index) && GetElement(index) == item)
                index = GetRootIndex(index);

           

            // If at least one of the slots matching the item slot shape contains a different item or refers to a root different from the index then return
            for (int i=0; i<item.SlotShape.y; i++)
            {
                for(int j=0; j<item.SlotShape.x; j++)
                {
                    if (!IsEmpty(index + i*columns + j) && (GetElement(index + i * columns + j) != item || roots[index + i * columns + j] != index ) )
                        return 0;

                }
            }
            
            // I can use the simple slot insert code to insert in the root slot
            int count = InternalInsertSingle(index, item, quantity);
            if (count == 0)
                return 0;

            // Set root reference
            for (int i = 0; i < item.SlotShape.y; i++)
            {
                for (int j = 0; j < item.SlotShape.x; j++)
                {
                    roots[index + i * columns + j] = index;
                    
                }
            }
            

            return count;
        }

        int InternalInsertMultiple(Item item, int quantity)
        {
            int tmp = quantity;

            // Look for room to add the item
            for (int i = 0; i < slots.Length && quantity > 0; i++)
            {
                quantity -= InternalInsertMultiple(i, item, quantity);

            }

            return tmp - quantity;
        }
        
        int InternalRemove(Item item, int quantity)
        {
            if (!item.TakesMoreSlots)
                return InternalRemoveSingle(item, quantity);
            else
                return InternalRemoveMulti(item, quantity);
        }

        int InternalRemove(int index, int quantity)
        {
            if (IsEmpty(index))
                return 0;

            
            if (!GetElement(index).TakesMoreSlots)
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


        /**
         * Removes an item from the index given its quantity. Index can be the root or any other slot.
         * */
        int InternalRemoveMulti(int index, int quantity)
        {
            if (IsEmpty(index))
                return 0;

            int root = index;

            // If this is not the root then find it
            if (!IsRoot(root))
                root = GetRootIndex(index);

            Item item = slots[root].Item; 


            if (slots[root].Quantity <= quantity)
            {
                // We must remove the item so we need al the indices of the big slot
                int[] indices;
                if (!TryGetBigSlotIndices(root, out indices))
                    throw new Exception(string.Format("Big slot root {0} seems to have no sub slots.", root));

                quantity = slots[root].Quantity;
                //slots[root] = null;
                for (int i = 0; i < indices.Length; i++)
                {
                    slots[indices[i]] = null; // Its mandatory only for the root
                    roots[indices[i]] = -1;
                }

            }
            else 
            {
                // We don't need to remove the item in this case
                slots[root].Quantity -= quantity;
            }

            return quantity;

        }

        int InternalRemoveMulti(Item item, int quantity)
        {
            if (!Exists(item))
                return 0;
            int tmp = quantity;
            for(int i=0; i<slots.Length && quantity > 0; i++)
            {
                quantity -= InternalRemoveMulti(i, quantity);
            }

            return tmp - quantity;
        }

        int FitBorders(int index, Item item)
        {
            int y = index / columns + (int)item.SlotShape.y - rows;
            int x = index % columns + (int)item.SlotShape.x - columns;
            if (x > 0 || y > 0) // It doesn't fit
            {
                // Try move back
                if (y > 0) index -= y * columns;
                if (x > 0) index -= x;
            }

            return index;
        }

        
        #endregion 

     

    }
}
