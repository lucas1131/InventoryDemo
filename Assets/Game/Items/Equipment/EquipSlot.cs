using UnityEngine;

namespace InventoryDemo.Equipment
{
    public class EquipSlot : MonoBehaviour
    {
        [SerializeField] private GameObject socket;
        private GameObject equippedItem;

        public void Equip(EquipableItem item)
        {
            Unequip();

            if (!item) return;
            
            item.transform.SetParent(socket.transform, false);
            item.transform.localPosition = Vector3.zero;
            item.transform.localRotation = Quaternion.identity;
            equippedItem = item.gameObject;
        }

        public void Unequip()
        {
            if (equippedItem == null) return;
            
            Destroy(equippedItem);
            equippedItem = null;
        }

        public bool HasItemEquipped() => equippedItem != null;
    }
}