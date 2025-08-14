using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InventoryDemo.Player
{
    [Serializable]
    public struct CharacterControllerProperties
    {
        [SerializeField] public float MoveSpeed;
        
        public CharacterControllerProperties(float moveSpeed)
        {
            MoveSpeed = moveSpeed;
        }
    }
    
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private CharacterControllerProperties movementProperties;
        private CharacterController characterController;
        private CameraController cameraController;

        private void Start()
        {
            characterController = EnsureComponentExists<CharacterController>();
            cameraController = EnsureComponentExists<CameraController>();
            
            DefaultInputActions actions = new();
            actions.Player.Look.performed += OnLook;
            actions.Player.Move.performed += OnMove;
            actions.Enable();
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

        private void OnLook(InputAction.CallbackContext action)
        {
            cameraController.Look(action.ReadValue<Vector2>());
        }

        private void OnMove(InputAction.CallbackContext action)
        {
            // Debug.Log($"OnMove: {action.ReadValue<Vector2>()}");
            // characterController.Move(action.ReadValue<Vector2>() * movementProperties.MoveSpeed);
        }
    }
}
