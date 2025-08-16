using UnityEngine;

namespace InventoryDemo.Player
{
    public class FollowObject : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset;

        private void Awake() => Follow(); // Initialize
        private void Update() => Follow(); // Actual gameplay logic
        private void OnValidate() => Follow(); // Follow in editor

        private void Follow()
        {
            if (target)
            {
                transform.position = target.position + offset;
            }
        }
    }
}
