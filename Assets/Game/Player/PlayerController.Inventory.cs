using System.Collections.Generic;
using InventoryDemo.InventorySystem.UI;
using InventoryDemo.Items.ItemData;
using InventoryDemo.SaveSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InventoryDemo.Player
{
    // This partial is going outside the scope of a player controller, this should become its own class
    // This is specially bad here because im serializing the InventoryMenuController, but as this is a UI element, is doesnt exists inside
    // the player prefab, which means we cant assign it in editor, and we break prefab self-containment.
    // For now, just to same time, this will be a hard reference in scene but this is a problem that should be addressed.
    public partial class PlayerController
    {
        [SerializeField] private InventoryMenuController inventoryController; // Bad hard reference to another object

        private void SetupInventory()
        {
            List<ItemData> items = LoadInventoryData();
            inventory.LoadInventory(items);
            SetupInventoryMenu(inventory.GetItems());
            SetupUIActions();
            CloseInventory();
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
                inventoryController.UpdateSlotAt(item, i);
            }
        }

        private void SetupUIActions()
        {
            actionAsset.FindActionMap("Player").FindAction("Interact").performed += ToggleInventory;
        }

        private void ToggleInventory(InputAction.CallbackContext _)
        {
            if (inventoryController.IsShowing)
            {
                CloseInventory();
            }
            else
            {
                OpenInventory();
            }
        }

        private void OpenInventory()
        {
            cachedAttackAction.Disable();
            inventoryController.Show();
            cameraController.LockRotationAndShowMouse();
        }
        
        private void CloseInventory()
        {
            cachedAttackAction.Enable();
            inventoryController.Hide();
            cameraController.UnlockRotationAndHideMouse();
        }
    }
}