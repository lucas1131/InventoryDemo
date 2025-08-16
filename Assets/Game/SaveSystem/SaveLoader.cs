using InventoryDemo.SaveSystem;
using UnityEngine;

namespace Game.SaveSystem
{
    public class SaveLoader : MonoBehaviour
    {
        private void Awake()
        {
            // bad synchronous save loading :c
            if (SaveManager.cachedSavedData == null)
            {
                SaveManager.Load();
            }
        }
    }
}