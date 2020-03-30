
using System;
using UnityEngine;
using OMTB.Collection;
using UnityEngine.Events;
using OMTB.Interface;

namespace OMTB.Container
{
    public enum Hand { Left, Right }

    public class Equipment : ItemContainer, IContainer<Item>
    {

        public enum SlotTypeId { HeadArmor = 0, ChestArmor, HandsArmor, LegsArmor, FeetArmor, LeftWeapon, RightWeapon }

        [SerializeField]
        int numberOfSlots;

        //[SerializeField]
        Slot[] slots;

        public static Equipment Instance { get; private set; }


        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;

                // Get data from chache
                //TryLoadFromCache();

                // Set onSave handle
                //CacheManager.SetHandleOnSave(HW.Constants.InventoryFileName, HandleCacheManagerOnSave);
                slots = new Slot[numberOfSlots];
                

                DontDestroyOnLoad(gameObject);
            }
            else
                GameObject.Destroy(gameObject);
        }

        public int GetFreeRoom(int index, Item item)
        {
            // Is not a weapon or a shield or is a shield but the current hand is the right one
            if (!CheckFreeRoom(index, item))
                return 0;

            Debug.Log("GetFreeRoom:aaaaaaaaaa");

            return 1;
        }



        public bool IsEmpty(int index)
        {
            return slots[index] == null;
        }

        public Item GetElement(int index)
        {
            if (slots[index] == null)
                return null;

            return slots[index].Item;
        }

        public int GetQuantity(int index)
        {
            return 1;
        }

        public int Insert(Item item, int quantity)
        {
            return 0;
        }

        public int Insert(int index, Item item, int quantity)
        {
            //throw new NotImplementedException();
            if (slots[index] != null)
                return 0;

            slots[index] = new Slot(item, quantity);

            OnChanged?.Invoke();

            return quantity;
        }

        public int Remove(int index, int quantity)
        {
            if (slots[index] == null)
                return 0;

            int q = slots[index].Quantity;

            slots[index].Quantity -= quantity;

            int ret = quantity;

            if (slots[index].Quantity <= 0)
            {
                slots[index] = null;
                ret = q;
                
            }

            OnChanged?.Invoke();

            return ret;    



            
        }

        public int Move(int srcIndex, int dstIndex, int quantity)
        {
            throw new NotImplementedException();
        }


        bool CheckFreeRoom(int index, Item item)
        {
            if (index == (int)SlotTypeId.RightWeapon)
            {
                if (!item.GetType().IsSubclassOf(typeof(Weapon)))
                    return false;
            }

            if (index == (int)SlotTypeId.LeftWeapon)
            {
                if (!item.GetType().IsSubclassOf(typeof(Weapon)) && item.GetType() != typeof(Shield))
                    return false;
            }

            if(index == (int)SlotTypeId.HeadArmor)
            {
                if (item.GetType() != typeof(Armor) || (item as Armor).BodyPart != ArmorBodyPart.Head)
                    return false;
            }

            if (index == (int)SlotTypeId.ChestArmor)
            {
                if (item.GetType() != typeof(Armor) || (item as Armor).BodyPart != ArmorBodyPart.Chest)
                    return false;
            }

            if (index == (int)SlotTypeId.HandsArmor)
            {
                if (item.GetType() != typeof(Armor) || (item as Armor).BodyPart != ArmorBodyPart.Hands)
                    return false;
            }

            if (index == (int)SlotTypeId.LegsArmor)
            {
                if (item.GetType() != typeof(Armor) || (item as Armor).BodyPart != ArmorBodyPart.Legs)
                    return false;
            }

            if (index == (int)SlotTypeId.FeetArmor)
            {
                if (item.GetType() != typeof(Armor) || (item as Armor).BodyPart != ArmorBodyPart.Feet)
                    return false;
            }

            return true;
        }



        //#region CONFIGURATION
        //protected override ItemContainerConfig GetConfiguration()
        //{
        //    return new ItemContainerConfig() { Rows = 5, Columns = 8 };
        //}


        //#endregion
    }


}
