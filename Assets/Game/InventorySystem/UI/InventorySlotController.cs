using InventoryDemo.Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.InventorySystem.UI
{
    public class InventorySlotController : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text amountText;

        private ItemData itemData;
        private int index;
        private int column;
        public ItemData ItemData => itemData;

        public void SetSlotIndex(int index)
        {
            this.index = index;
        }

        public void Setup(ItemData inItemData)
        {
            itemData = inItemData;
            
            icon.sprite = inItemData.Data.Icon;
            icon.enabled = icon.sprite != null;
            amountText.text = inItemData.Amount > 0 ? inItemData.Amount.ToString() : "";
        }
    }
}