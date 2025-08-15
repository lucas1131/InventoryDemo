using System;
using Game.SaveSystem;
using InventoryDemo.InventorySystem;
using InventoryDemo.Items;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace InventoryDemo.Player
{
    [Serializable]
    public struct CharacterControllerProperties
    {
        [SerializeField] [Range(1f, 5f)] public float MoveSpeed;
        [SerializeField] [Range(1f, 15f)] public float CharacterRotationSpeed;

        public CharacterControllerProperties(float moveSpeed, float characterRotationSpeed)
        {
            MoveSpeed = moveSpeed;
            CharacterRotationSpeed = characterRotationSpeed;
        }
    }

    public partial class PlayerController : MonoBehaviour
    {
        [SerializeField] private CharacterControllerProperties movementProperties;
        [SerializeField] private Animator animator;
        [SerializeField] private EquipSlot weaponSlot;
        [SerializeField] private EquipableItem editorDebugEquippedItemPrefab;
        [SerializeField] private InputActionAsset actionAsset;

        private readonly int attackTriggerID = Animator.StringToHash("Attack");
        private readonly int isMovingID = Animator.StringToHash("IsMoving");

        private CharacterController characterController;
        private CameraController cameraController;
        private PlayerAnimationEvents animationEventListener;
        private ItemPickuper itemPickuper;
        private Inventory inventory;
        private EquipableItem equippedItem;
        private InputAction cachedMoveAction;

        private void Awake()
        {
            // Only use EnsureExists for components that can be used with their default values, without setupping references.
            characterController = EnsureComponentExists<CharacterController>();
            cameraController = EnsureComponentExists<CameraController>();
            animationEventListener = EnsureComponentExists<PlayerAnimationEvents>();
            inventory = EnsureComponentExists<Inventory>();
            itemPickuper = EnsureComponentExists<ItemPickuper>();
        }

        private void Start()
        {
            itemPickuper.OnItemPickedUp += OnItemPickedUp;

            actionAsset.FindActionMap("Player").FindAction("Look").performed += OnLook;
            actionAsset.FindActionMap("Player").FindAction("Attack").performed += OnAttackAction;
            cachedMoveAction = actionAsset.FindActionMap("Player").FindAction("Move");
            actionAsset.FindActionMap("Player").FindAction("Interact").performed += ToggleInventory;

            animationEventListener.OnAttackStarted += AttackStarted;
            animationEventListener.OnAttackStarted += AttackEnded;

            SetupInventory();
            CreateEditorDebugEquipment();
            
            actionAsset.FindActionMap("Player").Enable();
        }

        private void CreateEditorDebugEquipment()
        {
            if (editorDebugEquippedItemPrefab != null && !weaponSlot.HasItemEquipped())
            {
                // We have an item equipped through the editor but nothing attached yet.
                EquipItem(Instantiate(editorDebugEquippedItemPrefab));
            }
        }

        private void Update()
        {
            Move(cachedMoveAction.ReadValue<Vector2>());

            /* Debug */
            if (Input.GetKeyDown(KeyCode.K))
            {
                SaveManager.DeleteSavedData();
            }
        }

        private T EnsureComponentExists<T>() where T : Component
        {
            T component = GetComponent<T>();
            if (component == null)
            {
                Debug.LogWarning($"Object {name} has no required component {typeof(T).Name}! Creating a default one.");
                component = gameObject.AddComponent<T>();
            }

            return component;
        }

        #region Movement

        private void OnLook(InputAction.CallbackContext action)
        {
            cameraController.Look(action.ReadValue<Vector2>());
        }

        private void Move(Vector2 input)
        {
            Vector3 direction = cameraController.GetForward() * input.y + cameraController.GetRight() * input.x;

            // Don't allow rotation to snap back to 0 when there is no input
            if (input.sqrMagnitude >= 0.00001)
            {
                float yaw = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.Euler(0f, yaw, 0f);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation,
                    movementProperties.CharacterRotationSpeed * Time.deltaTime);

                Vector3 xzDirection = new Vector3(direction.x, 0, direction.z);
                xzDirection.Normalize();
                characterController.SimpleMove(xzDirection * movementProperties.MoveSpeed);
                animator.SetBool(isMovingID, true);
            }
            else // Not moving
            {
                animator.SetBool(isMovingID, false);
            }
        }

        #endregion

        private void OnItemPickedUp(PickableItem item)
        {
            if (inventory.AddItem(item.GetItemData(), out ItemData leftoverItem) && leftoverItem.Amount <= 0)
            {
                Debug.Log($"Picked up everything, destroying item.");
                item.Destroy();
            }
            else
            {
                item.SetAmount(leftoverItem.Amount);
            }

            UpdateSaveWithInventoryData();
        }

        #region Attack

        private void OnAttackAction(InputAction.CallbackContext obj) => animator.SetTrigger(attackTriggerID);
        private void AttackStarted() => equippedItem?.Activate();
        private void AttackEnded() => equippedItem?.Deactivate();

        private void EquipItem(EquipableItem item)
        {
            weaponSlot.Equip(item);
        }

        #endregion
    }
}