﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Collection;
using UnityEngine.UI;
using OMTB.Interface;

namespace OMTB.UI
{
    /**
     * You can use this class in al lot of way to represents dragging items from one container to another, not only between slots of the very same container.
     * For example you could use it to move items between inventory and crafting system.
     * */
    public class ItemDraggerUI : MonoBehaviour, IPointerReceiver
    {
        [SerializeField]
        float baseWidth = 140f, baseHeight = 140f;

        static ItemDraggerUI instance;

        float scaleTime = 0.05f;

        IIndexable source;

        

        Vector2 mouseDisplacement;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                
            }
            else
                Destroy(gameObject);
        }

        // Start is called before the first frame update
        void Start()
        {
            transform.SetAsLastSibling();
            instance.transform.localScale = Vector2.zero;
            LeanTween.scale(gameObject, Vector2.one, scaleTime);
        }

        // Update is called once per frame
        void Update()
        {
            transform.position = Input.mousePosition;// + (Vector3)mouseDisplacement;
        }

        //public void Init(int index, Item item, int quantity, float baseWidth, float baseHeight)
        public void StartDragging(GameObject sender, GameObject currentRaycast)
        {
            // Store the source
            source = sender.GetComponent<IIndexable>();
            
            // Get the image component
            Image image = GetComponent<Image>();
          
            // Set icon and resize
            Item item = (source.GetContainer() as IItemContainer).GetItem(source.GetIndex());
            image.sprite = item.Icon;
            (transform as RectTransform).sizeDelta = new Vector2(baseWidth*item.SlotShape.x, baseHeight * item.SlotShape.y);
            (transform as RectTransform).pivot = new Vector2(0f, 1f);
            
        }

        public void StopDragging(GameObject sender, GameObject currentRaycast)
        {
           
            if(currentRaycast == null)
            {
                ExitDragMode();
                return;
            }

            // Try move to another slot
            IIndexable dest = currentRaycast.GetComponent<IIndexable>();
            
            if (dest != null)
            {
               
                // Get destination container
                IItemContainer dstContainer = dest.GetContainer() as IItemContainer;
                int dstIdx = dest.GetIndex();

                // Get source data
                IItemContainer srcContainer = source.GetContainer() as IItemContainer;
                int srcIdx = source.GetIndex();
                Item item = srcContainer.GetItem(source.GetIndex());

                // Try add the item to the destination container
                int quantity = 0;
                
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    bool open = true;
                    int q = dstContainer.GetFreeRoom(dstIdx, item);
                    if ((q <= 0) || ((dstContainer == srcContainer) && (dstIdx == srcIdx || srcContainer.GetRootIndex(srcIdx) == srcContainer.GetRootIndex(dstIdx))))
                        open = false;
                    
                    Debug.Log(string.Format("q:{0}", q));
                    if (open)
                    {
                        CounterSliderUI.Instance.Show(1, srcContainer.GetQuantity(srcIdx), (int a) =>
                        {
                            // Move items
                            srcContainer.Move(srcIdx, dstIdx, dstContainer, a);

                        }, () => { });
                    }
                    
                }
                else
                {
                    quantity = 1;
                    // Move items
                    srcContainer.Move(srcIdx, dstIdx, dstContainer, quantity);
                }

                
            }

            ExitDragMode();
            
        }

        void ExitDragMode()
        {
            source = null;
            LeanTween.scale(gameObject, Vector2.zero, transform.localScale.magnitude * scaleTime).setDestroyOnComplete(true);
        }



        public void PointerUp(GameObject sender, GameObject currentRaycast)
        {
        }

        public void PointerDown(GameObject sender, GameObject currentRaycast)
        {
        }

    }

}
