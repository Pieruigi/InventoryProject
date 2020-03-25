using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Collection;
using UnityEngine.UI;
using OMTB.Interface;

namespace OMTB.UI
{
    public class DraggedItemUI : MonoBehaviour
    {
        static DraggedItemUI instance;

        float scaleTime = 0.05f;

        IIndexable source;

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
            transform.position = Input.mousePosition;
        }

        //public void Init(int index, Item item, int quantity, float baseWidth, float baseHeight)
        public void StartDragging(IIndexable indexable, float width, float height)
        {
            source = indexable;
            // Get the image component
            Image image = GetComponent<Image>();
            // Set sprite and resize
            Item item = indexable.GetItem();
            image.sprite = item.Icon;

            (transform as RectTransform).sizeDelta = new Vector2(width, height);

        }

        public void StopDragging(IIndexable indexable)
        {
            //int quantity = 

            source = null;
            LeanTween.scale(gameObject, Vector2.zero, transform.localScale.magnitude * scaleTime).setDestroyOnComplete(true);
        }

        

    }

}
