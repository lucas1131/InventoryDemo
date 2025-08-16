using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace InventoryDemo.Items.ItemData
{
    [CreateAssetMenu(fileName = "ItemLibrary", menuName = "InventoryDemo/ItemLibrary")]
    public class ItemLibrary : ScriptableObject
    {
        [SerializeField] private List<ItemAsset> items;
        private Dictionary<int, ItemAsset> itemTable;

        private static ItemLibrary instance;
        public static ItemLibrary Instance
        {
            get => instance ?? LoadInstance();
            private set => instance = value;
        }

        private const string Path = "ItemLibrary";

        private static ItemLibrary LoadInstance()
        {
            instance = Addressables.LoadAssetAsync<ItemLibrary>(Path).WaitForCompletion();

            if (instance == null)
            {
                Debug.LogError($"No instance of {nameof(ItemLibrary)} found at {Path}! Creating an empty one.");
                instance = CreateInstance<ItemLibrary>();
            }
            
            return instance;
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
                    Debug.LogError(
                        $"Duplicate item id ({itemAsset.ItemDefinition.Id}) found: '{itemAsset.name}' and '{table[itemAsset.ItemDefinition.Id].name}'");
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