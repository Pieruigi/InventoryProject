using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Collection;


namespace OMTB.Interface
{
    
    public interface IItemContainer: IContainer
    {
        //void SetOnChanged(UnityAction handle);

        bool IsEmpty(int index);

        bool IsRoot(int index);

        Item GetItem(int index);

        int GetQuantity(int index);

        int GetRootIndex(int index);

        int GetFreeRoom(int index, Item item); // Returns the how many items can be stored of the same type, or zero

        int Insert(int index, Item item, int quantity);

        int Remove(int index, int quantity);

        int Move(int srcIndex, int dstIndex, int quantity);

        int Move(int srcIndex, int dstIndex, IItemContainer dstContainer, int quantity);

        bool TryGetCoordsInBigSlot(int index, out Vector2 coords);


    }

}
