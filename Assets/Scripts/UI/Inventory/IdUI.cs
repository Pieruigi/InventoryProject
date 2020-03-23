using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OMTB.Interface;

namespace OMTB.UI
{
    public class IdUI : MonoBehaviour
    {
        IIndexable slotUI;

        // Start is called before the first frame update
        void Start()
        {
            GetComponent<Text>().text = GetComponentInParent<IIndexable>().GetIndex().ToString();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
