using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Interface;

namespace OMTB.UI
{
    public class SlotProperties : MonoBehaviour
    {
        [SerializeField]
        GameObject leftReceiverPrefab;
        public GameObject LeftReceiverPrefab { get { return leftReceiverPrefab; } }

        [SerializeField]
        GameObject rightReceiverPrefab;
        public GameObject RightReceiverPrefab { get { return rightReceiverPrefab; } }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
