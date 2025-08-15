using System;
using TMPro;
using UnityEngine;

namespace Game.InventorySystem.UI
{
    public class TooltipInitializer : MonoBehaviour
    {
        [SerializeField] private GameObject container;
        [SerializeField] private TMP_Text tooltipText;
        private void Awake()
        {
            TooltipManager.Instance.Setup(tooltipText, container);
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