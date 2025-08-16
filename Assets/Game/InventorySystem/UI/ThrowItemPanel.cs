using UnityEngine;
using UnityEngine.EventSystems;

namespace InventoryDemo.InventorySystem.UI
{
    public class ThrowItemPanel : MonoBehaviour, IPointerClickHandler
    {
        public delegate void OnClickedDelegate();

        public event OnClickedDelegate OnClicked;

        private void OnDestroy() => OnClicked = null;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                OnClicked?.Invoke();
            }
        }
    }
}