using System;
using InventoryDemo.Items.ItemData;
using InventoryDemo.SaveSystem;
using UnityEditor;
using UnityEngine;

namespace InventoryDemo.Items
{
    public class PickableItem : MonoBehaviour
    {
        [SerializeField] private ItemData.ItemData item;
        [SerializeField] private ItemAsset itemAsset;
        [SerializeField] private Rigidbody rb; 
        private bool queryDone = false;

        private const float SnapToGroundHeight = 5f;
        public Rigidbody Rigidbody => rb; 

        private void Awake()
        {
            item.Data = itemAsset.ItemDefinition;

            if (item.InstanceId.Equals(Guid.Empty.ToString()))
            {
                item.SetInstanceId(Guid.NewGuid());
            }

            if (rb == null)
            {
                rb = GetComponent<Rigidbody>();
            }
        }

        #region Editor validation and snap to ground

        private void OnValidate()
        {
#if UNITY_EDITOR
            // Make sure prefab itself has empty ID, this should be populated either OnValidate for editor or Awake for runtime objects
            if (PrefabUtility.IsPartOfPrefabAsset(this))
            {
                item.SetInstanceId(Guid.Empty);
                return;
            }
#endif

            // Make items placed in world generate this during editor saving
            if (item.InstanceId.Equals(Guid.Empty.ToString()))
            {
                item.SetInstanceId(Guid.NewGuid());
            }

            SnapToGround();
        }

        private void OnDrawGizmosSelected()
        {
            Vector3 origin = transform.position + Vector3.up * SnapToGroundHeight;
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(origin, .1f);
            Debug.DrawRay(origin, Vector3.down * 100f, Color.green);
            if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Terrain")))
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(hit.point, .1f);
            }
        }

        private void SnapToGround()
        {
            // Try to snap a bit above the object
            Vector3 origin = transform.position + Vector3.up * SnapToGroundHeight;
            if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Terrain")))
            {
                float extentsY = GetComponent<BoxCollider>()?.bounds.extents.y ?? 0;
                transform.position = hit.point + Vector3.up * extentsY;
            }
        }
        
        #if UNITY_EDITOR
        public void Editor_RegenerateID() => item.SetInstanceId(Guid.NewGuid());
        #endif

        #endregion

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

        public ItemData.ItemData GetItemData() => item;
        public void SetAmount(int inAmount) => item.Amount = inAmount;

        public void Destroy()
        {
            SaveData saveData = SaveManager.GetCachedData();
            saveData.PickedItemsId.Add(item.InstanceId);
            SaveManager.Save(saveData);
            Destroy(gameObject);
        }
    }
}