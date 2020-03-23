using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.UI;
using UnityEngine.UI;
using OMTB.Gameplay;
using OMTB.Collection;


public class _InventoryTest : MonoBehaviour
{
    [SerializeField]
    InputField singleAddName;

    [SerializeField]
    InputField singleAddCount;

    [SerializeField]
    Button addBtn;

    [SerializeField]
    Button removeBtn;

    List<TestItem> items;

    [SerializeField]
    InputField commonSlot;


    Item item;

    int slot = -1;
    int quantity;

    // Start is called before the first frame update
    void Start()
    {
        
        addBtn.onClick.AddListener( HandleAddOnClick);
        removeBtn.onClick.AddListener(HandleDelOnClick);

        // Load all the resources
        items = new List<TestItem>( Resources.LoadAll<TestItem>("Items/Test"));
        Debug.Log("Resources.Count:" + items.Count); 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (InventoryUI.Instance.IsOpened)
                InventoryUI.Instance.Close();
            else
                InventoryUI.Instance.Open();
        }
    }

    void HandleAddOnClick()
    {
        Debug.Log(singleAddName.text.ToLower());
        SetData();
        int count = 0;
        if (slot < 0)
            count = Inventory.Instance.Insert(item, quantity);
        else
            count = Inventory.Instance.Insert(slot, item, quantity);
        Debug.Log("Added:" + count);

    }

    void HandleDelOnClick()
    {
        Debug.Log(singleAddName.text.ToLower());
        SetData();
        int count = 0;
        if (slot < 0)
            count = Inventory.Instance.Remove(item, quantity);
        else
            count = Inventory.Instance.Remove(slot, quantity);
        Debug.Log("removed:" + count);

    }

    void SetData()
    {
        item = items.Find(i => i.Name.ToLower() == singleAddName.text.ToLower());

        quantity = int.Parse(singleAddCount.text);
        slot = -1;
        if (commonSlot.text != "")
            slot = int.Parse(commonSlot.text);
    }
}
