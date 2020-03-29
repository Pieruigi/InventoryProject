using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Gameplay;
using UnityEngine.UI;
using OMTB.Interface;
using OMTB.Collection;

namespace OMTB.UI
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField]
        GameObject slotPrefab;

        [SerializeField]
        Transform slotGroup;

        [SerializeField]
        Inventory container;

        GameObject[] slots;

        GameObject panel;

        public bool IsOpened { get { return panel.activeSelf; } }

        //public static InventoryUI Instance { get; private set; }

        

        private void Awake()
        {
            //if (Instance == null)
            //    Instance = this;
            //else
            //    Destroy(gameObject);
        }

        // Start is called before the first frame update
        void Start()
        {

            Init();

            Close();

            //CounterSliderUI.Instance.Show(1, 10, (int a) => { Debug.Log("Counter:" + a); }, () => { Debug.Log("Cancel"); });
        }


        public void Open()
        {
            panel.SetActive(true);
        }

        public void Close()
        {
            panel.SetActive(false);
        }

        #region PRIVATE
        void Init()
        {
            // Set the panel to open and close the inventory
            panel = transform.GetChild(0).gameObject;

            // Get inventory properties
            int rows = container.Rows;
            int columns = container.Columns;
            int tot = rows * columns;

            // Init array to store all the slots we are going to add
            slots = new GameObject[tot];

            // Set then number of rows and columns in the slot group grid layout
            GridLayoutGroup glg = slotGroup.GetComponent<GridLayoutGroup>();
            glg.constraint = GridLayoutGroup.Constraint.FixedRowCount;
            glg.constraintCount = rows;

            // Create slots from prefab
            for(int i=0; i<tot; i++)
            {
                GameObject slot = GameObject.Instantiate(slotPrefab, slotGroup);
                slots[i] = slot;

                // Set index and container
                slot.GetComponent<IIndexable<Item>>().SetContainer(container);
                slot.GetComponent<IIndexable<Item>>().SetIndex(i);
            }
        }
        #endregion
    }
}

