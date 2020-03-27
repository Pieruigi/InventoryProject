using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace OMTB.Interface
{
    public interface IContainer<T> 
    {
        void SetOnChanged(UnityAction handle);

        void UnsetOnChanged(UnityAction handle);
    }

}
