using System;
using System.Collections.Generic;
using InventoryDemo.Items.ItemData;
using UnityEngine;
using UnityEngine.UI;

namespace InventoryDemo.InventorySystem.UI
{
    public class InventoryMenuController : MonoBehaviour
    {
        public delegate void OnEquipItemDelegate(ItemData itemData);
        public event OnEquipItemDelegate OnEquipItem;
        
        [SerializeField] private GridLayoutGroup inventoryGrid;
        [SerializeField] private InventorySlotController slotPrefab;
        [SerializeField] private ThrowItemPanel throwItemPanel;

        private List<InventorySlotController> slots = new();
        private int totalRows;
        private int totalColumns;
        private Inventory inventory;
        private (ItemData itemData, int slotIndex)? currentlyHeldItem;

        private void Awake()
        {
            ClearAllChildren();
        }

        private void OnDestroy()
        {
            OnEquipItem = null;
        }

        public void Setup(Inventory inventory, int rows, int columns)
        {
            this.inventory = inventory;
            inventory.OnInventorySlotUpdatedEvent += UpdateSlotAt;
            throwItemPanel.OnClicked += ThrowSelectedItem;
            totalRows = rows;
            totalColumns = columns;
            RectTransform panelTransform = (RectTransform)inventoryGrid.transform;

            float panelWidth = panelTransform.rect.width;
            float panelHeight = panelTransform.rect.width;

            Vector2 totalPadding = new(
                inventoryGrid.spacing.x * (columns - 1) + inventoryGrid.padding.left + inventoryGrid.padding.right,
                inventoryGrid.spacing.y * (rows - 1) + inventoryGrid.padding.top + inventoryGrid.padding.bottom);

            float usableWidth = panelWidth - totalPadding.x;
            float usableHeight = panelHeight - totalPadding.y;

            // Fit cell
            float size = Mathf.Min(usableWidth / columns, usableHeight / rows);
            inventoryGrid.cellSize = new Vector2(size, size);

            slots = new List<InventorySlotController>(totalRows * totalColumns);

            for (int i = 0; i < rows * columns; i++)
            {
                InventorySlotController slot = Instantiate(slotPrefab, panelTransform);
                slot.Setup(this, i);
                slots.Add(slot);
            }
        }

        private void ThrowSelectedItem()
        {
            if (currentlyHeldItem == null) return;

            Vector3 playerForward = inventory.gameObject.transform.forward + inventory.gameObject.transform.up*0.5f; // aim a little bit higher
            Vector3 spawnOrigin = inventory.gameObject.transform.position;
            const float spawnOffset = 2;
            Vector3 spawnLocation = spawnOrigin + playerForward * spawnOffset;
                
            GameObject item = Instantiate(currentlyHeldItem.Value.itemData.Data.PickableItemPrefab, spawnLocation, Quaternion.identity);
            
            // Inventory is attached to player, so this should throw forward
            Rigidbody rb = item.GetComponent<Rigidbody>();
            if (rb)
            {
                rb.AddForce(playerForward * 3f, ForceMode.VelocityChange);
            }
            inventory.RemoveItemAt(currentlyHeldItem.Value.slotIndex);
            inventory.SaveInventory();
            DeSelectItem();
        }

        public void UpdateSlotAt(ItemData itemData, int index)
        {
            slots[index].SetItemData(itemData);
        }

        private void ClearAllChildren()
        {
            foreach (InventorySlotController slot in slots)
            {
                Destroy(slot.gameObject);
            }

            // Make sure to clear any leftover preview, they wont exist in slots list
            foreach (RectTransform child in inventoryGrid.transform)
            {
                Destroy(child.gameObject);
            }
        }

        public bool IsShowing => gameObject.activeSelf;
        public void Show() => gameObject.SetActive(true);

        public void Hide()
        {
            gameObject.SetActive(false);
            if (currentlyHeldItem != null)
            {
                slots[currentlyHeldItem.Value.slotIndex].SetIsSelected(false);
                currentlyHeldItem = null;
                SelectedItemManager.Instance.HideItem();
            }
        }

        public void OnSlotClicked(int index, ItemData itemData)
        {
            if (currentlyHeldItem == null)
            {
                if (itemData.Amount <= 0) return; // Empty slot and we dont have any selection, nothing to do

                SelectSlot(index, itemData);
            }
            else
            {
                // Held item and new slot are the same slot, deselect item and do nothing
                if (currentlyHeldItem.Value.slotIndex == index)
                {
                    DeSelectItem();
                    return;
                }

                HandleSwap(currentlyHeldItem.Value, (itemData, index));
            }
        }

        private void SelectSlot(int index, ItemData itemData)
        {
            currentlyHeldItem = (itemData, index);
            slots[index].SetIsSelected(true);
            SelectedItemManager.Instance.ShowItem(itemData.Data.Icon);
        }

        private void HandleSwap((ItemData itemData, int slotIndex) item1, (ItemData itemData, int slotIndex) item2)
        {
            ItemData swappedItem = inventory.SwapItems(item1, item2);
            if (swappedItem.Amount <= 0)
            {
                // Stacked and no leftover
                DeSelectItem();
            }
            else
            {
                DeSelectItem();
                if (swappedItem.Data.Id == item1.itemData.Data.Id)
                {
                    // In this if, we stacked. There's nothing special to do for swap.
                    // This should be re-selecting same slot but with updated data. 
                    SelectSlot(item1.slotIndex, swappedItem);
                }
            }

            inventory.SaveInventory();
        }

        private void DeSelectItem()
        {
            if (currentlyHeldItem == null) return;

            slots[currentlyHeldItem.Value.slotIndex].SetIsSelected(false);
            currentlyHeldItem = null;
            SelectedItemManager.Instance.HideItem();
        }

        public void OnTryUseItem(int index, ItemData itemData)
        {
            if (itemData.Data.IsEquippable)
            {
                OnEquipItem?.Invoke(itemData);
            } 
            else if (itemData.Data.UsableItemBehaviour != null)
            {
                itemData.Data.UsableItemBehaviour.Use();
                inventory.RemoveAmountAt(index, 1);
                inventory.SaveInventory();
            }
        }
    }
}