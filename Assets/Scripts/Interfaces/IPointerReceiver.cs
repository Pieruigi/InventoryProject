using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB.Interface
{
    public interface IPointerReceiver
    {
        void StartDragging(GameObject sender, GameObject currentRaycast);

        void StopDragging(GameObject sender, GameObject currentRaycast);

        void PointerUp(GameObject sender, GameObject currentRaycast);

        void PointerDown(GameObject sender, GameObject currentRaycast);
    }

}
