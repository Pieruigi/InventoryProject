using UnityEngine;
using OMTB.Collection;
using System;
using UnityEditor;

namespace OMTB.Editor
{
    public abstract class ItemBuilder : MonoBehaviour
    {


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
             

        }

        public static Item Build(string path, System.Type type, ItemConfig config)
        {
            try
            {
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }

               Item item = Item.Create(type, config);
                
                AssetDatabase.CreateAsset(item, path + "/" + GUID.Generate() + ".asset");
                AssetDatabase.SaveAssets();

                EditorUtility.FocusProjectWindow();

                Selection.activeObject = item;

                return item;
            }
            catch(Exception e)
            {
                EditorUtility.DisplayDialog("ERROR!", e.Message, "OK");
                return null;
            }

        }

        
    }

}
