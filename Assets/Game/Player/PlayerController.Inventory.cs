using System.Collections.Generic;
using Game.InventorySystem.UI;
using InventoryDemo.Items;
using UnityEngine;

namespace InventoryDemo.Player
{
    public partial class PlayerController
    {
        [SerializeField] private InventoryMenuController inventoryController;
        [SerializeField] public ItemData DebugInventoryLoadItem;

        private void SetupInventory()
        {
            List<ItemData> items = LoadInventoryData();
            foreach (ItemData item in items)
            {
                // There shouldn't be any leftover from save file, unless we update item definitions over multiple versions
                inventory.AddItem(item, out ItemData _); 
            }
            
            SetupInventoryMenu(inventory.GetItems());
            SetupUIActions();
        }

        private List<ItemData> LoadInventoryData()
        {
            // TODO save/load
            List<ItemData> items = new ();
            ItemDefinition itemDefinition = ItemLibrary.instance.FindItem(DebugInventoryLoadItem.Data.Id);
            int amount = DebugInventoryLoadItem.Amount;
            
            items.Add(new ItemData
            {
                Amount = amount,
                Data = itemDefinition
            });

            return items;
        }

        private void SetupInventoryMenu(IReadOnlyList<ItemData> items)
        {
            inventoryController.Setup(inventory.Rows, inventory.Columns);
            for (int i = 0; i < items.Count; i++)
            {
                ItemData item = items[i];
                (int row, int col) = inventory.Slot1DTo2D(i);
                inventoryController.UpdateSlotAt(row, col, item);
            }
        }

        private void SetupUIActions()
        {
        }
    }
}