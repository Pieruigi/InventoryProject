using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Interface;
using UnityEngine.UI;
using OMTB.Gameplay;

namespace OMTB.UI
{
    public class ItemNameUI : MonoBehaviour
    {
        

        // Start is called before the first frame update
        void Start()
        {
            Inventory.Instance.OnChanged += HandleInventoryOnChanged;
                        
            CheckInventory();
           
        }

        // Update is called once per frame
        void Update()
        {

        }

        void HandleInventoryOnChanged()
        {
            CheckInventory();
        }

        void CheckInventory()
        {
            int index = GetComponentInParent<IIndexable>().GetIndex();

            if (!Inventory.Instance.IsEmpty(index))
            {
                GetComponent<Text>().text = Inventory.Instance.GetItem(index).Name;
                
            }
            else
            {
                GetComponent<Text>().text = "";
                
            }
                
        }
    }

}
