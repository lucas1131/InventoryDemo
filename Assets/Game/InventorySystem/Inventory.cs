using System.Collections.Generic;
using UnityEngine;
using InventoryDemo.Items;

namespace InventoryDemo.InventorySystem
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private int inventoryRows = 4;
        [SerializeField] private int inventoryColumns = 8;

        public int Rows => inventoryRows;
        public int Columns => inventoryColumns;

        private ItemData[] inventory;

        public delegate void OnInventorySlotUpdated(ItemData item, int index);

        public event OnInventorySlotUpdated OnInventorySlotUpdatedEvent;

        private void Start()
        {
            inventory = new ItemData[inventoryRows * inventoryColumns];
        }

        private void OnDestroy()
        {
            OnInventorySlotUpdatedEvent = null;
        }

        private void OnValidate()
        {
            inventory = new ItemData[inventoryRows * inventoryColumns];
        }

        private int FindFirstEmptySlot()
        {
            for (int i = 0; i < inventory.Length; i++)
            {
                ItemData item = inventory[i];
                if (item.Data.Id <= 0) return i;
            }

            return -1;
        }

        private int FindItemIndex(ItemData item)
        {
            for (int i = 0; i < inventory.Length; i++)
            {
                if (inventory[i] == item)
                {
                    return i;
                }
            }

            return -1;
        }

        public bool AddItem(ItemData addedItem, out ItemData leftoverItem)
        {
            leftoverItem = addedItem;
            int idx = FindItemIndex(addedItem);
            int leftovers = StackItems(idx, addedItem, ref leftoverItem);

            if (leftovers > 0)
            {
                Debug.Log($"Item {addedItem.Data.Name} hit max stack, trying to add x{leftovers} leftovers to another slot.");
                idx = FindFirstEmptySlot();
                // No more space for leftovers
                if (idx < 0)
                {
                    Debug.Log(
                        $"Inventory is full, cannot fit all items. Picked up x{addedItem.Amount - leftovers} {addedItem.Data.Name} and left: x{leftovers}.");
                    return false;
                }

                leftoverItem.Amount = 0;
                inventory[idx] = addedItem;
                inventory[idx].Amount = leftovers;
                BroadcastSlotUpdated(idx, inventory[idx]);
            }

            Debug.Log($"Picked up x{addedItem.Amount} {addedItem.Data.Name}.");
            return true;
        }

        private int StackItems(int indexToStack, ItemData addedItem, ref ItemData leftoverItem)
        {
            int leftovers = addedItem.Amount;

            // Item already in inventory, increment amount
            if (indexToStack >= 0)
            {
                int calculatedAmount = inventory[indexToStack].Amount + addedItem.Amount;
                leftovers = calculatedAmount - inventory[indexToStack].Data.MaxStack;
                inventory[indexToStack].Amount = Mathf.Clamp(calculatedAmount, 0, inventory[indexToStack].Data.MaxStack);
                leftoverItem.Amount = Mathf.Max(leftovers, 0);

                BroadcastSlotUpdated(indexToStack, inventory[indexToStack]);
            }

            return leftovers;
        }

        private void BroadcastSlotUpdated(int idx, ItemData item)
        {
            OnInventorySlotUpdatedEvent?.Invoke(item, idx);
        }

        public bool RemoveItem(ItemData removedItem, bool shouldOnlyRemoveIfAmountIsEnough = false)
        {
            int idx = FindItemIndex(removedItem);
            if (idx < 0) return false;

            int leftovers = inventory[idx].Amount - removedItem.Amount;
            if (shouldOnlyRemoveIfAmountIsEnough && leftovers >= 0)
            {
                inventory[idx].Amount = leftovers;
            }
            else
            {
                inventory[idx].Amount = Mathf.Max(leftovers, 0);
            }

            if (inventory[idx].Amount <= 0)
            {
                inventory[idx] = new ItemData(); // Cleanup with empty item
            }

            BroadcastSlotUpdated(idx, inventory[idx]);
            return true;
        }

        public ItemData SwapItems((ItemData data, int idx) item1, (ItemData data, int idx) item2)
        {
            /*
             if different id, simply swap item1 with item2
             if same id, try stack item1 onto item2
             should return the amount of item leftover
                if stacked, then return should be the amount of item1
                if swapped, then return should be the amount of item2
                */
            if (item1.data.Data.Id == item2.data.Data.Id)
            {
                ItemData leftovers = item1.data;
                StackItems(item2.idx, item1.data, ref leftovers);
                return leftovers;
            }
            else
            {
                return SwapItemsInternal(item1, item2);
            }
        }

        private ItemData SwapItemsInternal((ItemData data, int idx) item1, (ItemData data, int idx) item2)
        {
            (inventory[item1.idx], inventory[item2.idx]) = (item2.data, item1.data);
            BroadcastSlotUpdated(item1.idx, item1.data);
            BroadcastSlotUpdated(item2.idx, item2.data);
            return item2.data;
        }

        public IReadOnlyList<ItemData> GetItems() => inventory;
    }
}