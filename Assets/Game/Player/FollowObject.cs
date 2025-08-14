using UnityEngine;

namespace InventoryDemo.Player
{
    public class FollowObject : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset;

        // Update is called once per frame
        private void Update()
        {
            if (target)
            {
                transform.position = target.position + offset;
            }
        }
    }
}
