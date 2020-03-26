using UnityEngine;
using OMTB.Collection;
using System;
using UnityEditor;

namespace OMTB.Editor
{
    public abstract class AssetBuilder : MonoBehaviour 
    {
        

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {


        }

        public static Asset Build(string path, System.Type type, Config config)
        {
            return Build(path, GUID.Generate().ToString(), type, config);
        }

        public static Asset Build(string path, string assetName, System.Type type, Config config)
        {
            try
            {
                path = "Assets/Resources/" + path;
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }

                Asset asset = Asset.Create(type, config);

                AssetDatabase.CreateAsset(asset, path + assetName + ".asset");
                AssetDatabase.SaveAssets();

                EditorUtility.FocusProjectWindow();

                Selection.activeObject = asset;

                return asset;
            }
            catch (Exception e)
            {
                EditorUtility.DisplayDialog("ERROR!", e.Message, "OK");
                Debug.LogError(e);
                return null;
            }
        }
        
    }

}
