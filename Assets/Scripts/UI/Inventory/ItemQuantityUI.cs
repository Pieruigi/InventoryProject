using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Interface;
using UnityEngine.UI;
using OMTB.Gameplay;
using OMTB.Collection;

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
            {
                if (!Inventory.Instance.GetItem(indexable.GetIndex()).HasBigSlot)
                {
                    GetComponent<Text>().text = Inventory.Instance.GetQuantity(indexable.GetIndex()).ToString();
                }
                else
                {
                    // We show the quantity to the bottom right
                    Item item = Inventory.Instance.GetItem(indexable.GetIndex());
                    Vector2 coords;
                    Inventory.Instance.TryGetCoordsInBigSlot(indexable.GetIndex(), out coords);

                    if(coords.x == item.SlotShape.x-1 && coords.y == item.SlotShape.y - 1)
                        GetComponent<Text>().text = Inventory.Instance.GetQuantity(indexable.GetIndex()).ToString();

                    //if (Inventory.Instance.IsRoot(indexable.GetIndex()))
                    //    GetComponent<Text>().text = Inventory.Instance.GetQuantity(indexable.GetIndex()).ToString();
                    //else
                    //    GetComponent<Text>().text = "";
                }
            }
            else
                GetComponent<Text>().text = "";
        }
    }

}
