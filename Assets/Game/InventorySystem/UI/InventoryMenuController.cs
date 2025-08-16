using System.Collections.Generic;
using InventoryDemo.InventorySystem;
using InventoryDemo.Items;
using UnityEngine;
using UnityEngine.UI;

namespace Game.InventorySystem.UI
{
    public class InventoryMenuController : MonoBehaviour
    {
        [SerializeField] private GridLayoutGroup inventoryGrid;
        [SerializeField] private InventorySlotController slotPrefab;
        
        private List<InventorySlotController> slots = new();
        private int totalRows;
        private int totalColumns;
        private Inventory inventory;
        private (ItemData itemData, int slotIndex)? currentlyHeldItem;

        private void Awake()
        {
            ClearAllChildren();
        }
        
        public void Setup(Inventory inventory, int rows, int columns)
        {
            this.inventory = inventory;
            inventory.OnInventorySlotUpdatedEvent += UpdateSlotAt;
            totalRows = rows;
            totalColumns = columns;
            RectTransform panelTransform = (RectTransform) inventoryGrid.transform;
            
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
            
            for (int i = 0; i < rows*columns; i++)
            {
                InventorySlotController slot = Instantiate(slotPrefab, panelTransform);
                slot.Setup(this, i);
                slots.Add(slot);
            }
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
            }
        }

        public void OnSlotClicked(int index, ItemData itemData)
        {
            if (currentlyHeldItem == null)
            {
                if (itemData.Amount <= 0) return; // Empty slot and we dont have any selection, nothing to do
                
                currentlyHeldItem = (itemData, index);
                slots[index].SetIsSelected(true);
                SelectedItemManager.Instance.ShowItem(itemData.Data.Icon);
            }
            else
            {
                // Held item and new slot are the same slot, deselect item and do nothing
                if (currentlyHeldItem.Value.slotIndex == index)
                {
                    DeSelectItem();
                    return;
                } 
                
                ItemData swappedItem = SwapItems(currentlyHeldItem.Value, index, itemData);
                if (swappedItem.Amount <= 0)
                {
                    // Stacked and no leftover
                    DeSelectItem();
                }
                else
                {
                    // We either have leftover items or simply swapped two different items
                    currentlyHeldItem = (swappedItem, currentlyHeldItem.Value.slotIndex);
                    SelectedItemManager.Instance.ShowItem(itemData.Data.Icon);
                }
            }
        }

        private void DeSelectItem()
        {
            if (currentlyHeldItem == null) return;
            
            slots[currentlyHeldItem.Value.slotIndex].SetIsSelected(false);
            currentlyHeldItem = null;
            SelectedItemManager.Instance.HideItem();
        }

        private ItemData SwapItems((ItemData itemData, int slotIndex) currentItem, int newItemIndex, ItemData newItemData)
        {
            return inventory.SwapItems(currentItem, (newItemData, newItemIndex));
        }
    }
}