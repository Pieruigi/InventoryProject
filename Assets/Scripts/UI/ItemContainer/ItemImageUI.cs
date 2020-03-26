﻿using UnityEngine;
using OMTB.Interface;
using UnityEngine.UI;
using OMTB.Gameplay;
using OMTB.Collection;
using OMTB.Utility;

namespace OMTB.UI
{
    public class ItemImageUI : MonoBehaviour//, IPointerEnterHandler, IPointerExitHandler
    {

        IIndexable indexable;

        // Start is called before the first frame update
        void Start()
        {
            indexable = GetComponentInParent<IIndexable>();

            indexable.GetContainer().SetOnChanged(HandleContainerOnChanged);
                        
            CheckContainer();
           
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnDestroy()
        {
            if(indexable != null)
                indexable.GetContainer().UnsetOnChanged(HandleContainerOnChanged);
        }

        void HandleContainerOnChanged()
        {
            CheckContainer();
        }

        void CheckContainer()
        {
            int index = indexable.GetIndex();

            // Get the container
            IItemContainer container = indexable.GetContainer() as IItemContainer;
         
            // Get the image component
            Image image = GetComponent<Image>();
            
            // If there an item at index then show its icon
            if (!container.IsEmpty(index))
            {
                image.enabled = true;
               
                Item item = container.GetItem(index);

                // If is not a big slot then show the icon as it is
                if (!item.HasBigSlot)
                {
                    image.sprite = item.Icon;
                }
                else // It's a big slot, lets do some puzzle
                {
                    
                    if(container.IsRoot(index))
                    {
                        image.sprite = SpriteUtil.GetSprite(item.Icon.texture, (int)item.SlotShape.x, (int)item.SlotShape.y, 0, 0);
                    }
                    else
                    {

                        Vector2 coords;
                        
                        if (container.TryGetCoordsInBigSlot(index, out coords))
                            image.sprite = SpriteUtil.GetSprite(item.Icon.texture, (int)item.SlotShape.x, (int)item.SlotShape.y, (int)coords.x, (int)coords.y);


                    }
                }
                
            }
            else // No item has been found, hide icon
            {
                image.sprite = null;
                image.enabled = false;
            }
                
        }

  
    }

}
