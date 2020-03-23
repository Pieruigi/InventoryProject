using UnityEngine;
using UnityEditor;

using OMTB.Collection;

namespace OMTB.Editor
{
    public class ItemBuilderWindow : EditorWindow
    {
        string path = "Test";
        string basePath = "Assets/Resources/Items/";

        string shortDesc;

        [MenuItem("Window/InventoryAsset")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow<ItemBuilderWindow>("Item Builder");
        }

        
        private void OnGUI()
        {
            path = EditorGUILayout.TextField(basePath, path);

            shortDesc = EditorGUILayout.TextField("Short Description", shortDesc);

            if (GUILayout.Button("Create!"))
            {

                if(shortDesc == null || "".Equals(shortDesc))
                {
                    EditorUtility.DisplayDialog("Error!", "Specify a short description", "OK");
                    return;
                }

                Item item = ItemBuilder.Build(basePath + path, typeof(TestItem), new TestItemConfig()
                {
                    Name = shortDesc,
                    MaxQuantityPerSlot = 20,
                    MultipleSlots = false
                });



            }
        }

        
    }

}
