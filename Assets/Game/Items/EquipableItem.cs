using UnityEngine;

namespace InventoryDemo.Items
{
    public class EquipableItem : MonoBehaviour
    {
        [SerializeField] private GameObject pivot;
        public GameObject Pivot => pivot;
        
        
    }
}