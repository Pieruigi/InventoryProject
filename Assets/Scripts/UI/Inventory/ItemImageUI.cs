using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Interface;
using UnityEngine.UI;
using OMTB.Gameplay;
using OMTB.Collection;
using OMTB.Utility;
using UnityEngine.EventSystems;

namespace OMTB.UI
{
    public class ItemImageUI : MonoBehaviour//, IPointerEnterHandler, IPointerExitHandler
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
            Image image = GetComponent<Image>();
            if (!Inventory.Instance.IsEmpty(index))
            {
                image.enabled = true;
                Item item = Inventory.Instance.GetItem(index);
                if (!item.HasBigSlot)
                {
                    image.sprite = item.Icon;
                }
                else
                {
                    if (Inventory.Instance.IsRoot(index))
                    {
                        image.sprite = SpriteUtil.GetSprite(item.Icon.texture, (int)item.SlotShape.x, (int)item.SlotShape.y, 0, 0);
                    }
                    else
                    {

                        Vector2 coords;
                        if (Inventory.Instance.TryGetCoordsInBigSlot(index, out coords))
                            image.sprite = SpriteUtil.GetSprite(item.Icon.texture, (int)item.SlotShape.x, (int)item.SlotShape.y, (int)coords.x, (int)coords.y);


                    }
                }
                
            }
            else
            {
                image.sprite = null;
                image.enabled = false;
            }
                
        }

        //public void OnPointerEnter(PointerEventData eventData)
        //{
        //    //throw new System.NotImplementedException();
        //    Debug.Log(string.Format("Enter: {0}", gameObject.name));
        //}

        //public void OnPointerExit(PointerEventData eventData)
        //{

        //    Debug.Log(string.Format("Exit: {0}", gameObject.name));
        //}
    }

}
