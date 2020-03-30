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

        bool IsEmpty(int index);

        T GetElement(int index);

        int GetQuantity(int index);

        int GetFreeRoom(int index, T item); // Returns the how many items can be stored of the same type, or zero

        int Insert(int index, T item, int quantity);

        int Insert(T item, int quantity);

        int Remove(int index, int quantity);

        int Move(int srcIndex, int dstIndex, int quantity);
    }

}
