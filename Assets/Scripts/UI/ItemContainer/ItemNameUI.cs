using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Interface;
using UnityEngine.UI;
using OMTB.Gameplay;
using OMTB.Collection;

namespace OMTB.UI
{
    public class ItemNameUI : MonoBehaviour
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
            indexable.GetContainer().UnsetOnChanged(HandleContainerOnChanged);
        }

        void HandleContainerOnChanged()
        {
            CheckContainer();
        }

        void CheckContainer()
        {
            int index = indexable.GetIndex();
            IContainer<Item> container = indexable.GetContainer();

            if (!container.IsEmpty(index))
            {
                GetComponent<Text>().text = container.GetElement(index).Name;
                
            }
            else
            {
                GetComponent<Text>().text = "";
                
            }
                
        }
    }

}
