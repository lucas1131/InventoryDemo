using UnityEngine;

namespace InventoryDemo.Items
{
    public class PickableItem : MonoBehaviour
    {
        [SerializeField] private int amount;
        [SerializeField] private ItemAsset itemAsset;

        public ItemData GetItemData() => new() { Amount = amount, Data = itemAsset.ItemDefinition };

        public void Destroy() => Destroy(gameObject);
    }
}