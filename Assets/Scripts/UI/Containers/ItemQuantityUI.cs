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
            IContainer<Item> container = indexable.GetContainer() as IContainer<Item>;
            // Get the text
            Text text = GetComponent<Text>();

            // Check if there is an item at index
            if (!container.IsEmpty(index))
            {
                if (!container.GetElement(index).TakesMoreSlots || !new List<System.Type>(container.GetType().GetInterfaces()).Contains(typeof(IBigSlotContainer)))
                {
                    text.text = container.GetQuantity(index).ToString();
                }
                else
                {
                    // We show the quantity to the bottom right
                    Item item = container.GetElement(index);
                    Vector2 coords;
                    (container as IBigSlotContainer).TryGetCoordsInBigSlot(index, out coords);

                    if(coords.x == item.SlotShape.x-1 && coords.y == item.SlotShape.y - 1)
                        text.text = container.GetQuantity(index).ToString();
                    else
                        text.text = "";
                }
            }
            else
                text.text = "";
        }
    }

}
