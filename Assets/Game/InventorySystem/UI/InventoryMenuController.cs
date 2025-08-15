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

        private void Awake()
        {
            ClearAllChildren();
        }
        
        public void Setup(Inventory inventory, int rows, int columns)
        {
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
                slot.SetSlotIndex(i);
                slots.Add(slot);
            }
        }

        public void UpdateSlotAt(ItemData itemData, int index)
        {
            slots[index].Setup(itemData);
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

        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);
    }
}