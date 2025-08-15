using System;
using UnityEngine;

namespace InventoryDemo.Items
{
    public class EquipableItem : MonoBehaviour
    {
        [SerializeField] private GameObject pivot;
        [SerializeField] private Collider hitbox;
        public GameObject Pivot => pivot;

        private void Awake()
        {
            if (hitbox != null) hitbox.enabled = false;
        }

        public void Activate()
        {
            hitbox.enabled = true;
        }

        public void Deactivate()
        {
            hitbox.enabled = false;
        }
    }
}