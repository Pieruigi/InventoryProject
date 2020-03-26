
using System;
using UnityEngine;
using OMTB.Collection;
using UnityEngine.Events;
using OMTB.Interface;

namespace OMTB.Gameplay
{

    public class Inventory : ItemContainer
    { 

    public static Inventory Instance { get; private set; }



        protected override void Awake()
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

                base.Awake();

                DontDestroyOnLoad(gameObject);
            }
            else
                GameObject.Destroy(gameObject);
        }

        //#region CONFIGURATION
        //protected override ItemContainerConfig GetConfiguration()
        //{
        //    return new ItemContainerConfig() { Rows = 5, Columns = 8 };
        //}


        //#endregion
    }


}
