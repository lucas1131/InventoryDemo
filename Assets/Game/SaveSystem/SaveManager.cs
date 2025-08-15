using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using InventoryDemo.Items;
using UnityEngine;

namespace Game.SaveSystem
{
    [Serializable]
    public struct SaveData
    {
        public List<ItemData> Items;   
        public List<Guid> PickedItems;
    }
    
    public class SaveManager
    {
        private static string SavePath => Path.Combine(Application.persistentDataPath, "save.json");
        internal static SaveData? cachedSavedData;

        public static bool Save(SaveData data)
        {
            try
            {
                File.WriteAllText(SavePath, JsonUtility.ToJson(data));
                cachedSavedData = data;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return false;
            }

            return true;
        }

        public static SaveData Load()
        {
            cachedSavedData = File.Exists(SavePath) 
                ? JsonUtility.FromJson<SaveData>(File.ReadAllText(SavePath)) 
                : new SaveData();
            
            return cachedSavedData.Value;
        }

        public static void DeleteSavedData()
        {
            Debug.Log($"Deleting saved data");
            if (File.Exists(SavePath))
            {
                File.Delete(SavePath);
            }
        }
        
        public static SaveData GetCachedData() => cachedSavedData ?? new SaveData();
        public static bool QueryPickedItems(ItemData item)
        {
            // If we never loaded we cant have anything pickup
            return cachedSavedData != null && cachedSavedData.Value.PickedItems.Contains(item.InstanceId);
        }
        
    }
}