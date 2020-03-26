
using UnityEngine;
using OMTB.Collection;

namespace OMTB.Interface
{
    public interface IIndexable
    {
        int GetIndex(); // The index in the container

        void SetIndex(int index);

        IContainer GetContainer(); // The container 

        void SetContainer(IContainer container);


    }

}
