using UnityEngine;

namespace InventoryDemo.Items
{
    public class ItemPickuper : MonoBehaviour
    {
        public delegate void OnItemPickedUpEvent(PickableItem item);

        public event OnItemPickedUpEvent OnItemPickedUp;
            
        private void OnTriggerEnter(Collider other)
        {
            PickableItem item = other.gameObject.GetComponent<PickableItem>();
            if (item)
            {
                OnItemPickedUp?.Invoke(item);
            }
        }
    }
}