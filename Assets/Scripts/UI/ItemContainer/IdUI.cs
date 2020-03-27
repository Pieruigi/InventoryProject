﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OMTB.Interface;
using OMTB.Collection;
namespace OMTB.UI
{
    public class IdUI : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            GetComponent<Text>().text = GetComponentInParent<IIndexable<Item>>().GetIndex().ToString();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
