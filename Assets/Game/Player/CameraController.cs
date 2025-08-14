using UnityEngine;

namespace InventoryDemo.Player
{
    // this comes from an older project of mine so its not properly using the new-ish input system
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform targetCamera;
        [SerializeField] private Transform pivot;

        [SerializeField] [Range(5f, 20f)] private float sensitivity = 5f;
        [SerializeField] [Range(1f, 5f)] private float zoomSensitivity = 3f;
        [SerializeField] [Range(5f, 20f)] private float cameraZoomInterpolationSpeed = 10f;
        [SerializeField] [Range(0f, 10f)] private float minDistance = 0f;
        [SerializeField] [Range(0f, 10f)] private float maxDistance = 10f;
        [SerializeField] [Range(-89f, 89f)] private float minPitch = -40f;
        [SerializeField] [Range(-89f, 89f)] private float maxPitch = 70f;
        
        private Vector3 velocity;
        private float yaw;
        private float pitch;
        private float distance = 3f;

        private void Start()
        {
            if (Camera.main != null)
            {
                targetCamera ??= Camera.main.transform;
            }
        }

        private void OnValidate()
        {
            if (minDistance > maxDistance)
            {
                minDistance = maxDistance;
            }
        }

        private void Update()
        {
            if (pivot != null)
            {
                FollowTarget();
                RotateAroundTarget();
            }
        }
        
        public void Look(Vector2 lookInput)
        {
            // Rotate yaw & pitch from input
            yaw += lookInput.x * sensitivity * Time.deltaTime;
            pitch -= lookInput.y * sensitivity * Time.deltaTime;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        }

        private void FollowTarget()
        {
            float mouseWheelY = -Input.mouseScrollDelta.y; // TODO pass this through proper input system
            distance = Mathf.Clamp(distance + mouseWheelY * zoomSensitivity, minDistance, maxDistance);

            Vector3 targetPosition = new Vector3(0f, 0f, -distance);
            
            targetCamera.transform.localPosition = Vector3.Lerp(targetCamera.transform.localPosition, targetPosition, Time.deltaTime * cameraZoomInterpolationSpeed);
        }

        private void RotateAroundTarget()
        {
            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
            pivot.transform.rotation = rotation;
        }
    }
}