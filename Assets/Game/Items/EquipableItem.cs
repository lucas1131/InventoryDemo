using System;
using Game.ResourceSystem;
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

        public void Activate() => hitbox.enabled = true;
        public void Deactivate() => hitbox.enabled = false;

        private void OnTriggerEnter(Collider other)
        {
            DamageableResource damageable = other.GetComponent<DamageableResource>();
            if (damageable == null) return;
            
            damageable.Damage(1);
        }
    }
}