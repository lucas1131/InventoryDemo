using InventoryDemo.Items.ItemData;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace InventoryDemo.InventorySystem.UI
{
    public class InventorySlotController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text amountText;
        [SerializeField] private Color selectedColor = new(0.6f, 0.6f, 0.6f, 0.8f);

        private ItemData itemData;
        private int index;
        private InventoryMenuController inventoryController;
        public ItemData ItemData => itemData;

        public void Setup(InventoryMenuController inventoryController, int idx)
        {
            this.inventoryController = inventoryController;
            this.index = idx;
        }

        public void SetItemData(ItemData inItemData)
        {
            itemData = inItemData;

            icon.sprite = inItemData.Data.Icon;
            icon.enabled = icon.sprite != null;
            amountText.text = inItemData.Amount > 0 ? inItemData.Amount.ToString() : "";
        }
        
        public void SetIsSelected(bool isSelected)
        {
            icon.color = isSelected ? selectedColor : Color.white;
        }

        public void OnPointerEnter(PointerEventData _) => TooltipManager.Instance.ShowTooltip(itemData.Data.Description);
        public void OnPointerExit(PointerEventData _) => TooltipManager.Instance.HideTooltip();
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                inventoryController.OnSlotClicked(index, itemData);
            }
            else
            {
                inventoryController.OnTryUseItem(index, itemData);
            }
        }
    }
}