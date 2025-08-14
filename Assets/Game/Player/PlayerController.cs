using System;
using UnityEngine;
using UnityEngine.InputSystem;

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

    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private CharacterControllerProperties movementProperties;
        private CharacterController characterController;
        private CameraController cameraController;
        private DefaultInputActions actions;

        private void Start()
        {
            characterController = EnsureComponentExists<CharacterController>();
            cameraController = EnsureComponentExists<CameraController>();

            actions = new DefaultInputActions();
            actions.Player.Look.performed += OnLook;
            actions.Player.Fire.performed += OnAttack;
            actions.Enable();
        }


        private void Update()
        {
            Move(actions.Player.Move.ReadValue<Vector2>());
        }

        private T EnsureComponentExists<T>() where T : Component
        {
            T component = GetComponent<T>();
            if (component == null)
            {
                Debug.LogWarning($"Object {name} has no required component {nameof(T)}! Creating a default one.");
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
            Debug.Log($"Movement: {input}");
            
            Vector3 direction = cameraController.GetForward() * input.y + cameraController.GetRight() * input.x;

            // Don't allow rotation to snap back to 0 when there is no input
            if (input.sqrMagnitude >= 0.0001)
            {
                float yaw = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.Euler(0f, yaw, 0f);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, movementProperties.CharacterRotationSpeed * Time.deltaTime);
            }

            characterController.SimpleMove(direction * movementProperties.MoveSpeed);
        }
        #endregion
        
        private void OnAttack(InputAction.CallbackContext obj)
        {
            throw new NotImplementedException();
        }

    }
}