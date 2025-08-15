using System;
using UnityEngine;

namespace InventoryDemo.Items
{
    public class PickableItem : MonoBehaviour
    {
        public void Destroy() => Destroy(gameObject);
    }
}