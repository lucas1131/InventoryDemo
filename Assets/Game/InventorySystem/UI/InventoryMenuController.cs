using System.Collections.Generic;
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

        public void Setup(int rows, int columns)
        {
            totalRows = rows;
            totalColumns = columns;
            RectTransform panelTransform = (RectTransform) inventoryGrid.transform;
            
            float panelWidth = panelTransform.rect.width;
            float panelHeight = panelTransform.rect.width;
            
            float usableWidth = panelWidth + inventoryGrid.spacing.x*(columns - 1) + inventoryGrid.padding.left + inventoryGrid.padding.right;
            float usableHeight = panelHeight + inventoryGrid.spacing.y*(rows - 1) + inventoryGrid.padding.top + inventoryGrid.padding.bottom;
            
            inventoryGrid.cellSize = new Vector2(usableWidth/columns, usableHeight/rows);

            slots = new List<InventorySlotController>(totalRows * totalColumns);
            
            for (int i = 0; i < rows*columns; i++)
            {
                slots.Add(Instantiate(slotPrefab, panelTransform));
            }
        }

        public void UpdateSlotAt(int row, int column, ItemData itemData)
        {
            slots[row * totalRows + column].Setup(itemData);
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
    }
}