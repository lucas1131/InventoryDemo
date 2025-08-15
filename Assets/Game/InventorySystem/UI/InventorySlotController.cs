using System;
using InventoryDemo.Items;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.InventorySystem.UI
{
    public class InventorySlotController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text amountText;

        private ItemData itemData;
        private int index;
        public ItemData ItemData => itemData;

        public void SetSlotIndex(int idx)
        {
            this.index = idx;
        }

        public void Setup(ItemData inItemData)
        {
            itemData = inItemData;

            icon.sprite = inItemData.Data.Icon;
            icon.enabled = icon.sprite != null;
            amountText.text = inItemData.Amount > 0 ? inItemData.Amount.ToString() : "";
        }

        public void OnPointerEnter(PointerEventData _) => TooltipManager.Instance.ShowTooltip(itemData.Data.Description);
        public void OnPointerExit(PointerEventData _) => TooltipManager.Instance.HideTooltip();
    }
}