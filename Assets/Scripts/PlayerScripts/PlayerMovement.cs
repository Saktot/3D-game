using UnityEngine;

namespace Amer{
    public class PlayerMovement : MonoBehaviour{
        private Rigidbody rb;

        [Header("Movement")]
        private PlayerInput _playerInput;
        private float _inputAxisX => _playerInput.InputAxisX;
        private float _inputAxisZ => _playerInput.InputAxisZ;

        [field: SerializeField] private float walkSpeed = 3;
        [SerializeField] private float rotationSpeed = 720f;

        [Header("Footsteps")]
        [SerializeField] private AudioSource footstepsSource;
        [SerializeField] private AudioClip[] footstepSounds;
        [SerializeField] private float stepInterval = 0.4f;
        [SerializeField] private float minMoveSpeed = 0.1f;

        private float stepTimer;

        private void Awake(){
            if (rb == null)
                rb = GetComponent<Rigidbody>();

            if (_playerInput == null)
                _playerInput = GetComponent<PlayerInput>();

            if (footstepsSource == null)
                footstepsSource = GetComponent<AudioSource>();
        }

        private void Update(){
            Vector3 velocity = rb.linearVelocity;
            velocity.y = 0f;

            if (velocity.magnitude > minMoveSpeed){
                stepTimer += Time.deltaTime;

                if (stepTimer >= stepInterval){
                    PlayFootstep();
                    stepTimer = 0f;
                }
            }
            else {
                stepTimer = 0f;
            }
        }

        private void FixedUpdate(){
            Vector3 movement = new Vector3(
                _inputAxisX,0f,_inputAxisZ);

            movement = Vector3.ClampMagnitude(movement, 1f);

            float moveSpeedX = FixeMove(movement.x);
            float moveSpeedZ = FixeMove(movement.z);

            rb.linearVelocity = new Vector3(
                moveSpeedX,
                rb.linearVelocity.y,
                moveSpeedZ
            );

            RotateToMovement(movement);
        }

        private void PlayFootstep(){
            if (footstepsSource == null ||
                footstepSounds == null ||
                footstepSounds.Length == 0)
                return;

            int randomIndex = Random.Range(0,footstepSounds.Length);

            footstepsSource.PlayOneShot(footstepSounds[randomIndex]);
        }

        private void RotateToMovement(Vector3 movement){
            if (movement.sqrMagnitude < 0.01f)
                return;

            Quaternion targetRotation = Quaternion.LookRotation(movement);

            Quaternion newRotation = Quaternion.RotateTowards(
                rb.rotation, targetRotation,
                rotationSpeed * Time.fixedDeltaTime);

            rb.MoveRotation(newRotation);
        }

        private float FixeMove(float inputAxis){
            return inputAxis * walkSpeed;
        }
    }
}
