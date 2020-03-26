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
            IItemContainer container = indexable.GetContainer() as IItemContainer;
            // Get the text
            Text text = GetComponent<Text>();

            // Check if there is an item at index
            if (!container.IsEmpty(index))
            {
                if (!container.GetItem(index).HasBigSlot)
                {
                    text.text = container.GetQuantity(index).ToString();
                }
                else
                {
                    // We show the quantity to the bottom right
                    Item item = container.GetItem(index);
                    Vector2 coords;
                    container.TryGetCoordsInBigSlot(index, out coords);

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
