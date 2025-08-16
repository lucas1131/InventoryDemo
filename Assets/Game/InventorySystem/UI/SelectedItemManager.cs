using UnityEngine;
using UnityEngine.UI;

namespace Game.InventorySystem.UI
{
    public class SelectedItemManager
    {
        private static SelectedItemManager instance;
        public static SelectedItemManager Instance => instance ??= new SelectedItemManager();

        private Image icon;
        private GameObject container;

        public void Setup(Image inIcon, GameObject inContainer)
        {
            icon = inIcon;
            container = inContainer;
        }

        public void ShowItem(Sprite sprite)
        {
            if (sprite == null) return;
            
            icon.sprite = sprite;
            container.SetActive(true);
        }

        public void HideItem()
        {
            container.SetActive(false);
        }
    }
}