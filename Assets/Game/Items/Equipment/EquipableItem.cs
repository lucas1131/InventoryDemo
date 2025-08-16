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
        // TODO make a counterpart for the itemDefinition for equipmentItemDefinition that we can use to set default values like damage or even durability
        private void OnCollided(DamageableResource damageable) => damageable.Damage(1); 

        private void OnTriggerEnter(Collider other)
        {
            DamageableResource damageable = other.GetComponent<DamageableResource>();
            if(damageable) OnCollided(damageable);
        }
    }
}