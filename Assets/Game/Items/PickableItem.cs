using System;
using Game.SaveSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace InventoryDemo.Items
{
    public class PickableItem : MonoBehaviour
    {
        [SerializeField] private ItemData item;
        [SerializeField] private ItemAsset itemAsset;
        private bool queryDone = false;

        private void Start()
        {
            item.Data = itemAsset.ItemDefinition;
        }

        // Since we have no scene orchestrator to properly load save before scene, this querying will have to happen in Update so we have
        // time to load save data before destroying picked items
        private void LateUpdate()
        {
            if (queryDone) return;
            if (SaveManager.QueryPickedItems(item))
            {
                Destroy(gameObject);
                queryDone = true;
            }
        }

        public ItemData GetItemData() => item;
        public void SetAmount(int inAmount) => item.Amount = inAmount;

        public void Destroy()
        {
            SaveData saveData = SaveManager.GetCachedData();
            saveData.PickedItems.Add(item.InstanceId);
            SaveManager.Save(saveData);
            Destroy(gameObject);
        }
    }
}