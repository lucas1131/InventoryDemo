using Game.SaveSystem;
using UnityEngine;

namespace InventoryDemo.Items
{
    public class PickableItem : MonoBehaviour
    {
        [SerializeField] private ItemData data;
        [SerializeField] private ItemAsset itemAsset;
        private bool queryDone = false;

        // Since we have no scene orchestrator to properly load save before scene, this querying will have to happen in Update so we have
        // time to load save data before destroying picked items
        private void LateUpdate()
        {
            if (queryDone) return;
            if (SaveManager.QueryPickedItems(data))
            {
                Destroy(gameObject);
                queryDone = true;
            }
        }

        public ItemData GetItemData() => data;
        public void SetAmount(int inAmount) => data.Amount = inAmount;

        public void Destroy()
        {
            SaveData saveData = SaveManager.GetCachedData();
            saveData.PickedItems.Add(data.InstanceId);
            Destroy(gameObject);
        }
    }
}