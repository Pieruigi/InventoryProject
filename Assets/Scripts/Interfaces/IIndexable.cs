
using UnityEngine;
using OMTB.Collection;

namespace OMTB.Interface
{
    public interface IIndexable<T>
    {
        int GetIndex(); // The index in the container

        void SetIndex(int index);

        IContainer<T> GetContainer(); // The container 

        void SetContainer(IContainer<T> container);


    }

}
