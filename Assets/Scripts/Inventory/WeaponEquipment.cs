
using System;
using UnityEngine;
using OMTB.Collection;
using UnityEngine.Events;
using OMTB.Interface;

namespace OMTB.Gameplay
{
    public enum Hand { Left, Right }

    public class WeaponEquipment : MonoBehaviour, IContainer<Item>
    {

        [SerializeField]
        Hand hand;

        public static WeaponEquipment Instance { get; private set; }



        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;

                //ItemContainerConfig config = GetConfiguration();
                //Instance.rows = config.Rows;
                //Instance.columns = config.Columns;
                //Instance.slots = new Slot[rows * columns];
                //Instance.roots = new int[rows * columns];
                //// Init parents
                //for (int i = 0; i < roots.Length; i++)
                //    roots[i] = -1;

                // Get data from chache
                //TryLoadFromCache();

                // Set onSave handle
                //CacheManager.SetHandleOnSave(HW.Constants.InventoryFileName, HandleCacheManagerOnSave);

                

                DontDestroyOnLoad(gameObject);
            }
            else
                GameObject.Destroy(gameObject);
        }

        public int GetFreeRoom(int index, Item item)
        {
            Debug.Log("GetFreeRoom");
            // Is not a weapon or a shield or is a shield but the current hand is the right one
            //if ( !item.GetType().IsSubclassOf(typeof(Weapon)) || item.GetType() != typeof(Shield) || (item.GetType() == typeof(Shield) && hand == Hand.Right))
            //    return 0;

            Debug.Log("GetFreeRoom:aaaaaaaaaa");

            return 1;
        }

        public void SetOnChanged(UnityAction handle)
        {
            throw new NotImplementedException();
        }

        public void UnsetOnChanged(UnityAction handle)
        {
            throw new NotImplementedException();
        }

        public bool IsEmpty(int index)
        {
            throw new NotImplementedException();
        }

        public Item GetElement(int index)
        {
            throw new NotImplementedException();
        }

        public int GetQuantity(int index)
        {
            throw new NotImplementedException();
        }

        public int Insert(int index, Item item, int quantity)
        {
            throw new NotImplementedException();
        }

        public int Remove(int index, int quantity)
        {
            throw new NotImplementedException();
        }

        public int Move(int srcIndex, int dstIndex, int quantity)
        {
            throw new NotImplementedException();
        }




        //#region CONFIGURATION
        //protected override ItemContainerConfig GetConfiguration()
        //{
        //    return new ItemContainerConfig() { Rows = 5, Columns = 8 };
        //}


        //#endregion
    }


}
