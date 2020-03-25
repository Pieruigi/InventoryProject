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
            Text text = GetComponent<Text>();
            if (!Inventory.Instance.IsEmpty(index))
            {
                if (!Inventory.Instance.GetItem(index).HasBigSlot)
                {
                    text.text = Inventory.Instance.GetQuantity(index).ToString();
                }
                else
                {
                    // We show the quantity to the bottom right
                    Item item = Inventory.Instance.GetItem(index);
                    Vector2 coords;
                    Inventory.Instance.TryGetCoordsInBigSlot(index, out coords);

                    if(coords.x == item.SlotShape.x-1 && coords.y == item.SlotShape.y - 1)
                        text.text = Inventory.Instance.GetQuantity(index).ToString();

                }
            }
            else
                text.text = "";
        }
    }

}
