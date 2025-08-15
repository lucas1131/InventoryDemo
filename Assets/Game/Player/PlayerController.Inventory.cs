using System.Collections.Generic;
using System.Linq;
using Game.InventorySystem.UI;
using Game.SaveSystem;
using InventoryDemo.Items;
using UnityEngine;

namespace InventoryDemo.Player
{
    // This partial is going outside the scope of a player controller, this should become its own class
    public partial class PlayerController
    {
        [SerializeField] private InventoryMenuController inventoryController;

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

        private static List<ItemData> LoadInventoryData()
        {
            List<ItemData> items = new(SaveManager.Load().Items);
            for (int i = 0; i < items.Count; i++)
            {
                ItemData mutableItemData = items[i];
                mutableItemData.Data = ItemLibrary.Instance.FindItem(items[i].Data.Id);
                items[i] = mutableItemData;
            }

            return items;
        }

        private void SetupInventoryMenu(IReadOnlyList<ItemData> items)
        {
            inventoryController.Setup(inventory, inventory.Rows, inventory.Columns);
            for (int i = 0; i < items.Count; i++)
            {
                ItemData item = items[i];
                (int row, int col) = inventory.Slot1DTo2D(i);
                inventoryController.UpdateSlotAt(item, row, col);
            }
        }

        private void SetupUIActions()
        {
        }

        private void UpdateSaveWithInventoryData()
        {
            SaveData data = SaveManager.GetCachedData();
            data.Items = inventory.GetItems().ToList();
            SaveManager.Save(data);
        }
        
    }
}