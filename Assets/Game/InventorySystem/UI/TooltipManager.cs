using TMPro;

namespace Game.InventorySystem.UI
{
    public class TooltipManager
    {
        private static TooltipManager instance;
        public static TooltipManager Instance => instance ??= new TooltipManager();

        private TMP_Text tooltipText;

        public void Setup(TMP_Text text)
        {
            tooltipText = text;
        }

        public void ShowTooltip(string description)
        {
            tooltipText.text = description;
            tooltipText.gameObject.SetActive(true);
        }

        public void HideTooltip()
        {
            tooltipText.gameObject.SetActive(false);
        }
    }
}