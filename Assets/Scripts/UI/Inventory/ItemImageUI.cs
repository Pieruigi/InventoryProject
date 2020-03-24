using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Interface;
using UnityEngine.UI;
using OMTB.Gameplay;
using OMTB.Collection;
using OMTB.Utility;

namespace OMTB.UI
{
    public class ItemImageUI : MonoBehaviour
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
                GetComponent<Image>().enabled = true;
                Item item = Inventory.Instance.GetItem(indexable.GetIndex());
                if (!item.HasBigSlot)
                {
                    GetComponent<Image>().sprite = item.Icon;
                }
                else
                {
                    if (Inventory.Instance.IsRoot(indexable.GetIndex()))
                    {
                        GetComponent<Image>().sprite = SpriteUtil.GetSprite(item.Icon.texture, (int)item.SlotShape.x, (int)item.SlotShape.y, 0, 0);
                    }
                    else
                    {

                        Vector2 coords;
                        if (Inventory.Instance.TryGetCoordsInBigSlot(indexable.GetIndex(), out coords))
                            GetComponent<Image>().sprite = SpriteUtil.GetSprite(item.Icon.texture, (int)item.SlotShape.x, (int)item.SlotShape.y, (int)coords.x, (int)coords.y);


                    }
                }
                
            }
            else
            {
                GetComponent<Image>().sprite = null;
                GetComponent<Image>().enabled = false;
            }
                
        }
    }

}
