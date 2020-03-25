using UnityEngine;
using OMTB.Collection;
using OMTB.Gameplay;
using OMTB.Interface;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace OMTB.UI
{
    public class SlotUI : MonoBehaviour, IIndexable, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField]
        GameObject draggedItemPrefab; 

        int index;

        private static GameObject draggedItem;
        
        protected virtual void Awake()
        {
            index = transform.parent.childCount - 1;
        }

        // Start is called before the first frame update
        void Start()
        {
            if (!Inventory.Instance.IsEmpty(index))
            {
                Item item = Inventory.Instance.GetItem(index);
            }        
        }

        // Update is called once per frame
        void Update()
        {

        }

        public int GetIndex() { return index; }

        public bool IsEmpty()
        {
            return Inventory.Instance.IsEmpty(index);
        }

        public Item GetItem()
        {
            return Inventory.Instance.GetItem(index);
        }



        public int GetQuantity()
        {
            return Inventory.Instance.GetQuantity(index);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            //throw new System.NotImplementedException();
            Debug.Log(string.Format("Enter: {0}", gameObject.name));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            
            Debug.Log(string.Format("Exit: {0}", gameObject.name));
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            HideDraggedImage(draggedItem);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            ShowDraggedImage();
        }

        void ShowDraggedImage()
        {
            // If empty do nothing
            if (Inventory.Instance.IsEmpty(index))
                return;

            // Create the icon image we are going to drag aroung
            draggedItem = GameObject.Instantiate(draggedItemPrefab, transform.root, false);

            // Init
            Item item = Inventory.Instance.GetItem(index);
            float width = (transform as RectTransform).rect.width * item.SlotShape.x;
            float height = (transform as RectTransform).rect.height * item.SlotShape.y;

            draggedItem.GetComponent<DraggedItemUI>().StartDragging(this, width, height);
          
            
        }

        void HideDraggedImage(GameObject obj)
        {
            if (draggedItem == null)
                return;
            draggedItem.GetComponent<DraggedItemUI>().StopDragging(this);
        }


    }

}
