using System;
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
         
        public delegate void OnInventorySlotUpdated(ItemData item, int row, int column);
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

        public int Slot2DTo1D(int row, int column) => row * inventoryColumns + column;
        public (int row, int col) Slot1DTo2D(int index) => (index % inventoryColumns, index / inventoryColumns);

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
            int leftovers = addedItem.Amount;
            int idx = FindItemIndex(addedItem);

            // Item already in inventory, increment amount
            if (idx >= 0)
            {
                int calculatedAmount = inventory[idx].Amount + addedItem.Amount;
                leftovers = calculatedAmount - inventory[idx].Data.MaxStack;
                inventory[idx].Amount = Mathf.Clamp(calculatedAmount, 0, inventory[idx].Data.MaxStack);
                leftoverItem.Amount = leftovers;

                BroadcastSlotUpdated(idx, inventory[idx]);
            }

            if (leftovers > 0)
            {
                Debug.Log($"Item {addedItem.Data.Name} hit max stack, trying to add x{leftovers} leftovers to another slot.");
                idx = FindFirstEmptySlot();
                // No more space for leftovers
                if (idx < 0)
                {
                    Debug.Log($"Inventory is full, cannot fit all items. Picked up x{addedItem.Amount-leftovers} {addedItem.Data.Name} and left: x{leftovers}.");
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

        private void BroadcastSlotUpdated(int idx, ItemData item)
        {
            (int row, int col) = Slot1DTo2D(idx);
            OnInventorySlotUpdatedEvent?.Invoke(item, row, col);
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
        
        public IReadOnlyList<ItemData> GetItems() => inventory;
    }
}