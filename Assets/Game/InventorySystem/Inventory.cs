using System;
using UnityEngine;
using InventoryDemo.Items;

namespace InventoryDemo.Inventory
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private int inventoryRows = 4; 
        [SerializeField] private int inventoryColumns = 8;
        
        private ItemData[] inventory;

        private void OnValidate()
        {
            inventory = new ItemData[inventoryRows*inventoryColumns];
        }

        private int Slot2DTo1D(int row, int column) => row * inventoryColumns + column;
        private (int row, int col) Slot1DTo2D(int index) => (index % inventoryColumns, index / inventoryColumns);

    

        public bool AddItem(ItemData item)
        {
            foreach (ItemData itemData in inventory)
            {
                if(itemData == item)
            }
            int idx = FindFirstEmptySlot();
            if (idx < 0) return false;
            
            inventory[idx] = item;
            return true;
        }
        
        public bool
    }
}