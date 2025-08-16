using UnityEngine;
using UnityEngine.UI;

namespace Game.InventorySystem.UI
{
    public class SelectedItemInitializer : MonoBehaviour
    {
        [SerializeField] private GameObject container;
        [SerializeField] private Image icon;
        private void Awake()
        {
            SelectedItemManager.Instance.Setup(icon, container);
        }

        private void Update()
        {
            if (container.activeSelf)
            {
                RectTransform rectTransform = (RectTransform) container.transform;
                rectTransform.position = Input.mousePosition;
            }
        }
    }
}