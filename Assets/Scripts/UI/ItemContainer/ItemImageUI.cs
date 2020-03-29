using UnityEngine;
using OMTB.Interface;
using UnityEngine.UI;
using OMTB.Gameplay;
using OMTB.Collection;
using OMTB.Utility;

namespace OMTB.UI
{
    public class ItemImageUI : MonoBehaviour//, IPointerEnterHandler, IPointerExitHandler
    {

        IIndexable<Item> indexable;

        // Start is called before the first frame update
        void Start()
        {
            indexable = GetComponentInParent<IIndexable<Item>>();

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
            IContainer<Item> container = indexable.GetContainer();
         
            // Get the image component
            Image image = GetComponent<Image>();
            
            // If there an item at index then show its icon
            if (!container.IsEmpty(index))
            {
                image.enabled = true;
               
                Item item = container.GetElement(index);

                // If is not a big slot then show the icon as it is
                if (!item.TakesMoreSlots)
                {
                    image.sprite = item.Icon;
                }
                else // It's a big slot, lets do some puzzle
                {
                    
                    if((container as IBigSlotContainer).IsRoot(index))
                    {
                        image.sprite = SpriteUtil.GetSprite(item.Icon.texture, (int)item.SlotShape.x, (int)item.SlotShape.y, 0, 0);
                    }
                    else
                    {

                        Vector2 coords;
                        
                        if ((container as IBigSlotContainer).TryGetCoordsInBigSlot(index, out coords))
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
