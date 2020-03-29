using UnityEngine;
using OMTB.Collection;
using OMTB.Gameplay;
using OMTB.Interface;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace OMTB.UI
{
    /**
     * You can use this class to represent slots in a lot of places, such as the inventory, the crafting system, the equipment.
     * */
    public class SlotUI : MonoBehaviour, IIndexable<Item>, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler,
                          IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        /**
         * You can set the receivers via inspector or adding the SlotProperties component to the slot parent.
         * */
        [SerializeField]
        GameObject leftReceiverPrefab;

        [SerializeField]
        GameObject rightReceiverPrefab;

        private static GameObject receiver;

        private static GameObject rightReceiver;

        int index;
        IContainer<Item> container;

        //private static GameObject selected;

        protected virtual void Awake()
        {
           
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {
            SlotProperties properties = GetComponentInParent<SlotProperties>();
            if(!leftReceiverPrefab && properties)
                leftReceiverPrefab = properties.LeftReceiverPrefab;
            if(!rightReceiverPrefab && properties)
                rightReceiverPrefab = properties.RightReceiverPrefab;

            if (!container.IsEmpty(index))
            {
                Item item = container.GetElement(index);
            }        
        }

        // Update is called once per frame
        void Update()
        {

        }

        
        public virtual int GetIndex() { return index; }

        public virtual void SetIndex(int index)
        {
            this.index = index;
        }

        public virtual IContainer<Item> GetContainer()
        {
            return container;
        }

        public virtual void SetContainer(IContainer<Item> container)
        {
            this.container = container as IContainer<Item>;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            HandleOnBeginDrag(eventData, eventData.pointerCurrentRaycast.gameObject);
        }

        public void OnDrag(PointerEventData eventData)
        {
            //Debug.Log(string.Format("Drag"));
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                HandleOnEndDrag(eventData, eventData.pointerCurrentRaycast.gameObject);
            }
                
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
           
        }

        public void OnPointerExit(PointerEventData eventData)
        {
           
        }

        public void OnPointerUp(PointerEventData eventData)
        {
           
        }

        public void OnPointerDown(PointerEventData eventData)
        {
           
        }

        void HandleOnBeginDrag(PointerEventData eventData, GameObject currentRaycast)
        {
            // I have already a receiver working
            if (receiver != null)
                return;

            // If empty do nothing
            if (container.IsEmpty(index))
                return;

            if (eventData.button == PointerEventData.InputButton.Left)
            {
                // Create the left receiver drag
                if(leftReceiverPrefab)
                    receiver = GameObject.Instantiate(leftReceiverPrefab, transform.root, false);
            }
            else if (eventData.button == PointerEventData.InputButton.Left)
            {
                // Create the icon image we are going to drag aroung
                if(rightReceiverPrefab)
                    receiver = GameObject.Instantiate(rightReceiverPrefab, transform.root, false);
            }
            if(receiver)
                receiver.GetComponent<IPointerReceiver>().StartDragging(gameObject, currentRaycast);
        }

        void HandleOnEndDrag(PointerEventData eventData, GameObject currentRaycast)
        {
   
            if (receiver != null)
                receiver.GetComponent<IPointerReceiver>().StopDragging(gameObject, currentRaycast);
            
        }



    }

}
