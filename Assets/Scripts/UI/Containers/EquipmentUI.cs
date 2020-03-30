using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Interface;
using OMTB.Collection;
using UnityEngine.Events;
using OMTB.Container;

namespace OMTB.UI
{
    public class EquipmentUI: MonoBehaviour
    {
        [SerializeField]
        SlotUI[] slots;

        [SerializeField]
        Equipment container;

        GameObject panel;

        // Start is called before the first frame update
        void Start()
        {
            Init();
            Close();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Open()
        {
            panel.SetActive(true);
        }

        public void Close()
        {
            panel.SetActive(false);
        }


        void Init()
        {
            panel = transform.GetChild(0).gameObject;

            // Init slots
            for(int i=0; i<slots.Length; i++)
            {
                SlotUI slot = slots[i];
                slot.SetIndex(i);
                slot.SetContainer(container);
            }
        }

    }

}
