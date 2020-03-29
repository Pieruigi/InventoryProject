using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Collection;


namespace OMTB.Interface
{
    
    public interface IBigSlotContainer: IContainer<Item>
    {
        
        bool IsRoot(int index);

       
        int GetRootIndex(int index);

        

        bool TryGetCoordsInBigSlot(int index, out Vector2 coords);

        //int Move(int srcIndex, int dstIndex, IItemContainer dstContainer, int quantity);
    }

}
