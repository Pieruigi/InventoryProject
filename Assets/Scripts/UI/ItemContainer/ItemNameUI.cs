using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Interface;
using UnityEngine.UI;
using OMTB.Gameplay;

namespace OMTB.UI
{
    public class ItemNameUI : MonoBehaviour
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

            if (!container.IsEmpty(index))
            {
                GetComponent<Text>().text = container.GetItem(index).Name;
                
            }
            else
            {
                GetComponent<Text>().text = "";
                
            }
                
        }
    }

}
