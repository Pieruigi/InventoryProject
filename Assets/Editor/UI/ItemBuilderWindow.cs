using UnityEngine;
using UnityEditor;

using OMTB.Collection;

namespace OMTB.Editor
{


    public class ItemBuilderWindow : EditorWindow
    {
        static Vector2 winSize = new Vector2(800, 600);

        string basePath = "Assets/Resources/Items/";

        string weaponName;

        int weaponGrip = 0;
        string[] weaponGrips = new string[] { WeaponGrip.None.ToString(), WeaponGrip.OneHand.ToString(), WeaponGrip.TwoHands.ToString() };

        string armorName;

        int bodyPartSelected = 0;

        string[] bodyParts = new string[]
        {
                ArmorBodyPart.Head.ToString(),
                ArmorBodyPart.Chest.ToString(),
                ArmorBodyPart.Hands.ToString(),
                ArmorBodyPart.Legs.ToString(),
                ArmorBodyPart.Feet.ToString()
        };


        string categoryName;

        [MenuItem("Window/InventoryAsset")]
        public static void ShowWindow()
        {
            Vector2 size = Vector2.one * 800;
            EditorWindow.GetWindow<ItemBuilderWindow>().maxSize = winSize;
            EditorWindow.GetWindow<ItemBuilderWindow>().minSize = winSize;
            EditorWindow.GetWindow<ItemBuilderWindow>().maximized = true;
            EditorWindow.GetWindow<ItemBuilderWindow>("Items Builder");
        }

        
        private void OnGUI()
        {
            float width = winSize.x;
            float height = winSize.y;
            GUI.BeginGroup(new Rect(0f , 0f , width, height));
            ShowBox();
            CreateWeapon();
            

            GUI.EndGroup();

            GUI.BeginGroup(new Rect(0f, 20f, width, height));
            ShowBox();
            
            CreateArmor();

            GUI.EndGroup();

            GUI.BeginGroup(new Rect(0f, 40f, width, height));
            ShowBox();

            CreateCategory();

            GUI.EndGroup();
        }

        void ShowBox()
        {
            
        }

        void CreateWeapon()
        {
            
            weaponName = EditorGUILayout.TextField("Weapon Name", weaponName);
            weaponGrip = EditorGUILayout.Popup(weaponGrip, weaponGrips);
            if (GUILayout.Button("Create!"))
            {

                if (weaponName == null || "".Equals(weaponName))
                {
                    EditorUtility.DisplayDialog("Error!", "Specify a name for this weapon", "OK");
                    return;
                }



                Asset item = AssetBuilder.Build(OMTB.Configuration.ResourcesConfiguration.WeaponsPath, typeof(MeleeWeapon), 
                    new MeleeWeaponConfig()
                    {
                        Name = weaponName,
                        Grip = (WeaponGrip)weaponGrip,
                        MaxQuantityPerSlot = 5
                    });



            }
        }

        void CreateArmor()
        {
            //path = EditorGUILayout.TextField(basePath, path);

            armorName = EditorGUILayout.TextField("Armor Name", armorName);

            bodyPartSelected = EditorGUILayout.Popup(bodyPartSelected, bodyParts);

            if (GUILayout.Button("Create!"))
            {

                if (armorName == null || "".Equals(armorName))
                {
                    EditorUtility.DisplayDialog("Error!", "Specify a name for this piece of armor", "OK");
                    return;
                }

                Asset item = AssetBuilder.Build(OMTB.Configuration.ResourcesConfiguration.ArmorsPath, typeof(Armor),
                    new ArmorConfig()
                    {
                        Name = armorName,
                        BodyPart = (ArmorBodyPart)bodyPartSelected
                    });
                    


            }
        }

        void CreateCategory()
        {
            //path = EditorGUILayout.TextField(basePath, path);

            categoryName = EditorGUILayout.TextField("Category Name", categoryName);

            if (GUILayout.Button("Create!"))
            {

                if (categoryName == null || "".Equals(categoryName))
                {
                    EditorUtility.DisplayDialog("Error!", "Specify a name for this category", "OK");
                    return;
                }

                Asset item = AssetBuilder.Build(OMTB.Configuration.ResourcesConfiguration.CategoriesPath, categoryName, typeof(Category), new Config());



            }
        }

    }

}
