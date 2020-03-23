using UnityEngine;
using OMTB.Collection;
using OMTB.Gameplay;
using OMTB.Interface;

namespace OMTB.UI
{
    public class SlotUI : MonoBehaviour, IIndexable
    {
        int index;
        

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

    }

}
