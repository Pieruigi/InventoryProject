using System.Collections;
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

        static Transform parent;

        static ItemDraggerUI instance;

        float scaleTime = 0.05f;

        IIndexable<Item> source;

        

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
            if (parent == null)
                parent = GameObject.FindGameObjectWithTag("ForegroundCanvas").transform;
            transform.parent = parent.transform;
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
            source = sender.GetComponent<IIndexable<Item>>();
            
            // Get the image component
            Image image = GetComponent<Image>();
          
            // Set icon and resize
            Item item = (source.GetContainer() as IContainer<Item>).GetElement(source.GetIndex());
            image.sprite = item.Icon;
            (transform as RectTransform).sizeDelta = new Vector2(baseWidth*item.SlotShape.x, baseHeight * item.SlotShape.y);
            (transform as RectTransform).pivot = new Vector2(0f, 1f);
            
        }

        public void StopDragging(GameObject sender, GameObject currentRaycast)
        {

            if (currentRaycast == null)
            {
                ExitDragMode();
                return;
            }

            // Try move to another slot
            IIndexable<Item> dest = currentRaycast.GetComponent<IIndexable<Item>>();

            if (dest != null)
            {
                // Get destination data
                IContainer<Item> dstContainer = dest.GetContainer();
                int dstIdx = dest.GetIndex();
                Item dstItem = dstContainer.GetElement(dstIdx);

                // Get source data
                IContainer<Item> srcContainer = source.GetContainer();
                int srcIdx = source.GetIndex();
                Item srcItem = srcContainer.GetElement(source.GetIndex());


                if (srcContainer == dstContainer) /*********************** Same container ( ex. from inventory to inventory ) *********************/
                {
                    if (dstItem == null || dstItem == srcItem) // Destination is empty or contains the same item
                    {

                        int srcQ = srcContainer.GetQuantity(srcIdx);
                        int dstFreeRoom = srcContainer.GetFreeRoom(dstIdx, srcItem);
                        bool doNothing = false;

                        // If there is no room or I'm not moving the item at all then do nothing
                        bool multiSlotSupport = new List<System.Type>(srcContainer.GetType().GetInterfaces()).Contains(typeof(IBigSlotContainer));
                        if ((dstFreeRoom <= 0) || ((dstIdx == srcIdx || ( multiSlotSupport && (srcContainer as IBigSlotContainer).GetRootIndex(srcIdx) == (srcContainer as IBigSlotContainer).GetRootIndex(dstIdx)  ) ) ))
                            doNothing = false;

                        if (!doNothing)
                        {
                            int max = Mathf.Min(dstFreeRoom, srcQ);
                            Debug.Log("Max:" + max);
                            if (max > 1 && Input.GetKey(KeyCode.LeftControl))
                            {
                                CounterSliderUI.Instance.Show(1, max /*srcContainer.GetQuantity(srcIdx)*/, (int a) =>
                                {
                                    // Move items
                                    srcContainer.Move(srcIdx, dstIdx, a);

                                }, () => { });
                            }
                            else
                            {
                                max = 1;
                                srcContainer.Move(srcIdx, dstIdx, max);
                            }
                        }

                    }
                }
                else /********** Different containers *****************/
                {
                    if (dstItem == null || dstItem == srcItem) // Destination is empty or contains the same item
                    {
                        int srcQ = srcContainer.GetQuantity(srcIdx);
                        int dstFreeRoom = dstContainer.GetFreeRoom(dstIdx, srcItem);
                        bool doNothing = false;

                        if (dstFreeRoom <= 0)
                            doNothing = true;

                        if (!doNothing)
                        {
                            int max = Mathf.Min(dstFreeRoom, srcQ);
                            Debug.Log("Max:" + max);
                            if (max > 1 && Input.GetKey(KeyCode.LeftControl))
                            {
                                CounterSliderUI.Instance.Show(1, max /*srcContainer.GetQuantity(srcIdx)*/, (int a) =>
                                {
                                    // Move items
                                    int q = dstContainer.Insert(dstIdx, srcItem, max);
                                    if (q > 0)
                                        srcContainer.Remove(srcIdx, q);

                                }, () => { });
                            }
                            else
                            {
                                max = 1;
                                // Move items
                                int q = dstContainer.Insert(dstIdx, srcItem, max);
                                if (q > 0)
                                    srcContainer.Remove(srcIdx, q);
                                
                            }
                        }
                    }
                    else // Different items between different containers
                    {
                        int srcQ = srcContainer.GetQuantity(srcIdx);
                        int dstFreeRoom = dstContainer.GetFreeRoom(dstIdx, srcItem);
                        bool doNothing = false;

                        if (!doNothing)
                        {
                            int max = Mathf.Min(dstFreeRoom, srcQ);
                            Debug.Log("Max:" + max);
                            if (max > 1 && Input.GetKey(KeyCode.LeftControl))
                            {
                                CounterSliderUI.Instance.Show(1, max /*srcContainer.GetQuantity(srcIdx)*/, (int a) =>
                                {
                                    // Move items
                                    TrySwitch(srcIdx, dstIdx, srcItem, dstItem, max, srcContainer, dstContainer);


                                }, () => { });
                            }
                            else
                            {
                                max = 1;
                                TrySwitch(srcIdx, dstIdx, srcItem, dstItem, max, srcContainer, dstContainer);
       
                            }
                        }
                    }
                }
            }

            ExitDragMode();

        }

        private void TrySwitch(int srcIdx, int dstIdx, Item srcItem, Item dstItem, int quantity, IContainer<Item> srcContainer, IContainer<Item> dstContainer)
        {
            // Remove items
            dstContainer.Remove(dstIdx, quantity);
            srcContainer.Remove(srcIdx, quantity);

            // Switch items
            int q = srcContainer.Insert(dstItem, quantity);
            if (q == 0)
            {
                dstContainer.Insert(dstIdx, dstItem, quantity);
                srcContainer.Insert(srcIdx, srcItem, quantity);
            }
            else
            {
                q = dstContainer.Insert(dstIdx, srcItem, quantity);
                if (q == 0)
                {
                    dstContainer.Insert(dstIdx, dstItem, quantity);
                    srcContainer.Insert(srcIdx, srcItem, quantity);
                }
            }
        }

        //public void StopDragging(GameObject sender, GameObject currentRaycast)
        //{

        //    if (currentRaycast == null)
        //    {
        //        ExitDragMode();
        //        return;
        //    }

        //    // Try move to another slot
        //    IIndexable<Item> dest = currentRaycast.GetComponent<IIndexable<Item>>();

        //    if (dest != null)
        //    {

        //        // I need a controller to define behaviour when moving items from one container to another
        //        ///////////////////////////////////////////

        //        // Get destination container
        //        IContainer<Item> dstContainer = dest.GetContainer();
        //        int dstIdx = dest.GetIndex();

        //        // Get source data
        //        IContainer<Item> srcContainer = source.GetContainer();
        //        int srcIdx = source.GetIndex();
        //        Item item = srcContainer.GetElement(source.GetIndex());

        //        // Try add the item to the destination container
        //        int quantity = 0;

        //        if (Input.GetKey(KeyCode.LeftControl))
        //        {
        //            int srcQ = srcContainer.GetQuantity(srcIdx);
        //            bool open = true;

        //            int q = dstContainer.GetFreeRoom(dstIdx, item);

        //            // If there is no room or I'm not moving the item at all then do nothing
        //            if ((q <= 0) || ((dstContainer == srcContainer) && (dstIdx == srcIdx || (srcContainer as IBigSlotContainer).GetRootIndex(srcIdx) == (srcContainer as IBigSlotContainer).GetRootIndex(dstIdx))))
        //                open = false;

        //            // If there is free room but only for a single object you don't need to open the quantity selector panel, then simply add the object ( this
        //            // works well for the equipment for example )
        //            if (q == 1 || srcQ == 1)
        //            {
        //                open = false;
        //            }

        //            Debug.Log(string.Format("q:{0}", q));
        //            if (open)
        //            {
        //                int max = Mathf.Min(q, srcQ);

        //                CounterSliderUI.Instance.Show(1, max /*srcContainer.GetQuantity(srcIdx)*/, (int a) =>
        //                {
        //                    // Move items
        //                    srcContainer.Move(srcIdx, dstIdx, a);

        //                }, () => { });

        //            }

        //        }
        //        else
        //        {

        //            quantity = 1;
        //            // Move items
        //            srcContainer.Move(srcIdx, dstIdx, quantity);



        //        }


        //    }

        //    ExitDragMode();

        //}

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
