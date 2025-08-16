using UnityEngine;

namespace InventoryDemo.Items.ItemData
{
    [CreateAssetMenu(fileName = "UsableItem", menuName = "InventoryDemo/UsableItem")]
    public class UsableItem : ScriptableObject
    {
        public void Use()
        {
            Debug.Log("I was used!");
        }
    }
}