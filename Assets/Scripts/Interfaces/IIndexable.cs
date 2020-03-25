
using UnityEngine;
using OMTB.Collection;

namespace OMTB.Interface
{
    public interface IIndexable
    {
        int GetIndex();

        Item GetItem();

        bool IsEmpty();

        int GetQuantity();

    }

}
