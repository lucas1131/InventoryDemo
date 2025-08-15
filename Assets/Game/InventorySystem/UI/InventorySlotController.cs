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
        public ItemData ItemData => itemData;

        public void Setup(ItemData inItemData)
        {
            itemData = inItemData;
            
            icon.sprite = inItemData.Data.Icon;
            amountText.text = inItemData.Amount > 0 ? inItemData.Amount.ToString() : "";
        }
    }
}