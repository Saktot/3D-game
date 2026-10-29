using UnityEngine;
using UnityEngine.InputSystem;

namespace Amer{
    public class CameraFollow : MonoBehaviour{
        [Header("CameraSettings")]
        [SerializeField] private Transform target;
        [SerializeField] private float smoothSpeed = 8f;
        [SerializeField] private float cameraAngle = 50f;

        [Header("ZoomSettings")]
        [SerializeField] private float zoomSpeed = 2f;
        [SerializeField] private float minZoom = 7f;
        [SerializeField] private float maxZoom = 15f;
        [SerializeField] private float currentZoom = 10f;

        private Quaternion fixedRotation;

        private void Start(){
            fixedRotation = Quaternion.Euler(cameraAngle, 0f, 0f);
            currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
        }
        private void LateUpdate(){
            if (target == null)
                return;

            float scroll = Input.GetAxis("Mouse ScrollWheel");

            currentZoom -= scroll * zoomSpeed * 10f;
            currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

            Vector3 offset = new Vector3(0f,currentZoom,-currentZoom * 0.7f);

            Vector3 desiredPosition = target.position + offset;

            transform.position = Vector3.Lerp(
                transform.position,
                desiredPosition,
                smoothSpeed * Time.deltaTime
            );

            transform.rotation = fixedRotation;
        }
    }
}