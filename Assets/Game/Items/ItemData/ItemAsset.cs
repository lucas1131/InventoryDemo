using UnityEngine;

namespace InventoryDemo.Items
{
    [CreateAssetMenu(fileName = "ItemAsset", menuName = "InventoryDemo/ItemAsset")]
    public class ItemAsset : ScriptableObject
    {
        public ItemDefinition ItemDefinition;

        public bool IsValid() => ItemDefinition.IsValid();
    }
}