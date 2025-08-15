using System;
using UnityEngine;
using InventoryDemo.Items;

namespace InventoryDemo.InventorySystem
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private int inventoryRows = 4;
        [SerializeField] private int inventoryColumns = 8;

        private ItemData[] inventory;

        private void OnValidate()
        {
            inventory = new ItemData[inventoryRows * inventoryColumns];
        }

        private int Slot2DTo1D(int row, int column) => row * inventoryColumns + column;
        private (int row, int col) Slot1DTo2D(int index) => (index % inventoryColumns, index / inventoryColumns);

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
            }

            if (leftovers > 0)
            {
                idx = FindFirstEmptySlot();
                // No more space for leftovers
                if (idx < 0)
                {
                    leftoverItem.Amount = leftovers;
                    return false;
                }

                inventory[idx] = addedItem;
                inventory[idx].Amount = leftovers;
            }

            return true;
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

            return true;
        }
    }
}