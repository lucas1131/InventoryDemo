using System.Collections.Generic;
using InventoryDemo.Items.ItemData;
using UnityEditor;
using UnityEngine;

namespace InventoryDemo.Items.Editor
{
    [CustomEditor(typeof(ItemLibrary))]
    public class ItemLibraryCustomEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Populate"))
            {
                PopulateLibrary((ItemLibrary)target);
            }

            GUILayout.Space(10);
            DrawDefaultInspector();
        }

        private static void PopulateLibrary(ItemLibrary library)
        {
            string[] itemsGUIDs = AssetDatabase.FindAssets($"t:{nameof(ItemAsset)}");
            List<ItemAsset> items = new(itemsGUIDs.Length);

            foreach (string guid in itemsGUIDs)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                ItemAsset itemAsset = AssetDatabase.LoadAssetAtPath<ItemAsset>(path);
                if (itemAsset != null && itemAsset.IsValid())
                {
                    items.Add(itemAsset);
                }
                else
                {
                    Debug.LogError($"Found invalid item {itemAsset.name} ({path}) while trying to load all items for library.");
                }
            }

            items.Sort((asset1, asset2) => asset1.ItemDefinition.Id - asset2.ItemDefinition.Id);
            library.EditorOnly_SetItemAssetList(items);
            EditorUtility.SetDirty(library);
        }
    }
}