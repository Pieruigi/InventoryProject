using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB.Collection
{
    public class Config
    {

    }

    public abstract class Asset : ScriptableObject
    {
        protected abstract void Init(Config config);

        public static Asset Create(System.Type type, Config config)
        {
            Debug.Log("System.type:" + type);
            Asset a = ScriptableObject.CreateInstance(type) as Asset;
            a.Init(config);
            return a;
        }
    }

}
