using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace InventoryDemo.Items.ItemData
{
    [CreateAssetMenu(fileName = "ItemLibrary", menuName = "InventoryDemo/ItemLibrary")]
    public class ItemLibrary : ScriptableObject
    {
        [SerializeField] private List<ItemAsset> items;
        private Dictionary<int, ItemAsset> itemTable;

        public static ItemLibrary Instance { get; private set; }

        private static string path = "Assets/Content/Items/ItemDataAssets/ItemLibrary.asset";
        private void OnEnable()
        {
            Instance = AssetDatabase.LoadAssetAtPath<ItemLibrary>(path);

            if (Instance == null)
            {
                Debug.LogError($"No instance of {nameof(ItemLibrary)} found at {path}!");
            }
        }

        public ItemDefinition FindItem(int id)
        {
            itemTable ??= ConstructLookupTable();

            if (itemTable.TryGetValue(id, out ItemAsset item))
            {
                return item.ItemDefinition;
            }

            return new ItemDefinition();
        }

        private Dictionary<int, ItemAsset> ConstructLookupTable()
        {
            Dictionary<int, ItemAsset> table = new();
            foreach (ItemAsset itemAsset in items)
            {
                if (!table.TryAdd(itemAsset.ItemDefinition.Id, itemAsset))
                {
                    Debug.LogError($"Duplicate item id ({itemAsset.ItemDefinition.Id}) found: '{itemAsset.name}' and '{table[itemAsset.ItemDefinition.Id].name}'");
                }
            }

            return table;
        }

#if UNITY_EDITOR
        public void EditorOnly_SetItemAssetList(List<ItemAsset> inItems)
        {
            items = inItems;
            ConstructLookupTable(); // this is mostly for the LogError in case we have duplicates
        }
#endif
    }
}