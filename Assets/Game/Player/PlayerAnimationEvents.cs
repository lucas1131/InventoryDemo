using JetBrains.Annotations;
using UnityEngine;

namespace InventoryDemo.Player
{
    public class PlayerAnimationEvents : MonoBehaviour
    {
        public delegate void OnAttackAnimationEvent();

        public event OnAttackAnimationEvent OnAttackStarted;
        public event OnAttackAnimationEvent OnAttackEnded;

        private void OnDestroy()
        {
            OnAttackStarted = null;
            OnAttackEnded = null;
        }

        [UsedImplicitly]
        public void AttackStarted()
        {
            OnAttackStarted?.Invoke();
        }

        [UsedImplicitly]
        public void AttackEnded()
        {
            OnAttackEnded?.Invoke();
        }
    }
}
