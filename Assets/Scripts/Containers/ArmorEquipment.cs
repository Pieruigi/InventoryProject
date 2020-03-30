
using System;
using UnityEngine;
using OMTB.Collection;
using UnityEngine.Events;
using OMTB.Interface;

namespace OMTB.Container
{

    public class ArmorEquipment : MonoBehaviour, IContainer<Item>
    {

        [SerializeField]
        ArmorBodyPart bodyPart;

        public static ArmorEquipment Instance { get; private set; }



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
            if (item.GetType() != typeof(Armor) || (item as Armor).BodyPart != bodyPart)
                return 0;

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

        public int Insert(Item item, int quantity)
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
