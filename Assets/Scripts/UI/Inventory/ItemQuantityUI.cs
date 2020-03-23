using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Interface;
using UnityEngine.UI;
using OMTB.Gameplay;

namespace OMTB.UI
{
    
    public class ItemQuantityUI : MonoBehaviour
    {
        IIndexable indexable;
        // Start is called before the first frame update
        void Start()
        {
            Inventory.Instance.OnChanged += HandleInventoryOnChanged;

            indexable = GetComponentInParent<IIndexable>();

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
            if (!Inventory.Instance.IsEmpty(indexable.GetIndex()))
                GetComponent<Text>().text = Inventory.Instance.GetQuantity(indexable.GetIndex()).ToString();
            else
                GetComponent<Text>().text = "0";
        }
    }

}
