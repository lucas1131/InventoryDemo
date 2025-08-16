using TMPro;
using UnityEngine;

namespace InventoryDemo.InventorySystem.UI
{
    public class TooltipManager
    {
        private static TooltipManager instance;
        public static TooltipManager Instance => instance ??= new TooltipManager();

        private TMP_Text tooltipText;
        private GameObject container;

        public void Setup(TMP_Text text, GameObject inContainer)
        {
            tooltipText = text;
            container = inContainer;
        }

        public void ShowTooltip(string description)
        {
            // Item selection has priority over tooltip
            if (string.IsNullOrEmpty(description) || SelectedItemManager.Instance.IsShowing()) return;
            
            tooltipText.text = description;
            container.SetActive(true);
        }

        public void HideTooltip()
        {
            container.SetActive(false);
        }
    }
}