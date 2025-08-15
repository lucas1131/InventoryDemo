using System;
using TMPro;
using UnityEngine;

namespace Game.InventorySystem.UI
{
    public class TooltipInitializer : MonoBehaviour
    {
        [SerializeField] private TMP_Text tooltipText;
        private void Awake()
        {
            TooltipManager.Instance.Setup(tooltipText);
        }

        private void Update()
        {
            if (tooltipText.gameObject.activeSelf)
            {
                tooltipText.transform.position = Input.mousePosition;
            }
        }
    }
}