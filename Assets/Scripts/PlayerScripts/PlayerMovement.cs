using UnityEngine;

namespace Amer{
    public class PlayerMovement : MonoBehaviour {
        private Rigidbody rb;

        [Header("Movement")]
        private PlayerInput _playerInput;
        private float _inputAxisX => _playerInput.InputAxisX;
        private float _inputAxisZ => _playerInput.InputAxisZ;
        [field: SerializeField] private float walkSpeed = 3; 

        private void Awake() {
            if (rb == null)
                rb = GetComponent<Rigidbody>();
            if (_playerInput == null)
                _playerInput = GetComponent<PlayerInput>();
        }

        private void Update() {
            
        }

        private void FixedUpdate() {
            Vector3 movement = new Vector3(_inputAxisX, 0f, _inputAxisZ);
            movement = Vector3.ClampMagnitude(movement, 1f);
            float moveSpeedX = FixeMove(movement.x);
            float moveSpeedZ = FixeMove(movement.z);
            
            rb.linearVelocity = new Vector3(moveSpeedX, rb.linearVelocity.y, moveSpeedZ);
        }
        private float FixeMove(float inputAxis) {
            return inputAxis * walkSpeed;
        }
    }
}
