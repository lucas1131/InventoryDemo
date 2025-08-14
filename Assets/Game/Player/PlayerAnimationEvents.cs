using JetBrains.Annotations;
using UnityEngine;

namespace InventoryDemo.Player
{
    public class PlayerAnimationEvents : MonoBehaviour
    {
        [SerializeField] private GameObject WeaponHitbox;

        private void Start()
        {
            WeaponHitbox = new GameObject();
            WeaponHitbox.gameObject.SetActive(false);
        }
        
        [UsedImplicitly]
        public void AttackStarted()
        {
            Debug.Log($"AttackStarted event called");
            WeaponHitbox.gameObject.SetActive(true);
        }

        [UsedImplicitly]
        public void AttackEnded()
        {
            Debug.Log($"AttackEnded event called");
            WeaponHitbox.gameObject.SetActive(false);
        }
    }
}
